using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Wrath of Air Totem
    /// Basic Minion
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class WrathofAirTotem : BaseMinion
    {
        private const int MANA_COST = 0;
        private const int ATTACK_POWER = 0;
        private const int HEALTH = 2;

        public WrathofAirTotem(int id = -1)
        {
            this.Id = id;
            this.Name = "Wrath of Air Totem";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.CurrentAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
        }
    }
}
