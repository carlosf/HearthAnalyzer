using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Captain's Parrot
    /// 
    /// <b>Battlecry:</b> Put a random Pirate from your deck into your hand.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class CaptainsParrot : BaseMinion
    {
        private const int MANA_COST = 2;
        private const int ATTACK_POWER = 1;
        private const int HEALTH = 1;

        public CaptainsParrot(int id = -1)
        {
            this.Id = id;
            this.Name = "Captain's Parrot";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.BEAST;
        }
    }
}
