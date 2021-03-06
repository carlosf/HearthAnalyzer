﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HearthAnalyzer.Core.Cards;
using HearthAnalyzer.Core.Deathrattles;
using HearthAnalyzer.Core.Interfaces;

namespace HearthAnalyzer.Core
{
    /// <summary>
    /// Represents a player in Hearthstone
    /// </summary>
    public abstract class BasePlayer : IDamageableEntity, IAttacker, IEquatable<BasePlayer>
    {
        internal int attacksThisTurn = 0;
        internal List<BaseCard> hand; 

        protected BasePlayer(int id = -1)
        {
            this.Id = id;
            this.hand = new List<BaseCard>();
            this.Deck = new Deck();
            this.Health = 30;
            this.Armor = 0;
            this.Mana = 0;
            this.MaxMana = 0;
            this.Graveyard = new List<BaseCard>();
            this.Weapon = null;
            this.HeroPower = null;
        }

        /// <summary>
        /// The id for this player
        /// </summary>
        public int Id;

        /// <summary>
        /// The name of this player's hero
        /// </summary>
        public string Name;

        /// <summary>
        /// The game board the player is playing on
        /// </summary>
        public GameBoard Board;

        /// <summary>
        /// The players hand
        /// </summary>
        public ReadOnlyCollection<BaseCard> Hand
        {
            get { return this.hand.AsReadOnly(); }
        }

        /// <summary>
        /// The player's deck
        /// </summary>
        public Deck Deck;

        /// <summary>
        /// The player's graveyard
        /// </summary>
        public List<BaseCard> Graveyard;

        /// <summary>
        /// The player's health
        /// </summary>
        public int Health;

        /// <summary>
        /// The player's armor value
        /// </summary>
        public int Armor;

        /// <summary>
        /// The player's remaining mana
        /// </summary>
        public int Mana;

        /// <summary>
        /// The player's maximum mana
        /// </summary>
        public int MaxMana;

        /// <summary>
        /// The player's current overload amount
        /// </summary>
        public int Overload;

        /// <summary>
        /// The player's pending overload amount
        /// </summary>
        public int PendingOverload;

        /// <summary>
        /// The temporary attack buff added to this player
        /// </summary>
        public int TemporaryAttackBuff;

        /// <summary>
        /// The player's status effects
        /// </summary>
        public PlayerStatusEffects StatusEffects;

        /// <summary>
        /// The player's weapon
        /// </summary>
        public BaseWeapon Weapon;

        /// <summary>
        /// The player's hero power
        /// </summary>
        public BaseCard HeroPower;

        /// <summary>
        /// The amount of bonus spell power provided by minions
        /// </summary>
        public int BonusSpellPower
        {
            get { return GameEngine.GameState.CurrentPlayerPlayZone.Where(card => card != null).Sum(card => ((BaseMinion)card).BonusSpellPower); }
        }

        /// <summary>
        /// Plays a card
        /// </summary>
        /// <param name="card">The card to play</param>
        /// <param name="subTarget">The sub target for this card, usually for targeting batlle cry spells</param>
        /// <param name="cardEffect">The card effect to use</param>
        /// <param name="gameboardPos">The position on the gameboard to place the card (if applicable). A value of -1 means play it in the next available slot</param>
        public void PlayCard(BaseCard card, IDamageableEntity subTarget, int gameboardPos = -1, CardEffect cardEffect = CardEffect.NONE)
        {
            // Is it even our turn to play?
            var gameState = GameEngine.GameState;
            if (gameState.CurrentPlayer != this)
            {
                throw new InvalidOperationException(string.Format("You can't play out of turn! It is currently {0}'s turn", gameState.CurrentPlayer));
            }

            // Check if it exists in the player's hand
            BaseCard cardInHand = this.Hand.FirstOrDefault(c => c.Equals(card));    
            
            if (cardInHand == null)
            {
                throw new InvalidOperationException(string.Format("You can't play a card that's not in hand (or deck if force summoned from there)! {0}", card));
            }

            // Check if we have enough mana to make the play
            if (this.Mana < cardInHand.CurrentManaCost)
            {
                throw new InvalidOperationException(string.Format("Not enough mana {0} to play that card {1}!", this.Mana, card.CurrentManaCost));
            }

            var minionCard = cardInHand as BaseMinion;
            if (minionCard != null)
            {
                this.SummonMinion(minionCard, subTarget, gameboardPos, cardEffect, forceSummoned: false, cardSource: this.hand);
            }

            var spellCard = cardInHand as BaseSpell;
            if (spellCard != null)
            {
                this.PlaySpell(spellCard, subTarget, cardEffect);
            }

            var weaponCard = cardInHand as BaseWeapon;
            if (weaponCard != null)
            {
                this.PlayWeapon(weaponCard, subTarget);
            }

            // Register any triggered effects
            var triggeredEffectCard = cardInHand as ITriggeredEffectOwner;
            if (triggeredEffectCard != null)
            {
                triggeredEffectCard.RegisterEffect();
            }

            // Trigger any deathrattles
            GameEngine.TriggerDeathrattles();

            GameEngine.CheckForGameEnd();
        }

        /// <summary>
        /// Summons a minion onto the board
        /// </summary>
        /// <param name="minion">The minion to summon</param>
        /// <param name="subTarget">The sub target for this card, usually for targetting battle cry spells</param>
        /// <param name="gameboardPos">The position on the gameboard to place the card. A value of -1 means play it in the next available slot</param>
        /// <param name="cardEffect">The card effect to use</param>
        /// <param name="forceSummoned">Whether or not to force summon the minion, this means no battle cry effects</param>
        /// <param name="cardSource">Where, if any, should we remove the card from</param>
        public void SummonMinion(BaseMinion minion, IDamageableEntity subTarget, int gameboardPos = -1, CardEffect cardEffect = CardEffect.NONE, bool forceSummoned = false, ICollection<BaseCard> cardSource = null)
        {
            var gameState = GameEngine.GameState;

            if (cardSource != null)
            {
                if (!cardSource.Contains(minion))
                {
                    throw new InvalidOperationException(string.Format("Card source doesn't contain {0}", minion));
                }

                cardSource.Remove(minion);
            }

            // Check if there are too many minions on the board
            var playZone = gameState.CurrentPlayerPlayZone;
            var playZoneCount = playZone.Count(slot => slot != null);
            if (playZoneCount >= Constants.MAX_CARDS_ON_BOARD)
            {
                throw new InvalidOperationException(string.Format("There are too many cards ({0}) in the playzone!", playZoneCount));
            }

            int summonPosition = gameboardPos;
            if (gameboardPos == -1)
            {
                summonPosition = GameEngine.GameState.CurrentPlayerPlayZone.Count(card => card != null);
            }

            // If there is a card already in the target gameboard position, shift everything to the right and place it there
            if (playZone[summonPosition] != null)
            {
                for (int i = playZone.Count - 1; i > gameboardPos; i--)
                {
                    playZone[i] = playZone[i - 1];
                }
            }

            // Place the minion on the board
            playZone[summonPosition] = minion;

            // Set the time the card was played
            minion.TimePlayed = DateTime.Now;

            // Start the minion as exhausted unless it has charge
            if (!minion.HasCharge)
            {
                minion.ApplyStatusEffects(MinionStatusEffects.EXHAUSTED);
            }

            minion.ResetAttacksThisTurn();

            // Remove mana from the player
            if (!forceSummoned)
            {
                this.Mana -= minion.CurrentManaCost;
            }

            // Call the card's card effect
            var multiCard = minion as IMultiCardEffectMinion;
            if (multiCard != null && !forceSummoned)
            {
                multiCard.UseCardEffect(cardEffect, subTarget);
            }

            // Fire minion placed event
            if (!forceSummoned)
            {
                GameEventManager.MinionPlaced(minion);
            }

            // Register deathrattle if applicable
            var deathrattleCard = minion as IDeathrattler;
            if (deathrattleCard != null)
            {
                deathrattleCard.RegisterDeathrattle();
            }

            if (!forceSummoned)
            {
                // call the card's battlecry 
                var battlecryCard = minion as IBattlecry;
                if (battlecryCard != null)
                {
                    if (subTarget is BaseMinion && ((BaseMinion)subTarget).IsStealthed)
                    {
                        throw new InvalidOperationException("Can't target stealthed minions");
                    }

                    battlecryCard.Battlecry(subTarget);
                }
            }

            // Fire card played event
            GameEventManager.MinionPlayed(minion);
        }

        /// <summary>
        /// Plays a spell
        /// </summary>
        /// <param name="spell">The spell to play</param>
        /// <param name="subTarget">The sub target for this spell card if applicable</param>
        /// <param name="cardEffect">The card effect to use</param>
        public void PlaySpell(BaseSpell spell, IDamageableEntity subTarget = null, CardEffect cardEffect = CardEffect.NONE)
        {
            if (subTarget != null && subTarget is BaseMinion)
            {
                if (((BaseMinion) subTarget).IsImmuneToSpellTarget || ((BaseMinion) subTarget).IsStealthed)
                {
                    throw new InvalidOperationException("Can't target minion that is immune to spell targeting or is stealthed");
                }
            }

            // Remove it from the player's hand
            this.hand.Remove(spell);

            // Remove mana from the player
            this.Mana -= spell.CurrentManaCost;

            // Fire spell casting event
            bool shouldAbort;
            GameEventManager.SpellCasting(spell, subTarget, out shouldAbort);

            // Check if we need to abort the spell or redirect
            if (!shouldAbort)
            {
                spell.Activate(subTarget, cardEffect);
            }

            // Fire spell casted event (if we need to)
        }

        /// <summary>
        /// Plays a weapon
        /// </summary>
        /// <param name="weapon">The weapon to play</param>
        /// <param name="subTarget">The sub target for this weapon if applicable</param>
        public void PlayWeapon(BaseWeapon weapon, IDamageableEntity subTarget = null)
        {
            this.hand.Remove(weapon);

            this.Mana -= weapon.CurrentManaCost;

            weapon.WeaponOwner = this;
            this.Weapon = weapon;

            // Register deathrattle if applicable
            var deathrattleCard = weapon as IDeathrattler;
            if (deathrattleCard != null)
            {
                deathrattleCard.RegisterDeathrattle();
            }

            // Call the card's battle cry
            var battlecryWeapon = weapon as IBattlecry;
            if (battlecryWeapon != null)
            {
                battlecryWeapon.Battlecry(subTarget);
            }

            // Fire card played event
            // TODO: Add weapon played event?
        }

        /// <summary>
        /// Draws a card from the player's deck and puts it into his hand
        /// </summary>
        public void DrawCard()
        {
            this.DrawCards(1);
        }

        /// <summary>
        /// Draws n cards from the player's deck and puts it into his hand
        /// </summary>
        /// <param name="n"></param>
        public void DrawCards(int n = 1)
        {
            var drawnCards = this.Deck.DrawCards(n);

            foreach (var drawnCard in drawnCards)
            {
                drawnCard.Owner = this;

                if (drawnCard is FatigueCard)
                {
                    // Oh noes!
                    this.TakeDamage(((FatigueCard)drawnCard).FatigueDamage);
                    GameEngine.CheckForGameEnd();
                }
                else
                {
                    Logger.Instance.Info(string.Format("{0}: Drew {1}", this.LogString(), drawnCard));
                    this.AddCardToHand(drawnCard);
                }
            }
        }

        /// <summary>
        /// Applies the provided effects to the player
        /// </summary>
        /// <param name="effects">The effects to apply to the player</param>
        public void ApplyStatusEffects(PlayerStatusEffects effects)
        {
            if (effects.HasFlag(PlayerStatusEffects.WINDFURY))
            {
                // If we're applying windfury, we need to unexhaust the player if it has
                // only attacked once so far this turn
                if (this.attacksThisTurn < 2)
                {
                    this.RemoveStatusEffects(PlayerStatusEffects.EXHAUSTED);
                }
            }

            this.StatusEffects |= effects;
        }

        /// <summary>
        /// Removes the provided effects from the player
        /// </summary>
        /// <param name="effects">The effects to remove</param>
        public void RemoveStatusEffects(PlayerStatusEffects effects)
        {
            this.StatusEffects = this.StatusEffects & ~effects;
        }

        /// <summary>
        /// This kills the player
        /// </summary>
        public void Die()
        {
            // Oh no, we died, inform the Game Engine.
            GameEngine.DeadPlayersThisTurn.Add(this);
        }

        /// <summary>
        /// Returns a list of playable cards given the player's current mana
        /// </summary>
        /// <returns>The list of playable cards given the player's current mana</returns>
        public List<BaseCard> GetPlayableCards()
        {
            return this.Hand.Where(c => c.OriginalManaCost <= this.Mana).ToList();
        }

        /// <summary>
        /// Returns whether or not the player is frozen
        /// </summary>
        public bool IsFrozen { get { return this.StatusEffects.HasFlag(PlayerStatusEffects.FROZEN); } }

        /// <summary>
        /// Returns whether or not the player is immune to damage
        /// </summary>
        public bool IsImmuneToDamage { get { return this.StatusEffects.HasFlag(PlayerStatusEffects.IMMUNE_TO_DAMAGE); } }

        /// <summary>
        /// Returns whether or not the player has windfury
        /// </summary>
        public bool HasWindfury { get { return this.StatusEffects.HasFlag(PlayerStatusEffects.WINDFURY); } }

        /// <summary>
        /// Returns whether or not the player is exhausted
        /// </summary>
        public bool IsExhausted { get { return this.StatusEffects.HasFlag(PlayerStatusEffects.EXHAUSTED); } }

        /// <summary>
        /// Returns whether or not the player can attack
        /// </summary>
        public bool CanAttack { get { return !this.IsFrozen && !this.IsExhausted; } }

        #region IAttacker

        public int GetCurrentAttackPower()
        {
            return this.TemporaryAttackBuff;
        }

        public void Attack(IDamageableEntity target)
        {
            if (!this.CanAttack)
            {
                throw new InvalidOperationException("Player can't attack yet!");
            }

            if (this.Weapon != null)
            {
                this.Weapon.Attack(target);
            }
            else if (this.TemporaryAttackBuff > 0)
            {
                // Fire attacking event
                bool shouldAbort;
                GameEventManager.Attacking(this, target, isRetaliation: false, shouldAbort: out shouldAbort);
            }
            else
            {
                throw new InvalidOperationException("Player has no attack damage to attack with!");
            }

            this.attacksThisTurn++;

            if (!this.HasWindfury || (this.HasWindfury && this.attacksThisTurn >= 2))
            {
                this.ApplyStatusEffects(PlayerStatusEffects.EXHAUSTED);
            }
        }
        
        /// <summary>
        /// Resets the number of attacks this minion has performed this turn.
        /// </summary>
        public void ResetAttacksThisTurn()
        {
            this.attacksThisTurn = 0;
        }

        /// <summary>
        /// Adds a mana crystal to the player up to a maximum
        /// </summary>
        public void AddManaCrystal()
        {
            this.MaxMana = Math.Min(this.MaxMana + 1, Constants.MAX_MANA_CAPACITY);
        }

        /// <summary>
        /// Adds a card to the player's hand if it's not full.
        /// If it is full, then the card is destroyed
        /// </summary>
        /// <param name="card">The card to add</param>
        public void AddCardToHand(BaseCard card)
        {
            if (this.Hand.Count >= Constants.MAX_CARDS_IN_HAND)
            {
                Logger.Instance.Info(string.Format("{0}: {1} was milled because the hand is too full!", this.LogString(), card));
                this.Graveyard.Add(card);
            }
            else
            {
                this.hand.Add(card);
                card.Owner = this;
            }
        }

        /// <summary>
        /// Adds a list of cards to the player's hand
        /// </summary>
        /// <param name="cards">The cards to add</param>
        public void AddCardsToHand(List<BaseCard> cards)
        {
            cards.ForEach(this.AddCardToHand);
        }

        /// <summary>
        /// Removes a card from the player's hand
        /// </summary>
        /// <param name="card">The card to remove</param>
        public void RemoveCardFromHand(BaseCard card)
        {
            if (!this.hand.Contains(card))
            {
                throw new InvalidOperationException(string.Format("Card {0} was not found in hand!", card));
            }

            this.hand.Remove(card);
        }

        #endregion

        #region IDamageableEntity

        public void TakeDamage(int damage)
        {
            if (this.IsImmuneToDamage) return;

            // Take damage to armor first
            int damageToTake = damage;
            int armorDamage = Math.Min(this.Armor, damageToTake);
            if (armorDamage > 0)
            {
                this.Armor -= armorDamage;
                damageToTake -= armorDamage;
            }

            if (damageToTake > 0)
            {
                this.Health -= damageToTake;
            }

            // Fire damage dealt event
            GameEventManager.DamageDealt(this, damage);

            if (this.Health <= 0)
            {
                this.Die();
            }
        }

        public void TakeHealing(int healAmount)
        {
            this.Health = Math.Min(this.Health + healAmount, 30);

            // Fire heal dealt event
        }

        public void TakeBuff(int attackBuff, int healthBuff)
        {
            throw new InvalidOperationException("Player's can't receive buffs.");
        }

        public void TakeTemporaryBuff(int attackBuff)
        {
            if (this.Weapon == null)
            {
                this.TemporaryAttackBuff += attackBuff;
            }
            else
            {
                this.Weapon.TakeTemporaryBuff(attackBuff);
            }
        }

        #endregion
        
        /// <summary>
        /// Returns a log friendly string for this player
        /// </summary>
        public string LogString()
        {
            return string.Format("{0}[{1}]", (this == GameEngine.GameState.Player) ? "Player" : "Opponent", this.Id);
        }

        #region IEquatable

        public bool Equals(BasePlayer other)
        {
            return this.Id == other.Id;
        }

        #endregion
    }

    /// <summary>
    /// Status effects for the player
    /// </summary>
    [Flags]
    public enum PlayerStatusEffects
    {
        FROZEN = 1,
        IMMUNE_TO_DAMAGE = 2,
        EXHAUSTED = 4,
        WINDFURY = 8
    }
}
