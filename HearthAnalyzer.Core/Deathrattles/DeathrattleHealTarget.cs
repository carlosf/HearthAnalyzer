using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Cards;

namespace HearthAnalyzer.Core.Deathrattles
{
    /// <summary>
    /// Represents a deathrattle the heals a target
    /// </summary>
    public class DeathrattleHealTarget : BaseDeathrattle
    {
        private BaseCard owner;
        private IDamageableEntity healTarget;
        private int healAmount;

        /// <summary>
        /// Constructs an instance of DeathrattleHealTarget
        /// </summary>
        /// <param name="owner">The owner of the deathrattle</param>
        /// <param name="healTarget">The target to heal</param>
        /// <param name="healAmount">The amount to heal</param>
        public DeathrattleHealTarget(BaseCard owner, IDamageableEntity healTarget, int healAmount)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            if (healTarget == null) throw new ArgumentNullException("healTarget");

            this.owner = owner;
            this.healTarget = healTarget;
            this.healAmount = healAmount;
        }

        public override void Deathrattle()
        {
            // TODO: Consider putting this in a global helper function
            bool shouldAbort;
            GameEventManager.Healing(this.owner.Owner, this.healTarget, this.healAmount, out shouldAbort);

            if (!shouldAbort)
            {
                this.healTarget.TakeHealing(this.healAmount);
            }
        }
    }
}
