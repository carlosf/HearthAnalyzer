using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Boulderfist Ogre
    /// </summary>
    public class BoulderfistOgre : BaseMinion
    {
        private const int MANA_COST = 6;
        private const int ATTACK_POWER = 6;
        private const int HEALTH = 7;

        public BoulderfistOgre(int id = -1)
        {
            this.Id = id;
            this.Name = "Boulderfist Ogre";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
