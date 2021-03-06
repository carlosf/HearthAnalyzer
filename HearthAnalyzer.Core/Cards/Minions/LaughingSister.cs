using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Laughing Sister
    /// 
    /// Can't be targeted by spells or Hero Powers.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class LaughingSister : BaseMinion
    {
        private const int MANA_COST = 3;
        private const int ATTACK_POWER = 3;
        private const int HEALTH = 5;

        public LaughingSister(int id = -1)
        {
            this.Id = id;
            this.Name = "Laughing Sister";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
