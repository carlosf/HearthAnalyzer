using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Gurubashi Berserker
    /// 
    /// Whenever this minion takes damage, gain +3 Attack.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class GurubashiBerserker : BaseMinion
    {
        private const int MANA_COST = 5;
        private const int ATTACK_POWER = 2;
        private const int HEALTH = 7;

        public GurubashiBerserker(int id = -1)
        {
            this.Id = id;
            this.Name = "Gurubashi Berserker";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
