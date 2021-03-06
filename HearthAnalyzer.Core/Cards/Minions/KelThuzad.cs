using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Kel'Thuzad
    /// 
    /// At the end of each turn, summon all friendly minions that died this turn.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class KelThuzad : BaseMinion
    {
        private const int MANA_COST = 8;
        private const int ATTACK_POWER = 6;
        private const int HEALTH = 8;

        public KelThuzad(int id = -1)
        {
            this.Id = id;
            this.Name = "Kel'Thuzad";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
