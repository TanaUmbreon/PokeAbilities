using NUnit.Framework;
using PokeAbilities.Test.Helpers.Battles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeAbilities.Test
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Test1()
        {
            BattleEmulator.StartBattleRoutine();

            BattleUnitModel librarian = new BattleUnitModelBuilder()
            {
                Faction = Faction.Player,
            }.Build();
        }

        [Test]
        public void Test2()
        {
            BattleEmulator.StartBattleRoutine(SephirahType.Keter);

            BattleUnitModel librarian = new BattleUnitModelBuilder()
            {
                Faction = Faction.Player,
            }.Build();
        }
    }
}
