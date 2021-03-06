using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the King Mukla
    /// 
    /// <b>Battlecry:</b> Give your opponent 2 Bananas.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class KingMukla : BaseMinion
    {
        private const int MANA_COST = 3;
        private const int ATTACK_POWER = 5;
        private const int HEALTH = 5;

        public KingMukla(int id = -1)
        {
            this.Id = id;
            this.Name = "King Mukla";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.BEAST;
        }
    }
}
