using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Mindgames spell
    /// 
    /// Put a copy of a random minion from your opponent's deck into the battlefield.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class Mindgames : BaseSpell
    {
        private const int MANA_COST = 4;
        private const int MIN_SPELL_POWER = 0;
        private const int MAX_SPELL_POWER = 0;

        public Mindgames(int id = -1)
        {
            this.Id = id;
            this.Name = "Mindgames";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            throw new NotImplementedException();
        }
    }
}
