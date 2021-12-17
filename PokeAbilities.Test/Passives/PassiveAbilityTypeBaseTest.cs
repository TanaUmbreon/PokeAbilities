using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
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

        /// <summary>
        /// このテストで使用するタイプ系パッシブ。
        /// </summary>
        private class PassiveAbility_SingleType : PassiveAbilityTypeBase
        {
            public override IEnumerable<PokeType> Types => new[] { Type1 };
        }

        private BattleUnitModel owner;
        private PassiveAbilityTypeBase passive;
        private BattleUnitModel enemy;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_SingleType();
            owner = new BattleUnitModelBuilder()
            {
                Passives = new[] { passive },
            }.Build();
            enemy = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.Build();
        }

        #region 所有するタイプのテスト

        [Test]
        public void TestTypes()
        {
            Assert.That(passive.Types, Is.EqualTo(new[] { Type1 }));
        }

        [Test]
        public void TestHasType()
        {
            Assert.That(passive.HasType(Type1), Is.EqualTo(true));
            Assert.That(passive.HasType(PokeType.Normal), Is.EqualTo(false));
        }

        #endregion

        #region バトルページへのタイプ付与のテスト

        [Test(Description = "幕の開始時、手札が0枚の場合は何もタイプ付与されない。")]
        public void TestOnRoundStartAfter_Hand0()
        {
            owner.allyCardDetail.DrawCards(0);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が1枚の場合はその手札にタイプ付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand1()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(1));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が2枚の場合はその手札全てにタイプ付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand2()
        {
            owner.allyCardDetail.DrawCards(2);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(2));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が3枚の場合は手札2枚にタイプが付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand3()
        {
            owner.allyCardDetail.DrawCards(3);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(2));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が3枚、タイプ付与された手札が1枚の場合は、タイプ付与されていない手札3枚の中から2枚にタイプが付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand3AndTypeHand1()
        {
            owner.allyCardDetail.DrawCards(4);
            BattleInfo.GetHandAt(owner, 0)?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Grass));
            Assert.That(Type1, Is.Not.EqualTo(PokeType.Grass));
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(4));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Grass), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(4));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Grass), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(2));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与された手札が1枚の場合はタイプが付与されない。")]
        public void TestOnRoundStartAfter_TypeHand1()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleInfo.GetHandAt(owner, 0)?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Grass));
            Assert.That(Type1, Is.Not.EqualTo(PokeType.Grass));
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Grass), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Grass), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        #endregion

        #region タイプ一致ボーナスのテスト

        [Test(Description = "バトルページに付与されたタイプがタイプ1に一致しないかつ、攻撃ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_DifferentTypeAndAttackDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Normal));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Hit);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1に一致しないかつ、防御ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_DifferentTypeAndDefenceDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Normal));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Guard);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1と一致するかつ、攻撃ダイスの場合はダメージ・混乱ダメージボーナス+1。")]
        public void TestBeforeGiveDamage_SameType1AndAttackDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(Type1));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Slash);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(1));
            Assert.That(behavior.BreakAdder, Is.EqualTo(1));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1と一致するかつ、防御ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_SameType1AndDefenceDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(Type1));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Evasion);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        #endregion
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
        private BattleUnitModel enemy;

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
                Passives = new[] { passive },
            }.Build();
            enemy = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.Build();
        }

        #region 所有するタイプのテスト

        [Test]
        public void TestTypes()
        {
            Assert.That(passive.Types, Is.EqualTo(new[] { PokeType.Grass, PokeType.Poison }));
        }

        [Test]
        public void TestHasType()
        {
            Assert.That(passive.HasType(Type1), Is.EqualTo(true));
            Assert.That(passive.HasType(Type2), Is.EqualTo(true));
            Assert.That(passive.HasType(PokeType.Normal), Is.EqualTo(false));
            Assert.That(passive.HasType(PokeType.Dark), Is.EqualTo(false));
        }

        #endregion

        #region バトルページへのタイプ付与のテスト

        [Test(Description = "幕の開始時、手札が0枚の場合は何もタイプ付与されない。")]
        public void TestOnRoundStartAfter_Hand0()
        {
            owner.allyCardDetail.DrawCards(0);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が1枚の場合はその手札にタイプ付与される。")]
        public void TestOnRoundStartAfter_Hand1()
        {
            owner.allyCardDetail.DrawCards(1);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            randomizer.SelectOneIndex = 0;
            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が2枚の場合はその手札全てにタイプ1とタイプ2がそれぞれ付与される。")]
        public void TestOnRoundStartAfter_Hand2()
        {
            owner.allyCardDetail.DrawCards(2);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            randomizer.SelectOneIndex = 0;
            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(1));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、手札が3枚の場合は手札2枚にタイプ1とタイプ2がそれぞれ付与される。")]
        public void TestOnRoundStartAfter_Hand3()
        {
            owner.allyCardDetail.DrawCards(3);
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(2));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(1));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与されていない手札が3枚、タイプ付与された手札が1枚の場合は、タイプ付与されていない手札3枚の中から2枚にタイプ1とタイプ2がそれぞれ付与される。")]
        public void TestOnRoundStartAfter_NoTypeHand3AndTypeHand1()
        {
            owner.allyCardDetail.DrawCards(4);
            BattleInfo.GetHandAt(owner, 0)?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Fire));
            Assert.That(Type1, Is.Not.EqualTo(PokeType.Fire));
            Assert.That(Type2, Is.Not.EqualTo(PokeType.Fire));
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(4));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(4));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(3));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Fire), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(1));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        [Test(Description = "幕の開始時、タイプ付与された手札が1枚の場合はタイプが付与されない。")]
        public void TestOnRoundStartAfter_TypeHand1()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleInfo.GetHandAt(owner, 0)?.AddBuf(new BattleDiceCardBuf_Type(PokeType.Water));
            Assert.That(Type1, Is.Not.EqualTo(PokeType.Water));
            Assert.That(Type2, Is.Not.EqualTo(PokeType.Water));
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Water), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));

            passive.OnRoundStartAfter();
            Assert.That(BattleInfo.GetHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, PokeType.Water), Is.EqualTo(1));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type1), Is.EqualTo(0));
            Assert.That(BattleInfo.GetTypeHandCount(owner, Type2), Is.EqualTo(0));
            Assert.That(BattleInfo.GetMultiTypesHandCount(owner), Is.EqualTo(0));
        }

        #endregion

        #region タイプ一致ボーナスのテスト

        [Test(Description = "バトルページに付与されたタイプがタイプ1にもタイプ2にも一致しないかつ、攻撃ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_DifferentTypeAndAttackDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Normal));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Penetrate);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1にもタイプ2にも一致しないかつ、防御ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_DifferentTypeAndDefenceDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(PokeType.Normal));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Evasion);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1と一致するかつ、攻撃ダイスの場合はダメージ・混乱ダメージボーナス+1。")]
        public void TestBeforeGiveDamage_SameType1AndAttackDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(Type1));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Hit);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(1));
            Assert.That(behavior.BreakAdder, Is.EqualTo(1));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ2と一致するかつ、攻撃ダイスの場合はダメージ・混乱ダメージボーナス+1。")]
        public void TestBeforeGiveDamage_SameType2AndAttackDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(Type2));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Slash);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(1));
            Assert.That(behavior.BreakAdder, Is.EqualTo(1));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ1と一致するかつ、防御ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_SameType1AndDefenceDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(Type1));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Guard);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        [Test(Description = "バトルページに付与されたタイプがタイプ2と一致するかつ、防御ダイスの場合はボーナスなし。")]
        public void TestBeforeGiveDamage_SameType2AndDefenceDice()
        {
            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.AddBuf(new BattleDiceCardBuf_Type(Type2));

            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, BehaviourDetail.Evasion);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));

            passive.BeforeGiveDamage(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(0));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        #endregion
    }

    #endregion
}
