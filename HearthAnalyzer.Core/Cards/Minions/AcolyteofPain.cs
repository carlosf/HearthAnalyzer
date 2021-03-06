using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Acolyte of Pain
    /// 
    /// Whenever this minion takes damage, draw a card.
    /// </summary>
    public class AcolyteOfPain : BaseMinion, ITriggeredEffectOwner
    {
        private const int MANA_COST = 3;
        private const int ATTACK_POWER = 1;
        private const int HEALTH = 3;

        public AcolyteOfPain(int id = -1)
        {
            this.Id = id;
            this.Name = "Acolyte of Pain";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.NORMAL_MINION;
        }

        public void RegisterEffect()
        {
            GameEventManager.RegisterForEvent(this, (GameEventManager.DamageDealtEventHandler)OnDamagedEffect);
        }

        internal void OnDamagedEffect(IDamageableEntity target, int damageDealt)
        {
            if (target == this)
            {
                this.Owner.DrawCard();
            }
        }
    }
}
