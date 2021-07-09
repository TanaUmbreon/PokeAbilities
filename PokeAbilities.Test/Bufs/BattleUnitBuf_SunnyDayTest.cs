using System.Collections.Generic;
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
            owner = new BattleUnitModelBuilder().ToBattleUnitModel();
        }

        /// <summary>
        /// 指定したキャラクターに指定したバフを付与します。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="buf"></param>
        private static void AddBuf(BattleUnitModel target, BattleUnitBuf buf)
        {
            target.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);
        }

        /// <summary>
        /// 指定したキャラクターの手札から、何らかのタイプが付与されているバトル ページのみを抽出したコレクションを取得します。
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<BattleDiceCardModel> GetTypeAddedHand(BattleUnitModel target)
            => target.allyCardDetail.GetHand().Where(h => h.GetBufList().OfType<BattleDiceCardBuf_Type>().Any());

        /// <summary>
        /// 指定したキャラクターの手札から、指定したタイプが付与されているバトル ページのみを抽出したコレクションを取得します。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<BattleDiceCardModel> GetTypeAddedHand(BattleUnitModel target, PokeType type)
            => target.allyCardDetail.GetHand().Where(h => h.GetBufList().OfType<BattleDiceCardBuf_Type>().Any(b => b.Type == type));

        /// <summary>
        /// 指定したキャラクターの手札から、何らかのタイプが 2 個以上同時に付与されているバトル ページのみを抽出したコレクションを取得します。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static IEnumerable<BattleDiceCardModel> GetMultiTypesAddedHand(BattleUnitModel target)
            => target.allyCardDetail.GetHand().Where(h => h.GetBufList().OfType<BattleDiceCardBuf_Type>().Count() >= 2);

        /// <summary>
        /// 指定したキャラクターに対してバトル ダイスを使用する攻撃キャラクターを生成します。
        /// </summary>
        /// <param name="detail">使用するバトル ダイスの種類。</param>
        /// <param name="addedType">使用するバトル ページに付与させるタイプ。 null の場合は付与しません。</param>
        /// <param name="target">攻撃対象のキャラクター。</param>
        /// <returns></returns>
        private static BattleUnitModel CreateUsingBehaviorAttacker(BehaviourDetail detail, PokeType? addedType, BattleUnitModel target)
        {
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.ToBattleUnitModel();

            attaker.currentDiceAction = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Target = target,
                Card = new BattleDiceCardModelBuilder()
                {
                    Owner = attaker,
                }.ToBattleDiceCardModel(),
                CurrentBehavior = new BattleDiceBehaviorBuilder()
                {
                    Detail = detail,
                    DiceVanillaValue = 10,
                }.ToBattleDiceBehavior(),
            }.ToBattlePlayingCardDataInUnitModel();

            if (addedType != null)
            {
                attaker.currentDiceAction.card.AddBuf(new BattleDiceCardBuf_Type(addedType.Value));
            }

            return attaker;
        }

        #region 付与数のテスト

        [Test(Description = "付与数のデフォルト値は1。")]
        public void TestStack_DefaultStack()
        {
            var buf = new BattleUnitBuf_SunnyDay();
            Assert.That(buf.stack, Is.EqualTo(1));

            AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "AddBufメソッドでは付与数0でも付与可能。")]
        public void TestStack_Stack0()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 0 };
            Assert.That(buf.stack, Is.EqualTo(0));

            AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "付与数の上限は5。")]
        public void TestStack_StackOver()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 6 };
            Assert.That(buf.stack, Is.EqualTo(6));

            AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        #endregion

        #region バトルページへのタイプ付与のテスト

        [Test(Description = "幕の開始時、手札が0枚の場合はほのおタイプが付与されない。")]
        public void TestOnRoundStartAfter_Hand0()
        {
            owner.allyCardDetail.DrawCards(0);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が1枚の場合はその手札にほのおタイプ付与される。")]
        public void TestOnRoundStartAfter_Hand1()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(1));
            Assert.That(GetTypeAddedHand(owner, PokeType.Fire).Count, Is.EqualTo(1));
            Assert.That(GetMultiTypesAddedHand(owner).Count, Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が2枚の場合はその手札全てにほのおタイプが付与される。")]
        public void TestOnRoundStartAfter_Hand2()
        {
            owner.allyCardDetail.DrawCards(2);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(2));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(2));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(2));
            Assert.That(GetTypeAddedHand(owner, PokeType.Fire).Count, Is.EqualTo(2));
            Assert.That(GetMultiTypesAddedHand(owner).Count, Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が3枚の場合は手札2枚にほのおタイプが付与される。")]
        public void TestOnRoundStartAfter_Hand3()
        {
            owner.allyCardDetail.DrawCards(3);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(3));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(3));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(2));
            Assert.That(GetTypeAddedHand(owner, PokeType.Fire).Count, Is.EqualTo(2));
            Assert.That(GetMultiTypesAddedHand(owner).Count, Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が4枚で内1枚が既にタイプ付与されている場合は、タイプ付与されていない残り手札3枚の中から2枚にほのおタイプが付与される。")]
        public void TestOnRoundStartAfter_Hand4AlreadyTypeAdded1()
        {
            owner.allyCardDetail.DrawCards(4);
            owner.allyCardDetail.GetHand().FirstOrDefault()?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Poison));
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(4));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(1));
            Assert.That(GetTypeAddedHand(owner, PokeType.Poison).Count, Is.EqualTo(1));
            Assert.That(GetTypeAddedHand(owner, PokeType.Fire).Count, Is.EqualTo(0));
            Assert.That(GetMultiTypesAddedHand(owner).Count, Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(4));
            Assert.That(GetTypeAddedHand(owner).Count, Is.EqualTo(3));
            Assert.That(GetTypeAddedHand(owner, PokeType.Poison).Count, Is.EqualTo(1));
            Assert.That(GetTypeAddedHand(owner, PokeType.Fire).Count, Is.EqualTo(2));
            Assert.That(GetMultiTypesAddedHand(owner).Count, Is.EqualTo(0));
        }

        #endregion

        #region 被ダメージ量増減のテスト

        [Test(Description = "攻撃キャラクターがいない状態でダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestBeforeTakeDamage_AttackerIsNull()
        {
            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(null, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "タイプ付与されてないバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestBeforeTakeDamage_NoTypePageAndAttackDice()
        {
            BattleUnitModel attaker = CreateUsingBehaviorAttacker(
                detail: BehaviourDetail.Slash, addedType: null, target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "タイプ付与されてないバトルページの守備ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestBeforeTakeDamage_NoTypePageAndDefenceDice()
        {
            BattleUnitModel attaker = CreateUsingBehaviorAttacker(
                detail: BehaviourDetail.Guard, addedType: null, target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "ほのおタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は+1 (被ダメージ軽減量-1)")]
        public void TestBeforeTakeDamage_FireTypePageAndAttackDice()
        {
            BattleUnitModel attaker = CreateUsingBehaviorAttacker(
                detail: BehaviourDetail.Slash, addedType: PokeType.Fire, target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(-1));
        }

        [Test(Description = "ほのおタイプが付与されたバトルページの防御ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestBeforeTakeDamage_FireTypePageAndDefenceDice()
        {
            BattleUnitModel attaker = CreateUsingBehaviorAttacker(
                detail: BehaviourDetail.Evasion, addedType: PokeType.Fire, target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        [Test(Description = "みずタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は-1 (被ダメージ軽減量+1)")]
        public void TestBeforeTakeDamage_WaterTypePageAndAttackDice()
        {
            BattleUnitModel attaker = CreateUsingBehaviorAttacker(
                detail: BehaviourDetail.Penetrate, addedType: PokeType.Water, target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));

            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(1));
        }

        [Test(Description = "ほのおタイプ、みずタイプ以外のタイプが付与されたバトルページの攻撃ダイスでダメージを受けた時、被ダメージ量は増減しない。")]
        public void TestBeforeTakeDamage_AnotherTypePageAndAttackDice()
        {
            BattleUnitModel attaker = CreateUsingBehaviorAttacker(
                detail: BehaviourDetail.Slash, addedType: PokeType.Grass, target: owner);

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
   
            buf.BeforeTakeDamage(attaker, 10);
            Assert.That(buf.GetDamageReductionAll(), Is.EqualTo(0));
        }

        #endregion

        #region TestOnRoundEnd

        [Test(Description = "幕の終了時、数値が1減る。")]
        public void TestOnRoundEnd1()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 5 };
            AddBuf(owner, buf);
            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(buf.IsDestroyed, Is.False);

            buf.OnRoundEnd();
            Assert.That(buf.stack, Is.EqualTo(4));
            Assert.That(buf.IsDestroyed, Is.False);
        }

        [Test(Description = "数値が0になると破棄される。")]
        public void TestOnRoundEnd2()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 3 };
            AddBuf(owner, buf);
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
    }
}
