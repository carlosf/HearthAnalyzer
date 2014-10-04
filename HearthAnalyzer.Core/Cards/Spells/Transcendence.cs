using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Transcendence spell
    /// 
    /// Until you kill Cho's minions, he can't be attacked.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class Transcendence : BaseSpell
    {
        private const int MANA_COST = 1;
        private const int MIN_SPELL_POWER = 0;
        private const int MAX_SPELL_POWER = 0;

        public Transcendence(int id = -1)
        {
            this.Id = id;
            this.Name = "Transcendence";

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
