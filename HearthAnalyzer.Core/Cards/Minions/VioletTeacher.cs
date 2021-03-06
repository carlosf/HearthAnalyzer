using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Violet Teacher
    /// 
    /// Whenever you cast a spell, summon a 1/1 Violet Apprentice.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class VioletTeacher : BaseMinion
    {
        private const int MANA_COST = 4;
        private const int ATTACK_POWER = 3;
        private const int HEALTH = 5;

        public VioletTeacher(int id = -1)
        {
            this.Id = id;
            this.Name = "Violet Teacher";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }
    }
}
