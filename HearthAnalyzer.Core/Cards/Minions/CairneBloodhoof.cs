using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Deathrattles;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Cairne Bloodhoof
    /// 
    /// <b>Deathrattle:</b> Summon a 4/5 Baine Bloodhoof.
    /// </summary>
    public class CairneBloodhoof : BaseMinion, IDeathrattler
    {
        private const int MANA_COST = 6;
        private const int ATTACK_POWER = 4;
        private const int HEALTH = 5;

        public CairneBloodhoof(int id = -1)
        {
            this.Id = id;
            this.Name = "Cairne Bloodhoof";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }

        public void RegisterDeathrattle()
        {
            GameEngine.RegisterDeathrattle(this, new DeathrattleSummonMinion<BaineBloodhoof>(this.Owner));
        }
    }
}
