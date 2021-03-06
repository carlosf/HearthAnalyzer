using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Dust Devil
    /// 
    /// <b>Windfury</b>. <b>Overload:</b> (2)
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class DustDevil : BaseMinion
    {
        private const int MANA_COST = 1;
        private const int ATTACK_POWER = 3;
        private const int HEALTH = 1;

        public DustDevil(int id = -1)
        {
            this.Id = id;
            this.Name = "Dust Devil";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
