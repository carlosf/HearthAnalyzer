using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Deathrattles;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Zombie Chow
    /// 
    /// <b>Deathrattle:</b> Restore 5 Health to the enemy hero.
    /// </summary>
    public class ZombieChow : BaseMinion, IDeathrattler
    {
        internal const int MANA_COST = 1;
        internal const int ATTACK_POWER = 2;
        internal const int HEALTH = 3;
        internal const int DEATHRATTLE_POWER = 5;

        public ZombieChow(int id = -1)
        {
            this.Id = id;
            this.Name = "Zombie Chow";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }

        public void RegisterDeathrattle()
        {
            var healTarget = this.Owner == GameEngine.GameState.Player
                ? GameEngine.GameState.Opponent
                : GameEngine.GameState.Player;

            GameEngine.RegisterDeathrattle(this, new DeathrattleHealTarget(this, healTarget, DEATHRATTLE_POWER));
        }
    }
}
