using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270023Test
    {
        private BattleUnitModel owner;
        private PassiveAbility_2270023 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270023();
            owner = new BattleUnitModelBuilder() 
            {
                Passives = new[] { passive }
            }.ToBattleUnitModel();
        }

        [Test(Description = "あめ状態でない時、何も付与されない。")]
        public void TestOnRoundStart1()
        {
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);
        }

        [Test(Description = "あめ状態の時、この幕にクイック2が付与される。")]
        public void TestOnRoundStart2()
        {
            owner.bufListDetail.AddBuf(new BattleUnitBuf_Rain() { stack = 5 });
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Not.Null);
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness).stack, Is.EqualTo(2));
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);
        }
    }
}
