using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards.Spells
{
    /// <summary>
    /// Implements the Shadow Bolt spell
    /// 
    /// Deal $4 damage to a minion.
    /// </summary>
    /// <remarks>
    /// TODO: NOT YET COMPLETELY IMPLEMENTED
    /// </remarks>
    public class ShadowBolt : BaseSpell
    {
        private const int MANA_COST = 3;
        private const int MIN_SPELL_POWER = 4;
        private const int MAX_SPELL_POWER = 4;

        public ShadowBolt(int id = -1)
        {
            this.Id = id;
            this.Name = "Shadow Bolt";

            this.OriginalManaCost = MANA_COST;
            this.CurrentManaCost = MANA_COST;
        }

        public override void Activate(IDamageableEntity target = null, CardEffect cardEffect = CardEffect.NONE)
        {
            throw new NotImplementedException();
        }
    }
}
