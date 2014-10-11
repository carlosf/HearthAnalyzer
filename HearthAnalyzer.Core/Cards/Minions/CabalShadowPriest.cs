using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Cabal Shadow Priest
    /// 
    /// <b>Battlecry:</b> Take control of an enemy minion that has 2 or less Attack.
    /// </summary>
    public class CabalShadowPriest : BaseMinion, IBattlecry
    {
        private const int MANA_COST = 6;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 5;
        private const int BATTLECRY_POWER = 2;

        public CabalShadowPriest(int id = -1)
        {
            this.Id = id;
            this.Name = "Cabal Shadow Priest";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }

        public void Battlecry(IDamageableEntity subTarget)
        {
            // Check to see if the enemy board has a minion with 2 or less attack
            if (GameEngine.GameState.WaitingPlayerPlayZone.Any(
                card => card != null && card.CurrentAttackPower <= BATTLECRY_POWER)
                && subTarget == null)
            {
                throw new InvalidOperationException("There is a valid target on the board so a target must be supplied!");
            }

            // If there are no valid targets but a target was supplied
            if (!GameEngine.GameState.WaitingPlayerPlayZone.Any(
                card => card != null && card.CurrentAttackPower <= BATTLECRY_POWER)
                && subTarget != null)
            {
                throw new InvalidOperationException("There are no valid targets on the board!");
            }

            if (subTarget != null)
            {
                if (!(subTarget is BaseMinion))
                {
                    throw new InvalidOperationException("Only minions can be targeted");
                }

                var targetMinion = (BaseMinion) subTarget;
                if (targetMinion.CurrentAttackPower > BATTLECRY_POWER)
                {
                    throw new InvalidOperationException(string.Format("{0}'s attack is too high!", targetMinion));
                }

                var currentPlayZone = GameEngine.GameState.CurrentPlayerPlayZone;
                var playZoneCount = currentPlayZone.Count(card => card != null);
                if (playZoneCount == Constants.MAX_CARDS_ON_BOARD)
                {
                    throw new InvalidOperationException("There is not enough room on the board!");
                }

                GameEngine.GameState.Board.RemoveCard(targetMinion);
                currentPlayZone[playZoneCount] = targetMinion;
                targetMinion.Owner = this.Owner;
            }
        }
    }
}
