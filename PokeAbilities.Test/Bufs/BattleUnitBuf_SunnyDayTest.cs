using System.Linq;
using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Bufs
{
    [TestFixture]
    public class BattleUnitBuf_SunnyDayTest
    {
        private BattleUnitModel owner;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().Build();
        }

        #region 付与数のテスト

        [Test(Description = "付与数のデフォルト値は1。")]
        public void TestStack_DefaultStack()
        {
            var buf = new BattleUnitBuf_SunnyDay();
            Assert.That(buf.stack, Is.EqualTo(1));

            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "BattleUnitModel.bufListDetail.AddBuf(BattleUnitBuf)では付与数0でも付与可能。")]
        public void TestStack_Stack0()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 0 };
            Assert.That(buf.stack, Is.EqualTo(0));

            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "付与数の上限は5。")]
        public void TestStack_StackOver()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 6 };
            Assert.That(buf.stack, Is.EqualTo(6));

            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "幕の終了時、数値が1減る。")]
        public void TestStack_DecrementOnRoundEnd()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 5 };
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
            var buf = new BattleUnitBuf_SunnyDay() { stack = 3 };
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

        #region バトルページへのタイプ付与のテスト

        [Test(Description = "幕の開始時、手札が0枚の場合はほのおタイプが付与されない。")]
        public void TestOnRoundStartAfter_Hand0()
        {
            owner.allyCardDetail.DrawCards(0);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が1枚の場合はその手札にほのおタイプ付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand1()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(1));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が2枚の場合はその手札全てにほのおタイプが付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand2()
        {
            owner.allyCardDetail.DrawCards(2);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(2));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が3枚の場合は手札2枚にほのおタイプが付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand3()
        {
            owner.allyCardDetail.DrawCards(3);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(2));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が3枚、タイプ付与された手札が1枚の場合は、タイプ付与されていない手札3枚の中から2枚にほのおタイプが付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand3AndTypeHand1()
        {
            owner.allyCardDetail.DrawCards(4);
            owner.allyCardDetail.GetHand().FirstOrDefault()?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Poison));
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(4));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Poison), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(4));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Poison), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(2));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与された手札が1枚の場合ははほのおタイプが付与されない。")]
        public void TestOnRoundStartAfter_TypeHand1()
        {
            owner.allyCardDetail.DrawCards(1);
            owner.allyCardDetail.GetHand().FirstOrDefault()?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Poison));
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Poison), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Poison), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        #endregion

        #region 被ダメージ量増減のテスト

        [Test(Description = "攻撃キャラクターがいない状態でダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_AttackerIsNull()
        {
            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(null, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "タイプ付与されてないバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_NoTypePageAndAttackDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
                currentBehaviourDetail: BehaviourDetail.Slash,
                target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "タイプ付与されてないバトルページの守備ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_NoTypePageAndDefenceDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
               currentBehaviourDetail: BehaviourDetail.Guard,
               target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "ほのおタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は+1 (被ダメージ軽減量-1)")]
        public void TestGetDamageReductionAll_FireTypePageAndAttackDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
                currentBehaviourDetail: BehaviourDetail.Slash,
                target: owner);
            attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Fire));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(-1));
        }

        [Test(Description = "ほのおタイプが付与されたバトルページの防御ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_FireTypePageAndDefenceDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
               currentBehaviourDetail: BehaviourDetail.Guard,
               target: owner);
            attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Fire));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "みずタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は-1 (被ダメージ軽減量+1)")]
        public void TestGetDamageReductionAll_WaterTypePageAndAttackDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
                currentBehaviourDetail: BehaviourDetail.Slash,
                target: owner);
            attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Water));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(1));
        }

        [Test(Description = "みずタイプが付与されたバトルページの防御ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_WaterTypePageAndDefenceDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
                currentBehaviourDetail: BehaviourDetail.Guard,
                target: owner);
            attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Water));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "ほのおタイプ、みずタイプ以外のタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_AnotherTypePageAndAttackDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
                currentBehaviourDetail: BehaviourDetail.Slash,
                target: owner);
            attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Grass));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "ほのおタイプ、みずタイプ以外のタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestGetDamageReductionAll_AnotherTypePageAndDefenceDice()
        {
            BattleUnitModel attaker = BattleEmulator.CreateAttacker(
                currentBehaviourDetail: BehaviourDetail.Guard,
                target: owner);
            attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Grass));

            var buf = new BattleUnitBuf_SunnyDay();
            BattleEmulator.AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        #endregion
    }
}
