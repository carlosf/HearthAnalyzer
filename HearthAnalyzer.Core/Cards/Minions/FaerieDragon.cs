using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Faerie Dragon
    /// 
    /// Can't be targeted by spells or Hero Powers.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class FaerieDragon : BaseMinion
    {
        private const int MANA_COST = 2;
        private const int ATTACK_POWER = 3;
        private const int HEALTH = 2;

        public FaerieDragon(int id = -1)
        {
            this.Id = id;
            this.Name = "Faerie Dragon";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.DRAGON;
        }
    }
}
