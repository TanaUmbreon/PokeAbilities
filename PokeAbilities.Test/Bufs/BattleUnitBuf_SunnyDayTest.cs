using System.Collections.Generic;
using System.Linq;
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

        #region TestStack

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

        #region TestOnRoundStartAfter

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

        #region TestBeforeGiveDamage

        [Test(Description = "ほのおタイプが付与されていないバトルページはダメージ量が増加しない。")]
        public void TestBeforeGiveDamage_NotAddedFireType()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = owner.allyCardDetail.GetHand().FirstOrDefault();
            BattleDiceBehavior behavior = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = owner,
                Target = null,
                CurrentBehavior = behavior,
                Card = card,
            }.ToBattlePlayingCardDataInUnitModel();
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "ほのおタイプが付与されているバトルページはダメージ量+1。")]
        public void TestBeforeGiveDamage_AddedFireType()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = owner.allyCardDetail.GetHand().FirstOrDefault();
            card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Fire));
            BattleDiceBehavior behavior = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = owner,
                Target = null,
                CurrentBehavior = behavior,
                Card = card,
            }.ToBattlePlayingCardDataInUnitModel();
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            var buf = new BattleUnitBuf_SunnyDay();
            AddBuf(owner, buf);
            buf.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(1));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
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
