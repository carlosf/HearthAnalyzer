using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Holy Wrath spell
    /// 
    /// Draw a card and deal damage equal to its cost.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class HolyWrath : BaseSpell
    {
        private const int MANA_COST = 5;
        private const int MIN_SPELL_POWER = 0;
        private const int MAX_SPELL_POWER = 0;

        public HolyWrath(int id = -1)
        {
            this.Id = id;
            this.Name = "Holy Wrath";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;

			this.BonusSpellPower = 0;
        }

        public override void Activate(IDamageableEntity target = null)
        {
            throw new NotImplementedException();
        }
    }
}
