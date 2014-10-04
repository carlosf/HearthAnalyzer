using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Questing Adventurer
    /// 
    /// Whenever you play a card, gain +1/+1.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class QuestingAdventurer : BaseMinion
    {
        private const int MANA_COST = 3;
        private const int ATTACK_POWER = 2;
        private const int HEALTH = 2;

        public QuestingAdventurer(int id = -1)
        {
            this.Id = id;
            this.Name = "Questing Adventurer";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
