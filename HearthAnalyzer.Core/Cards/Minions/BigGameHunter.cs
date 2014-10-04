using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Big Game Hunter
    /// 
    /// <b>Battlecry:</b> Destroy a minion with an Attack of 7 or more.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class BigGameHunter : BaseMinion
    {
        private const int MANA_COST = 3;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 2;

        public BigGameHunter(int id = -1)
        {
            this.Id = id;
            this.Name = "Big Game Hunter";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
