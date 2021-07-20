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
            BookXmlInfo equipBook = new BookXmlInfoBuilder()
            {
                Hp = 100,
            }.ToBookXmlInfo();

            owner = new BattleUnitModelBuilder()
            {
                EquipBook = equipBook,
            }.ToBattleUnitModel();
        }

        #region 付与数のテスト

        [Test(Description = "付与数のデフォルト値は1。")]
        public void TestStack_DefaultStack()
        {
            var buf = new BattleUnitBuf_Hail();
            Assert.That(buf.stack, Is.EqualTo(1));

            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "BattleUnitModel.bufListDetail.AddBuf(BattleUnitBuf)では付与数0でも付与可能。")]
        public void TestStack_Stack0()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 0 };
            Assert.That(buf.stack, Is.EqualTo(0));
      
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "付与数の上限は5。")]
        public void TestStack_StackOver()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 6 };
            Assert.That(buf.stack, Is.EqualTo(6));

            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "幕の終了時、数値が1減る。")]
        public void TestStack_DecrementOnRoundEnd()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(buf.IsDestroyed, Is.False);

            buf.OnRoundEnd();
            Assert.That(buf.stack, Is.EqualTo(4));
            Assert.That(buf.IsDestroyed, Is.False);
        }

        [Test(Description = "幕の終了時、数値が0になると破棄される。")]
        public void TestStack_DestroyedOnRoundEnd()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 3 };
            BattleEmulator.AddBuf(owner, buf);
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

        #endregion

        #region スリップダメージのテスト

        [Test(Description = "こおりタイプ持つキャラクターの場合はスリップダメージを受けない。")]
        public void TestOnRoundEndDamage_HasIceType()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            BattleEmulator.AddBuf(owner, buf);
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700600());
            Assert.That(owner.hp, Is.EqualTo(100f));

            buf.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100f));
        }

        [Test(Description = "こおりタイプ以外のタイプを持つキャラクターの場合はスリップダメージを受ける。")]
        public void TestOnRoundEndDamage_HasNotIceType()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            BattleEmulator.AddBuf(owner, buf);
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700500());
            Assert.That(owner.hp, Is.EqualTo(100f));

            buf.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(95f));
        }

        [Test(Description = "パッシブ「ゆきがくれ」を所有している場合はスリップダメージを受けない。")]
        public void TestOnRoundEndDamage_Passive2270016()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            BattleEmulator.AddBuf(owner, buf);
            BattleEmulator.AddPassive(owner, new PassiveAbility_2270016());
            Assert.That(owner.hp, Is.EqualTo(100f));

            buf.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100f));
        }

        [Test(Description = "パッシブ「アイスボディ」を所有している場合はスリップダメージを受けない。")]
        public void TestOnRoundEndDamage_Passive2270017()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 5 };
            BattleEmulator.AddBuf(owner, buf);
            BattleEmulator.AddPassive(owner, new PassiveAbility_2270017());
            Assert.That(owner.hp, Is.EqualTo(100f));

            buf.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100f));
        }

        // ToDo: スリップダメージのテストを実装する。

        #endregion
    }
}
