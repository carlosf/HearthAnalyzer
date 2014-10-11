using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Captain Greenskin
    /// 
    /// <b>Battlecry:</b> Give your weapon +1/+1.
    /// </summary>
    public class CaptainGreenskin : BaseMinion
    {
        private const int MANA_COST = 5;
        private const int ATTACK_POWER = 5;
        private const int HEALTH = 4;
        private const int BATTLECRY_BUFF_VALUE = 1;

        public CaptainGreenskin(int id = -1)
        {
            this.Id = id;
            this.Name = "Captain Greenskin";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.PIRATE;
        }

        public void Battlecry(IDamageableEntity subTarget)
        {
            BaseWeapon weapon = GameEngine.GameState.CurrentPlayer.Weapon;
            if (weapon != null) {
                weapon.TakeBuff(BATTLECRY_BUFF_VALUE, BATTLECRY_BUFF_VALUE);
            }
        }
    }
}
