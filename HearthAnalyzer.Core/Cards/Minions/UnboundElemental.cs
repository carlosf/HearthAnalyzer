using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Unbound Elemental
    /// Basic Minion
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class UnboundElemental : BaseMinion
    {
        private const int MANA_COST = 0;
        private const int ATTACK_POWER = 2;
        private const int HEALTH = 4;

        public UnboundElemental(int id = -1)
        {
            this.Id = id;
            this.Name = "Unbound Elemental";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.CurrentAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
        }
    }
}
