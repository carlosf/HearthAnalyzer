using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Reckless Rocketeer
    /// Basic Minion
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class RecklessRocketeer : BaseMinion
    {
        private const int MANA_COST = 6;
        private const int ATTACK_POWER = 5;
        private const int HEALTH = 2;

        public RecklessRocketeer(int id = -1)
        {
            this.Id = id;
            this.Name = "Reckless Rocketeer";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.CurrentAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
        }
    }
}
