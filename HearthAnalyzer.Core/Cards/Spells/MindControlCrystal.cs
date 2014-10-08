using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Mind Control Crystal spell
    /// 
    /// Activate the Crystal to control the Understudies!
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class MindControlCrystal : BaseSpell
    {
        private const int MANA_COST = 1;
        private const int MIN_SPELL_POWER = 0;
        private const int MAX_SPELL_POWER = 0;

        public MindControlCrystal(int id = -1)
        {
            this.Id = id;
            this.Name = "Mind Control Crystal";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            throw new NotImplementedException();
        }
    }
}
