using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270022Test
    {
        private BattleUnitModel owner;
        private PassiveAbility_2270022 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270022();
            owner = new BattleUnitModelBuilder() 
            {
                Passives = new[] { passive }
            }.ToBattleUnitModel();
        }

        [Test(Description = "虚弱を受けていない時、何も付与されない。")]
        public void Test1()
        {
            owner.bufListDetail.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "1幕で虚弱を受けるたび、次の幕からクイック1が付与される。")]
        public void Test2()
        {
            for (int round = 1; round <= 7; round++)
            {
                owner.bufListDetail.OnRoundStart();
                Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
                Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

                passive.OnRoundStart();
                Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(round == 1 ? 0 : 1));
                Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness)?.stack ?? 0, Is.EqualTo(round - 1));
                Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
                Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(round == 1 ? 1 : 2));
                Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness)?.stack ?? 0, Is.EqualTo(round - 1));
                Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Weak).stack, Is.EqualTo(1));
                Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

                passive.OnRoundEnd();
                Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(round == 1 ? 1 : 2));
                Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness)?.stack ?? 0, Is.EqualTo(round - 1));
                Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Weak).stack, Is.EqualTo(1));
                Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

                owner.bufListDetail.OnRoundEnd();
            }
        }

        [Test(Description = "1幕で虚弱を複数受けても、次の幕から付与されるクイック数は1。")]
        public void Test3()
        {
            owner.bufListDetail.OnRoundStart();
            passive.OnRoundStart();
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Weak).stack, Is.EqualTo(2));
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            passive.OnRoundEnd();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Weak).stack, Is.EqualTo(2));
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            owner.bufListDetail.OnRoundEnd();
            owner.bufListDetail.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness).stack, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "クイック付与の最大数は6。")]
        public void Test4()
        {
            for (int round = 1; round <= 7; round++)
            {
                owner.bufListDetail.OnRoundStart();
                passive.OnRoundStart();
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
                passive.OnRoundEnd();
                owner.bufListDetail.OnRoundEnd();
            }

            owner.bufListDetail.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(owner.bufListDetail.GetReadyBufList().Count, Is.Zero);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness).stack, Is.EqualTo(6));
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Weak), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Weak), Is.Null);
        }
    }
}
