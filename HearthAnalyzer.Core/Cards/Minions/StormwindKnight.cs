using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Stormwind Knight
    /// 
    /// <b>Charge</b>
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class StormwindKnight : BaseMinion
    {
        private const int MANA_COST = 4;
        private const int ATTACK_POWER = 2;
        private const int HEALTH = 5;

        public StormwindKnight(int id = -1)
        {
            this.Id = id;
            this.Name = "Stormwind Knight";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
