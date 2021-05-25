using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Bufs
{
    [TestFixture]
    public class BattleUnitBuf_HailTest
    {
        private BattleUnitModel owner;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder()
            {
                Hp = 100,
            }.ToBattleUnitModel();
        }

        #region stack

        [Test(Description = "インスタンス生成直後はスタック数1")]
        public void TestStack1()
        {
            var buf = new BattleUnitBuf_Hail();
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);

            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "AddBufメソッドでは付与数0でも付与可能")]
        public void TestStack2()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 0 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);

            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "付与数の上限は5")]
        public void TestStack3()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 6 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);

            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        #endregion

        #region OnRoundEnd

        // Note: OnRoundEndでスリップダメージを受けるパターンはテスト不可能。
        //   内部でUnitBattleDataModelを使用し、UnityEngineの参照を回避できないため。

        [Test(Description = "幕の終了時、数値が1減る。")]
        public void TestOnRoundEnd1()
        {
            // スリップダメージの回避
            owner.passiveDetail.AddPassive(new PassiveAbility_2270016());
            owner.passiveDetail.OnCreated();

            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(buf.IsDestroyed, Is.False);

            buf.OnRoundEnd();
            Assert.That(buf.stack, Is.EqualTo(4));
            Assert.That(buf.IsDestroyed, Is.False);
        }

        [Test(Description = "数値が0になると破棄される。")]
        public void TestOnRoundEnd2()
        {
            // スリップダメージの回避
            owner.passiveDetail.AddPassive(new PassiveAbility_2270016());
            owner.passiveDetail.OnCreated();

            var buf = new BattleUnitBuf_Hail() { stack = 3 };
            owner.bufListDetail.AddBuf(buf);
            Assert.That(buf.stack, Is.EqualTo(3));
            Assert.That(buf.IsDestroyed, Is.False);

            buf.OnRoundEnd();
            buf.OnRoundEnd();
            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(buf.IsDestroyed, Is.False);

            buf.OnRoundEnd();
            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(buf.IsDestroyed, Is.True);
        }

        [Test(Description = "パッシブ「ゆきがくれ」を所有している場合はスリップダメージを受けない。")]
        public void TestOnRoundEnd3()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_2270016());
            owner.passiveDetail.OnCreated();

            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            Assert.That(owner.hp, Is.EqualTo(100f));

            buf.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100f));
        }

        [Test(Description = "パッシブ「アイスボディ」を所有している場合はスリップダメージを受けない。")]
        public void TestOnRoundEnd4()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_2270017());
            owner.passiveDetail.OnCreated();

            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            Assert.That(owner.hp, Is.EqualTo(100f));

            buf.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100f));
        }

        [Test(Description = "スリップダメージのテストはできない。")]
        public void TestOnRoundEndXXX()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            Assert.That(owner.hp, Is.EqualTo(100f));

            Assert.That(() => { buf.OnRoundEnd(); }, Throws.Exception);
            Assert.That(owner.hp, Is.EqualTo(100f));
        }

        #endregion
    }
}
