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

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().ToBattleUnitModel();
        }

        #endregion

        #region stack

        [Test(Description = "インスタンス生成直後はスタック数1")]
        public void TestStack1()
        {
            var buf = new BattleUnitBuf_SunnyDay();
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);

            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "AddBufメソッドでは付与数0でも付与可能")]
        public void TestStack2()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 0 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);

            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "付与数の上限は5")]
        public void TestStack3()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 6 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);

            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        #endregion

        #region OnRoundStartAfter

        [Test(Description = "幕の開始時、手札からランダムで2枚ほのおタイプが付与される。")]
        public void TestOnRoundStartAfter1()
        {
            owner.allyCardDetail.DrawCards(4);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(4));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasBuf<BattleDiceCardBuf_FireType>()), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnRoundStartAfter();

            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(4));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasBuf<BattleDiceCardBuf_FireType>()), Is.EqualTo(2));
        }

        [Test(Description = "手札が1枚の場合、その手札のみにほのおタイプが付与される。")]
        public void TestOnRoundStartAfter2()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasBuf<BattleDiceCardBuf_FireType>()), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnRoundStartAfter();

            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasBuf<BattleDiceCardBuf_FireType>()), Is.EqualTo(1));
        }

        [Test(Description = "手札が無い場合、ほのおタイプは付与されない。")]
        public void TestOnRoundStartAfter3()
        {
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasBuf<BattleDiceCardBuf_FireType>()), Is.EqualTo(0));

            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnRoundStartAfter();

            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasBuf<BattleDiceCardBuf_FireType>()), Is.EqualTo(0));
        }

        #endregion

        #region BeforeGiveDamage

        [Test(Description = "ほのおタイプが付与されていないバトルページはダメージ量が増加しない。")]
        public void TestBeforeGiveDamage1()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel cardModel = owner.allyCardDetail.GetHand().FirstOrDefault();
            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                card = cardModel,
                owner = owner,
            };
            var diceBehaviour = cardModel.GetBehaviourList().FirstOrDefault();
            var behavior = new BattleDiceBehavior()
            {
                behaviourInCard = diceBehaviour,
                card = cardData,
            };
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);
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
        public void TestBeforeGiveDamage2()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel cardModel = owner.allyCardDetail.GetHand().FirstOrDefault();
            cardModel.AddBuf(new BattleDiceCardBuf_FireType());
            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                card = cardModel,
                owner = owner,
            };
            var diceBehaviour = cardModel.GetBehaviourList().FirstOrDefault();
            var behavior = new BattleDiceBehavior()
            {
                behaviourInCard = diceBehaviour,
                card = cardData,
            };
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 5 };
            owner.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);
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

        #region OnRoundEnd

        [Test(Description = "幕の終了時、数値が1減る。")]
        public void TestOnRoundEnd1()
        {
            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 5 };
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
            var buf = new BattleUnitBuf_SunnyDay(new SystemRandomizer()) { stack = 3 };
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

        #endregion
    }
}
