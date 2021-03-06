using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the The Coin spell
    /// 
    /// Gain 1 Mana Crystal this turn only.
    /// </summary>
    public class TheCoin : BaseSpell
    {
        private const int MANA_COST = 0;
        private const int MIN_SPELL_POWER = 0;
        private const int MAX_SPELL_POWER = 0;

        public TheCoin(int id = -1)
        {
            this.Id = id;
            this.Name = "The Coin";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            this.Owner.Mana = Math.Min(this.Owner.Mana + 1, Constants.MAX_MANA_CAPACITY);
        }
    }
}
