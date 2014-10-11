using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Cards;

namespace HearthAnalyzer.Core.Deathrattles
{
    /// <summary>
    /// Represents a deathrattle that summons minions
    /// </summary>
    public class DeathrattleSummonMinion<T> : BaseDeathrattle where T : BaseMinion
    {
        private readonly BasePlayer owner;
        private readonly ICollection<BaseCard> cardSource;

        public DeathrattleSummonMinion(BasePlayer owner, ICollection<BaseCard> cardSource = null)
        {
            this.owner = owner;
            this.cardSource = cardSource;
        }

        public override void Deathrattle()
        {
            BaseMinion minionToSummon;
            if (this.cardSource != null)
            {
                var minionFromSource = this.cardSource.FirstOrDefault(card => card is T);
                if (minionFromSource == null)
                {
                    throw new InvalidOperationException(
                        string.Format("Could not find an instance of {0} from the card source!", typeof (T)));
                }

                minionToSummon = (BaseMinion) minionFromSource;
            }
            else
            {
                minionToSummon = HearthEntityFactory.CreateCard<T>();
            }

            var rightMostIndex = GameEngine.GameState.CurrentPlayerPlayZone.Count(card => card != null);
            this.owner.SummonMinion(minionToSummon, null, rightMostIndex, CardEffect.NONE, forceSummoned: true, cardSource: this.cardSource);
        }
    }
}
