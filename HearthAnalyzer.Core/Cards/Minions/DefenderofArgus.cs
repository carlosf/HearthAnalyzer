using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Defender of Argus
    /// 
    /// <b>Battlecry:</b> Give adjacent minions +1/+1 and <b>Taunt</b>.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class DefenderofArgus : BaseMinion
    {
        private const int MANA_COST = 4;
        private const int ATTACK_POWER = 2;
        private const int HEALTH = 3;

        public DefenderofArgus(int id = -1)
        {
            this.Id = id;
            this.Name = "Defender of Argus";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
