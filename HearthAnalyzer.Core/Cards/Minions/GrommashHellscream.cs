using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Grommash Hellscream
    /// 
    /// <b>Charge</b>.  <b>Enrage:</b> +6 Attack
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class GrommashHellscream : BaseMinion
    {
        private const int MANA_COST = 8;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 9;

        public GrommashHellscream(int id = -1)
        {
            this.Id = id;
            this.Name = "Grommash Hellscream";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
