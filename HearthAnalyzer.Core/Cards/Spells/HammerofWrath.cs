using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Hammer of Wrath spell
    /// 
    /// Deal $3 damage.  Draw a card.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class HammerofWrath : BaseSpell
    {
        private const int MANA_COST = 4;
        private const int MIN_SPELL_POWER = 3;
        private const int MAX_SPELL_POWER = 3;

        public HammerofWrath(int id = -1)
        {
            this.Id = id;
            this.Name = "Hammer of Wrath";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            throw new NotImplementedException();
        }
    }
}
