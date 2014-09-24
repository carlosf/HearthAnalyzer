using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Nozdormu
    /// Basic Minion
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class Nozdormu : BaseMinion
    {
        private const int MANA_COST = 0;
        private const int ATTACK_POWER = 8;
        private const int HEALTH = 8;

        public Nozdormu(int id = -1)
        {
            this.Id = id;
            this.Name = "Nozdormu";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.CurrentAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
        }
    }
}