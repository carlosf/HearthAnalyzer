using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Shan'do's Lesson spell
    /// 
    /// Summon two 2/2 Treants with <b>Taunt</b>.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class ShandosLesson : BaseSpell
    {
        private const int MANA_COST = 0;
        private const int MIN_SPELL_POWER = 0;
        private const int MAX_SPELL_POWER = 0;

        public ShandosLesson(int id = -1)
        {
            this.Id = id;
            this.Name = "Shan'do's Lesson";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            throw new NotImplementedException();
        }
    }
}
