using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Baine Bloodhoof
    /// </summary>
    public class BaineBloodhoof : BaseMinion
    {
        private const int MANA_COST = 4;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 5;

        public BaineBloodhoof(int id = -1)
        {
            this.Id = id;
            this.Name = "Baine Bloodhoof";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
