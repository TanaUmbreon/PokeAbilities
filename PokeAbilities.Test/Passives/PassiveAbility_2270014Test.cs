using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270014Test
    {
        private BattleUnitModel owner;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().ToBattleUnitModel();
        }

        [Test(Description = "にほんばれ状態でない時、バフを付与可能。")]
        public void TestCanAddBuf1()
        {
            var passive = new PassiveAbility_2270014();
            passive.Init(owner);

            var posiBaf = new BattleUnitBuf_strength() { stack = 1 };
            Assert.That(passive.CanAddBuf(posiBaf), Is.True);

            var negaBaf = new BattleUnitBuf_paralysis() { stack = 1 };
            Assert.That(passive.CanAddBuf(negaBaf), Is.True);

            var noneBaf = new BattleUnitBuf_smoke() { stack = 1 };
            Assert.That(passive.CanAddBuf(noneBaf), Is.True);
        }

        [Test(Description = "にほんばれ状態の時、状態異常は付与不可、それ以外のバフは付与可能。")]
        public void TestCanAddBuf2()
        {
            var passive = new PassiveAbility_2270014();
            passive.Init(owner);
            owner.bufListDetail.AddBuf(new BattleUnitBuf_SunnyDay() { stack = 5 });

            var posiBaf = new BattleUnitBuf_strength() { stack = 1 };
            Assert.That(passive.CanAddBuf(posiBaf), Is.True);

            var negaBaf = new BattleUnitBuf_paralysis() { stack = 1 };
            Assert.That(passive.CanAddBuf(negaBaf), Is.False);

            var noneBaf = new BattleUnitBuf_smoke() { stack = 1 };
            Assert.That(passive.CanAddBuf(noneBaf), Is.True);
        }
    }
}
