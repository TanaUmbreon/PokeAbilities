using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    #region 単独タイプのテスト

    [TestFixture(Description = "単独タイプのテスト。")]
    public class PassiveAbilityTypeBaseTest_SingleType
    {
        /// <summary>このテストで使用する1番目のタイプ</summary>
        private const PokeType Type1 = PokeType.Dark;

        private class PassiveAbility_SingleType : PassiveAbilityTypeBase
        {
            public override IEnumerable<PokeType> Types => new[] { Type1 };
        }

        private BattleUnitModel owner;
        private PassiveAbilityTypeBase passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_SingleType();
            owner = new BattleUnitModelBuilder()
            {
                Passives = new[] { passive },
            }.ToBattleUnitModel();
        }

        [Test]
        public void TestTypes()
        {
            Assert.That(passive.Types, Is.EqualTo(new[] { Type1 }));
        }

        #region TestOnRoundStartAfter

        [Test(Description = "幕の開始時、手札が0枚の場合は何もタイプ付与されない。")]
        public void TestOnRoundStartAfter_Hand0()
        {
            owner.allyCardDetail.DrawCards(0);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が1枚の場合はその手札にタイプ付与される。")]
        public void TestOnRoundStartAfter_Hand1()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(1));
        }

        [Test(Description = "幕の開始時、手札が2枚の場合はその手札全てにタイプ付与される。")]
        public void TestOnRoundStartAfter_Hand2()
        {
            owner.allyCardDetail.DrawCards(2);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(2));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(2));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(2));
        }

        [Test(Description = "幕の開始時、手札が3枚の場合は手札2枚にタイプ付与される。")]
        public void TestOnRoundStartAfter_Hand3()
        {
            owner.allyCardDetail.DrawCards(3);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(3));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(3));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(2));
        }

        #endregion

        #region TestBeforeGiveDamage

        [Test(Description = "バトルページに付与されたタイプがタイプ1に一致しない場合はボーナスなし。")]
        public void TestBeforeGiveDamage_TypeUnmatched()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel ownerCard = owner.allyCardDetail.GetHand().FirstOrDefault();

            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = null,
                CurrentBehavior = ownerDice,
                Card = ownerCard,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));

            ownerCard.AddBuf(new BattleDiceCardBuf_Type(PokeType.Normal));
            passive.BeforeGiveDamage(ownerDice);

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1と一致する場合はダメージボーナス+1。")]
        public void TestBeforeGiveDamage_TypeMatched1()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel ownerCard = owner.allyCardDetail.GetHand().FirstOrDefault();

            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = null,
                CurrentBehavior = ownerDice,
                Card = ownerCard,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));

            ownerCard.AddBuf(new BattleDiceCardBuf_Type(Type1));
            passive.BeforeGiveDamage(ownerDice);

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(1));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));
        }

        #endregion

        [Test]
        public void TestHasType()
        {
            Assert.That(passive.HasType(Type1), Is.EqualTo(true));
            Assert.That(passive.HasType(PokeType.Normal), Is.EqualTo(false));
        }
    }

    #endregion

    #region 複合タイプのテスト

    [TestFixture(Description = "複合タイプのテスト。")]
    public class PassiveAbilityTypeBaseTest_MultiTypes
    {
        /// <summary>このテストで使用する1番目のタイプ</summary>
        private const PokeType Type1 = PokeType.Grass;
        /// <summary>このテストで使用する2番目のタイプ</summary>
        private const PokeType Type2 = PokeType.Poison;

        private class PassiveAbility_MultiTypes : PassiveAbilityTypeBase
        {
            public override IEnumerable<PokeType> Types => new[] { Type1, Type2 };
        }

        private FixedRandomizer randomizer;
        private BattleUnitModel owner;
        private PassiveAbilityTypeBase passive;

        [SetUp]
        public void SetUp()
        {
            randomizer = new FixedRandomizer();
            passive = new PassiveAbility_MultiTypes()
            {
                Randomizer = randomizer,
            };
            owner = new BattleUnitModelBuilder()
            {
                Passives = new[] { passive }
            }.ToBattleUnitModel();
        }

        [Test]
        public void TestTypes()
        {
            Assert.That(passive.Types, Is.EqualTo(new[] { PokeType.Grass, PokeType.Poison }));
        }

        #region TestOnRoundStartAfter

        [Test(Description = "幕の開始時、手札が0枚の場合は何もタイプ付与されない。")]
        public void TestOnRoundStartAfter_Hand0()
        {
            owner.allyCardDetail.DrawCards(0);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が1枚の場合はその手札にタイプ付与される。")]
        public void TestOnRoundStartAfter_Hand1()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type1), Is.EqualTo(0));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type2), Is.EqualTo(0));

            randomizer.SelectOneIndex = 0;
            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type1), Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type2), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が2枚の場合はその手札全てにタイプ1とタイプ2がそれぞれ付与される。")]
        public void TestOnRoundStartAfter_Hand2()
        {
            owner.allyCardDetail.DrawCards(2);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(2));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type1), Is.EqualTo(0));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type2), Is.EqualTo(0));

            randomizer.SelectOneIndex = 0;
            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(2));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type1), Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().OfType<BattleDiceCardBuf_Type>().Count(b => b.Type == Type2), Is.EqualTo(1));
        }

        [Test(Description = "幕の開始時、手札が3枚の場合は手札2枚にタイプ1とタイプ2がそれぞれ付与される。")]
        public void TestOnRoundStartAfter_Hand3()
        {
            owner.allyCardDetail.DrawCards(3);
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(3));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(0));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type2)), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(owner.allyCardDetail.GetHand().Count, Is.EqualTo(3));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type1)), Is.EqualTo(1));
            Assert.That(owner.allyCardDetail.GetHand().Count(c => c.HasType(Type2)), Is.EqualTo(1));
        }

        #endregion

        #region TestBeforeGiveDamage

        [Test(Description = "バトルページに付与されたタイプがタイプ1にもタイプ2にも一致しない場合はボーナスなし。")]
        public void TestBeforeGiveDamage_TypeUnmatched()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel ownerCard = owner.allyCardDetail.GetHand().FirstOrDefault();

            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = null,
                CurrentBehavior = ownerDice,
                Card = ownerCard,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));

            ownerCard.AddBuf(new BattleDiceCardBuf_Type(PokeType.Normal));
            passive.BeforeGiveDamage(ownerDice);

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1と一致する場合はダメージボーナス+1。")]
        public void TestBeforeGiveDamage_TypeMatched1()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel ownerCard = owner.allyCardDetail.GetHand().FirstOrDefault();

            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = null,
                CurrentBehavior = ownerDice,
                Card = ownerCard,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));

            ownerCard.AddBuf(new BattleDiceCardBuf_Type(Type1));
            passive.BeforeGiveDamage(ownerDice);

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(1));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ2と一致する場合はダメージボーナス+1。")]
        public void TestBeforeGiveDamage_TypeMatched2()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel ownerCard = owner.allyCardDetail.GetHand().FirstOrDefault();

            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = null,
                CurrentBehavior = ownerDice,
                Card = ownerCard,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(0));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));

            ownerCard.AddBuf(new BattleDiceCardBuf_Type(Type2));
            passive.BeforeGiveDamage(ownerDice);

            Assert.That(ownerDice.DamageAdder, Is.EqualTo(1));
            Assert.That(ownerDice.BreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.PowerAdder, Is.EqualTo(0));
            Assert.That(ownerDice.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GetDiceMin(), Is.EqualTo(1));
            Assert.That(ownerDice.GetDiceMax(), Is.EqualTo(1));
            Assert.That(ownerDice.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(ownerDice.GuardBreakMultiplier, Is.EqualTo(1));
        }

        #endregion

        [Test]
        public void TestHasType()
        {
            Assert.That(passive.HasType(Type1), Is.EqualTo(true));
            Assert.That(passive.HasType(Type2), Is.EqualTo(true));
            Assert.That(passive.HasType(PokeType.Normal), Is.EqualTo(false));
            Assert.That(passive.HasType(PokeType.Dark), Is.EqualTo(false));
        }
    }

    #endregion
}
