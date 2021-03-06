using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Kirin Tor Mage
    /// 
    /// <b>Battlecry:</b> The next <b>Secret</b> you play this turn costs (0).
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class KirinTorMage : BaseMinion
    {
        private const int MANA_COST = 3;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 3;

        public KirinTorMage(int id = -1)
        {
            this.Id = id;
            this.Name = "Kirin Tor Mage";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
