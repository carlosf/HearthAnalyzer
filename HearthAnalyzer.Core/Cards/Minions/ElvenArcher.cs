using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Elven Archer
    /// 
    /// <b>Battlecry:</b> Deal 1 damage.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class ElvenArcher : BaseMinion
    {
        private const int MANA_COST = 1;
        private const int ATTACK_POWER = 1;
        private const int HEALTH = 1;

        public ElvenArcher(int id = -1)
        {
            this.Id = id;
            this.Name = "Elven Archer";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
