using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270021Test
    {
        private BattleUnitModel owner;
        private PassiveAbility_2270021 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270021();
            owner = new BattleUnitModelBuilder() 
            {
                Passives = new[] { passive }
            }.Build();
        }

        [Test(Description = "あられ状態でない時、何も付与されない。")]
        public void TestOnRoundStart1()
        {
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "あられ状態の時、この幕にクイック2が付与される。")]
        public void TestOnRoundStart2()
        {
            owner.bufListDetail.AddBuf(new BattleUnitBuf_Hail() { stack = 5 });
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(2));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness).stack, Is.EqualTo(2));
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);
        }
    }
}
