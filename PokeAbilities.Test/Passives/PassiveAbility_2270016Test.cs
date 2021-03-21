using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270016Test
    {
        private BattleUnitModel owner;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().ToBattleUnitModel();
        }
    }
}
