﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Cards;
using HearthAnalyzer.Core.Cards.Minions;
using HearthAnalyzer.Core.Cards.Spells;
using HearthAnalyzer.Core.Cards.Weapons;
using HearthAnalyzer.Core.Heroes;
using NUnit.Framework;

namespace HearthAnalyzer.Core.Tests
{
    [TestFixture]
    public class GameEngineTests : BaseTestSuite
    {
        private BasePlayer player;
        private BasePlayer opponent;
        private bool gameEnded;
        private GameEngine.GameResult? gameResult;

        private readonly string DeckTestDataPath = @".\TestData\Decks";

        [SetUp]
        public void Setup()
        {
            player = HearthEntityFactory.CreatePlayer<Warlock>();
            opponent = HearthEntityFactory.CreatePlayer<Warlock>();

            string zooLockDeckFile = Path.Combine(DeckTestDataPath, "ZooLock.txt");

            player.Deck = Deck.FromDeckFile(zooLockDeckFile);
            opponent.Deck = Deck.FromDeckFile(zooLockDeckFile);

            GameEngine.Initialize(player, opponent);
            gameEnded = false;
            gameResult = null;
            GameEngine.GameEnded += OnGameEnded;
        }

        [TearDown]
        public void Cleanup()
        {
            GameEngine.Uninitialize();
        }

        /// <summary>
        /// Verify mulligan logic
        /// </summary>
        [Test]
        public void Mulligan()
        {
            GameEngine.DealPreMulligan();

            // Mulligan the player's hand completely
            var handCount = player.Hand.Count;
            BaseCard[] originalHand = new BaseCard[handCount];
            player.Hand.CopyTo(originalHand, 0);
            GameEngine.Mulligan(player, originalHand);

            // Verify that the player's hand don't contain the same cards
            Assert.IsFalse(player.Hand.SequenceEqual(originalHand.ToList()), "Verify the hand is new.");
            Assert.AreEqual(Constants.MAX_CARDS_IN_DECK - handCount, player.Deck.Cards.Count, "Verify deck size after mulligan");
            Assert.IsFalse(originalHand.Except(player.Deck.Cards).Any(), "Verify the original cards are back in the deck");
            Assert.IsTrue(GameEngine.PlayerMulliganed, "Verify that the player mulliganed flag is set");

            // For the opponent, choose not to mulligan any cards
            var opponentHandCount = opponent.Hand.Count;
            BaseCard[] opponentOriginalHand = new BaseCard[opponentHandCount];
            opponent.Hand.CopyTo(opponentOriginalHand, 0);
            GameEngine.Mulligan(opponent, null);

            // Verify that the opponent's hand contains the same cards
            Assert.IsFalse(opponentOriginalHand.Except(opponent.Hand).Any(), "Verify the hand has the same cards");
            Assert.IsTrue(GameEngine.OpponentMulliganed, "Verify that the opponent mulliganed flag is set");
        }

        /// <summary>
        /// Verify the post mulligan phase
        /// </summary>
        [Test]
        public void PostMulligan()
        {
            var currentPlayer = GameEngine.GameState.CurrentPlayer;
            var waitingPlayer = GameEngine.GameState.WaitingPlayer;

            GameEngine.DealPreMulligan();

            GameEngine.Mulligan(player, null);
            GameEngine.Mulligan(opponent, null);

            // Verify that it's now turn 1
            Assert.AreEqual(1, GameEngine.GameState.TurnNumber, "Verify turn number");
            Assert.AreEqual(1, currentPlayer.MaxMana, "Verify current player max mana");
            Assert.AreEqual(1, currentPlayer.Mana, "Verify current player mana");

            Assert.IsTrue(waitingPlayer.Hand.Any(card => card is TheCoin), "Verify that the waiting player got the coin");
            Assert.AreEqual(5, waitingPlayer.Hand.Count, "Verify the waiting player hand size");

            Assert.AreEqual(4, currentPlayer.Hand.Count, "Verify current player hand size");
        }

        /// <summary>
        /// Verify ending a turn
        /// </summary>
        [Test]
        public void EndTurn()
        {
            GameEngine.DealPreMulligan();

            GameEngine.Mulligan(player, null);
            GameEngine.Mulligan(opponent, null);

            var waitingPlayer = GameEngine.GameState.WaitingPlayer;
            GameEngine.EndTurn();

            Assert.AreEqual(waitingPlayer, GameEngine.GameState.CurrentPlayer, "Verify the current player has switched to the previously waiting player");
        }

        /// <summary>
        /// Verify mana cannot go beyond capacity
        /// </summary>
        [Test]
        public void AddManaCapped()
        {
            GameEngine.GameState.WaitingPlayer.MaxMana = 10;
            GameEngine.EndTurn();

            var currentPlayer = GameEngine.GameState.CurrentPlayer;
            Assert.AreEqual(Constants.MAX_MANA_CAPACITY, currentPlayer.MaxMana, "Verify max mana doesn't exceed maximum value");
            Assert.AreEqual(Constants.MAX_MANA_CAPACITY, currentPlayer.Mana, "Verify mana is replenished");
        }

        /// <summary>
        /// Verify death due to fatigue
        /// </summary>
        [Test]
        public void GameEndDueToFatigueDamage()
        {
            player.Health = 1;
            player.Deck = new Deck();

            player.DrawCard();

            // wait for game end
            Task.Factory.StartNew(() => this.WaitUntilGameEnded(250, 8)).Wait();

            // Game should have ended
            Assert.IsTrue(this.gameEnded, "Verify the game has ended");
            Assert.AreEqual(this.gameResult, GameEngine.GameResult.LOSE, "Verify we lost because we fatigued ourself to death");
        }

        /// <summary>
        /// Verify the game ended due to attack
        /// </summary>
        [Test]
        public void GameEndDueToAttack()
        {
            opponent.Health = 1;
            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            GameEngine.GameState.Board.PlayerPlayZone.Add(yeti);
            yeti.RemoveStatusEffects(MinionStatusEffects.EXHAUSTED);

            yeti.Attack(opponent);

            Task.Factory.StartNew(() => this.WaitUntilGameEnded(250, 8)).Wait();

            Assert.IsTrue(this.gameEnded, "Verify the game has ended");
            Assert.AreEqual(this.gameResult, GameEngine.GameResult.WIN, "Verify we won because we killed the opponent");
        }

        /// <summary>
        /// Verify the game was a draw
        /// </summary>
        [Test]
        public void GameDraw()
        {
            player.Health = 1;
            opponent.Health = 1;

            GameEngine.GameState.CurrentPlayer = player;
            var hellfire = HearthEntityFactory.CreateCard<Hellfire>();
            hellfire.Owner = player;
            hellfire.CurrentManaCost = 0;
            player.AddCardToHand(hellfire);
            player.PlayCard(hellfire, null);

            Task.Factory.StartNew(() => this.WaitUntilGameEnded(250, 8)).Wait();

            Assert.IsTrue(this.gameEnded, "Verify the game has ended");
            Assert.AreEqual(this.gameResult, GameEngine.GameResult.DRAW, "Verify it was a draw");
        }

        /// <summary>
        /// Verify the game ends if the opponent is killed as a result of playing a card
        /// </summary>
        [Test]
        public void GameEndDueToCardPlayed()
        {
            opponent.Health = 1;
            var stormpikeCommando = HearthEntityFactory.CreateCard<StormpikeCommando>();
            stormpikeCommando.CurrentManaCost = 0;
            player.AddCardToHand(stormpikeCommando);

            GameEngine.GameState.CurrentPlayer = player;
            player.PlayCard(stormpikeCommando, opponent);

            Task.Factory.StartNew(() => this.WaitUntilGameEnded(250, 8)).Wait();

            Assert.IsTrue(this.gameEnded, "Verify the game has ended");
            Assert.AreEqual(this.gameResult, GameEngine.GameResult.WIN, "Verify we won because we killed the opponent");
        }

        /// <summary>
        /// Verify overload logic
        /// </summary>
        [Test]
        public void Overload()
        {
            player.MaxMana = Constants.MAX_MANA_CAPACITY;
            var stormAxe = HearthEntityFactory.CreateCard<StormforgedAxe>();
            stormAxe.CurrentManaCost = 0;
            player.AddCardToHand(stormAxe);

            GameEngine.GameState.CurrentPlayer = player;
            player.PlayCard(stormAxe, null);

            Assert.AreEqual(stormAxe, player.Weapon, "Verify axe was equipped");
            Assert.AreEqual(1, player.PendingOverload, "Verify player's pending overload");

            GameEngine.EndTurn();
            GameEngine.EndTurn();

            Assert.AreEqual(0, player.PendingOverload, "Verify pending overload is now gone");
            Assert.AreEqual(1, player.Overload, "Verify overload");
            Assert.AreEqual(Constants.MAX_MANA_CAPACITY - 1, player.Mana, "Verify mana less overload");

            GameEngine.EndTurn();
            GameEngine.EndTurn();

            Assert.AreEqual(0, player.Overload, "Verify overload is now gone");
            Assert.AreEqual(Constants.MAX_MANA_CAPACITY, player.Mana, "Verify mana is back to normal");
        }

        /// <summary>
        /// Verify you can't attack through taunt
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Taunt()
        {
            var alakir = HearthEntityFactory.CreateCard<AlAkirtheWindlord>();
            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var faerie = HearthEntityFactory.CreateCard<FaerieDragon>();

            GameEngine.GameState.CurrentPlayer = player;

            GameEngine.GameState.CurrentPlayerPlayZone[0] = faerie;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = alakir;
            GameEngine.GameState.WaitingPlayerPlayZone[1] = yeti;

            faerie.Attack(yeti);
            faerie.Attack(opponent);
        }

        /// <summary>
        /// Verify charge and windfury mechanics
        /// </summary>
        [Test]
        public void ChargeAndWindfury()
        {
            var alakir = HearthEntityFactory.CreateCard<AlAkirtheWindlord>();
            alakir.CurrentManaCost = 0;

            GameEngine.GameState.CurrentPlayer = player;
            player.AddCardToHand(alakir);

            player.PlayCard(alakir, null);
            alakir.Attack(opponent);

            Assert.AreEqual(30 - alakir.CurrentAttackPower, opponent.Health, "Verify the opponent took damage");

            alakir.Attack(opponent);
            Assert.AreEqual(30 - (alakir.CurrentAttackPower * 2), opponent.Health, "Verify the opponent got hit again");

            try
            {
                alakir.Attack(opponent);
                Assert.Fail("Alakir shouldn't be able to attack a third time!");
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Verify divine shield mechanics
        /// </summary>
        [Test]
        public void DivineShield()
        {
            var alakir = HearthEntityFactory.CreateCard<AlAkirtheWindlord>();
            var playerAlakir = HearthEntityFactory.CreateCard<AlAkirtheWindlord>();

            GameEngine.GameState.CurrentPlayer = player;
            GameEngine.GameState.CurrentPlayerPlayZone[0] = playerAlakir;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = alakir;

            playerAlakir.Attack(alakir);

            Assert.IsFalse(playerAlakir.HasDivineShield, "Verify player's alakir lost divine shield");
            Assert.IsFalse(alakir.HasDivineShield, "Verify opponent's alakir lost divine shield");

            playerAlakir.Attack(alakir);

            Assert.AreEqual(5 - alakir.CurrentAttackPower, playerAlakir.CurrentHealth, "Verify player's alakir took damage");
            Assert.AreEqual(5 - playerAlakir.CurrentAttackPower, alakir.CurrentHealth, "Verify opponent's alakir took damage");
        }

        /// <summary>
        /// Verify can't attack modifier
        /// </summary>
        [Test]
        public void CantAttack()
        {
            GameEngine.GameState.CurrentPlayer = player;

            var ancientWatcher = HearthEntityFactory.CreateCard<AncientWatcher>();
            ancientWatcher.CurrentManaCost = 0;
            ancientWatcher.Owner = player;

            player.AddCardToHand(ancientWatcher);
            player.PlayCard(ancientWatcher, null);

            GameEngine.EndTurn();
            GameEngine.EndTurn();

            try
            {
                ancientWatcher.Attack(opponent);
                Assert.Fail("Ancient watcher shouldn't be able to attack yet!");
            }
            catch (InvalidOperationException)
            {
            }

            ancientWatcher.Silence();
            ancientWatcher.Attack(opponent);

            Assert.AreEqual(30 - ancientWatcher.CurrentAttackPower, opponent.Health, "Verify opponent got hit");
        }

        /// <summary>
        /// Verify stealthed mechanics
        /// </summary>
        [Test]
        public void Stealth()
        {
            GameEngine.GameState.CurrentPlayer = player;

            var bloodImp = HearthEntityFactory.CreateCard<BloodImp>();

            GameEngine.GameState.WaitingPlayerPlayZone[0] = bloodImp;

            var fireball = HearthEntityFactory.CreateCard<Fireball>();
            fireball.CurrentManaCost = 0;

            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            yeti.ApplyStatusEffects(MinionStatusEffects.CHARGE);

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti;

            player.AddCardToHand(fireball);
            
            // Can't target stealthed minion directly with spell
            try
            {
                player.PlayCard(fireball, bloodImp);
                Assert.Fail("Can't target stealthed minion!");
            }
            catch (InvalidOperationException)
            {
            }

            // Can't attack stealthed minions with minion
            try
            {
                yeti.Attack(bloodImp);
                Assert.Fail("Can't target stealthed minion!");
            }
            catch (InvalidOperationException)
            {
            }

            // Can't battlecry target stealthed minions
            var commando = HearthEntityFactory.CreateCard<StormpikeCommando>();
            commando.CurrentManaCost = 0;

            player.AddCardToHand(commando);
            try
            {
                player.PlayCard(commando, bloodImp);
                Assert.Fail("Can't target stealthed minion!");
            }
            catch (InvalidOperationException)
            {
            }

            // Attacking with stealtehd minion breaks stealth
            GameEngine.EndTurn();
            bloodImp.Attack(player);

            Assert.IsFalse(bloodImp.IsStealthed, "Verify blood imp is no longer stealthed");
        }

        /// <summary>
        /// Frozen minions should be unfrozen if they have not attacked on their turn
        /// </summary>
        [Test]
        public void UnfreezeMinions()
        {
            GameEngine.GameState.CurrentPlayer = player;

            var playerYeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var playerGolem = HearthEntityFactory.CreateCard<ArcaneGolem>();
            var playerAlakir = HearthEntityFactory.CreateCard<AlAkirtheWindlord>();
            var opponentYeti = HearthEntityFactory.CreateCard<ChillwindYeti>();

            GameEngine.GameState.CurrentPlayerPlayZone[0] = playerYeti;
            GameEngine.GameState.CurrentPlayerPlayZone[1] = playerGolem;
            GameEngine.GameState.CurrentPlayerPlayZone[2] = playerAlakir;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = opponentYeti;

            playerGolem.Attack(opponent);
            playerAlakir.Attack(opponent);

            playerYeti.ApplyStatusEffects(MinionStatusEffects.FROZEN);
            playerGolem.ApplyStatusEffects(MinionStatusEffects.FROZEN);
            playerAlakir.ApplyStatusEffects(MinionStatusEffects.FROZEN);
            opponentYeti.ApplyStatusEffects(MinionStatusEffects.FROZEN);

            GameEngine.EndTurn();

            // The player's yeti should be unfrozen because it was frozen before it could attack.
            // By definition, freezing means the minion should miss out on an attack so since the
            // yeti didn't attack yet, then it satisfies the requirements to be unfrozen.
            Assert.IsFalse(playerYeti.IsFrozen, "Verify player's yeti is not frozen");

            // This also means that the arcane golem shouldn't be unfrozen because it hasn't missed out on
            // an attack yet since it already attacked this turn
            Assert.IsTrue(playerGolem.IsFrozen, "Verify player's arcane golem is still frozen");

            // Now, if a windfuried minion has already attacked this turn, then it will remain frozen
            Assert.IsTrue(playerAlakir.IsFrozen, "Verify player's alakir is still frozen");

            // On the other hand, the opponent's should be frozen because it hasn't missed out on an attack yet
            Assert.IsTrue(opponentYeti.IsFrozen, "Verify opponent's yeti is still frozen");
            
            GameEngine.EndTurn();

            // Now, the opponent's yeti should be unfrozen since it has missed out on an attack
            Assert.IsFalse(opponentYeti.IsFrozen);
            
            Assert.IsTrue(playerGolem.IsFrozen, "Verify player's arcane golem is still frozen");
            Assert.IsTrue(playerAlakir.IsFrozen, "Verify player's alakir is still frozen");

            GameEngine.EndTurn();

            // Now, the arcane golem and alakir should finally be unfrozen because it has missed out on an attack
            Assert.IsFalse(playerGolem.IsFrozen, "Verify player's arcane golem is finally unfrozen");
            Assert.IsFalse(playerAlakir.IsFrozen, "Verify player's alakir is finally unfrozen");
        }

        /// <summary>
        /// Frozen players should be unfrozen if they have not attacked on their turn
        /// </summary>
        [Test]
        public void UnfreezePlayers()
        {
            GameEngine.GameState.CurrentPlayer = player;

            var fieryWarAxe = HearthEntityFactory.CreateCard<FieryWarAxe>();
            fieryWarAxe.WeaponOwner = player;
            player.Weapon = fieryWarAxe;

            player.ApplyStatusEffects(PlayerStatusEffects.FROZEN);
            opponent.ApplyStatusEffects(PlayerStatusEffects.FROZEN);

            GameEngine.EndTurn();

            // The player should be unfrozen because they have not attacked their turn
            Assert.IsFalse(player.IsFrozen, "Verify player is unfrozen");

            // The opponent on the other hand should still be frozen
            Assert.IsTrue(opponent.IsFrozen, "Verify opponent is frozen");

            GameEngine.EndTurn();

            // Now the opponent should be unfrozen
            Assert.IsFalse(opponent.IsFrozen, "Verify opponent is unfrozen");

            player.Attack(opponent);
            player.ApplyStatusEffects(PlayerStatusEffects.FROZEN);

            GameEngine.EndTurn();

            // This time, the player should remain frozen because they have attacked on their turn
            Assert.IsTrue(player.IsFrozen, "Verify player is frozen");

            GameEngine.EndTurn();
            GameEngine.EndTurn();

            // And now, they should be unfrozen
            Assert.IsFalse(player.IsFrozen, "Verify player is unfrozen");
        }

        /// <summary>
        /// Triggered when the game has ended
        /// </summary>
        /// <param name="result"></param>
        public void OnGameEnded(GameEngine.GameResult result)
        {
            this.gameEnded = true;
            this.gameResult = result;
        }

        /// <summary>
        /// Waits for the game to end within a timeout
        /// </summary>
        /// <param name="intervalInMs">The time to wait between retries in milliseconds</param>
        /// <param name="retries">The number of retries</param>
        private Task WaitUntilGameEnded(int intervalInMs, int retries)
        {
            for (int i = 0; i < retries; i++)
            {
                if (this.gameEnded)
                {
                    break;
                }

                Thread.Sleep(intervalInMs);
            }

            return null;
        }
    }
}
