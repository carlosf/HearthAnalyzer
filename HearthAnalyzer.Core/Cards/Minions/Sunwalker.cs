using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Sunwalker
    /// 
    /// <b>Taunt</b>. <b>Divine Shield</b>
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class Sunwalker : BaseMinion
    {
        private const int MANA_COST = 6;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 5;

        public Sunwalker(int id = -1)
        {
            this.Id = id;
            this.Name = "Sunwalker";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
