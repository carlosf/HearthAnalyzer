using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Prophet Velen
    /// 
    /// Double the damage and healing of your spells and Hero Power.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class ProphetVelen : BaseMinion
    {
        private const int MANA_COST = 7;
        private const int ATTACK_POWER = 7;
        private const int HEALTH = 7;

        public ProphetVelen(int id = -1)
        {
            this.Id = id;
            this.Name = "Prophet Velen";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
