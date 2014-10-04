using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Leeroy Jenkins
    /// 
    /// <b>Charge</b>. <b>Battlecry:</b> Summon two 1/1 Whelps for your opponent.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class LeeroyJenkins : BaseMinion
    {
        private const int MANA_COST = 4;
        private const int ATTACK_POWER = 6;
        private const int HEALTH = 2;

        public LeeroyJenkins(int id = -1)
        {
            this.Id = id;
            this.Name = "Leeroy Jenkins";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
