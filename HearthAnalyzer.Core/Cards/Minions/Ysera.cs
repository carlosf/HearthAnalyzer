using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Cards.Spells;

namespace HearthAnalyzer.Core.Cards.Minions
{
    /// <summary>
    /// Implements the Ysera
    /// 
    /// At the end of your turn, draw a Dream Card.
    /// </summary>
    public class Ysera : BaseMinion, ITriggeredEffectOwner
    {
        internal const int MANA_COST = 9;
        internal const int ATTACK_POWER = 4;
        internal const int HEALTH = 12;

        /// <summary>
        /// List of dream cards Ysera can spawn
        /// </summary>
        internal readonly List<Type> dreamCardTypes = new List<Type>()
        {
            typeof (Dream),
            typeof (EmeraldDrake),
            typeof (LaughingSister),
            typeof (Nightmare),
            typeof (YseraAwakens)
        };

        public Ysera(int id = -1)
        {
            this.Id = id;
            this.Name = "Ysera";
            this.CurrentManaCost = MANA_COST;
            this.OriginalManaCost = MANA_COST;
            this.OriginalAttackPower = ATTACK_POWER;
            this.MaxHealth = HEALTH;
            this.CurrentHealth = HEALTH;
			this.Type = CardType.DRAGON;
        }

        public void RegisterEffect()
        {
            GameEventManager.RegisterForEvent(this, (GameEventManager.TurnEndEventHandler)this.OnTurnEnd);
        }

        private void OnTurnEnd(BasePlayer player)
        {
            var randomDreamCardIndex = GameEngine.Random.Next(this.dreamCardTypes.Count);
            var randomDreamCardType = this.dreamCardTypes[randomDreamCardIndex];

            var createCardMethod = typeof(HearthEntityFactory).GetMethod("CreateCard");
            var createCardTypeMethod = createCardMethod.MakeGenericMethod(new[] { randomDreamCardType });

            dynamic dreamCard = createCardTypeMethod.Invoke(null, null);
            player.AddCardToHand(dreamCard);
        }
    }
}
