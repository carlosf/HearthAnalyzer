using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Consecration spell
    /// 
    /// Deal $2 damage to all enemies.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class Consecration : BaseSpell
    {
        private const int MANA_COST = 4;
        private const int MIN_SPELL_POWER = 2;
        private const int MAX_SPELL_POWER = 2;

        public Consecration(int id = -1)
        {
            this.Id = id;
            this.Name = "Consecration";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            throw new NotImplementedException();
        }
    }
}
