using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270004Test
    {
        private PassiveAbility_2270004 passive;
        private BattleUnitModel owner;
        private BattleUnitModel enemy;

        [SetUp]
        public void Setup()
        {
            passive = new PassiveAbility_2270004();
            owner = new BattleUnitModelBuilder()
            {
                Passives = new PassiveAbilityBase[] { passive },
            }.ToBattleUnitModel();
            enemy = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.ToBattleUnitModel();
        }

        [Test(Description = "バトルページのタイプが単体タイプ持ちキャラクターのタイプと一致しない場合はボーナスなし")]
        public void TestBeforeGiveDamage_SingleType_DifferentType()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22701100());
            Assert.That(owner.passiveDetail.HasType(PokeType.Psychic), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Dragon);
            Assert.That(card.HasType(PokeType.Dragon), Is.True);

            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Slash);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        [Test(Description = "バトルページのタイプが単体タイプ持ちキャラクターのタイプと一致するかつ、防御ダイスを使用した場合はボーナスなし")]
        public void TestBeforeGiveDamage_SingleType_SameTypeAndDefenceDice()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22701100());
            Assert.That(owner.passiveDetail.HasType(PokeType.Psychic), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Psychic);
            Assert.That(card.HasType(PokeType.Psychic), Is.True);

            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        [Test(Description = "バトルページのタイプが単体タイプ持ちキャラクターのタイプと一致するかつ、攻撃ダイスを使用した場合はボーナス適用")]
        public void TestBeforeGiveDamage_SingleType_SameTypeAndAttackDice()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22701100());
            Assert.That(owner.passiveDetail.HasType(PokeType.Psychic), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Psychic);
            Assert.That(card.HasType(PokeType.Psychic), Is.True);

            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Slash);
        }

        [Test(Description = "バトルページのタイプが複合タイプ持ちキャラクターのタイプと一致しない場合はボーナスなし")]
        public void TestBeforeGiveDamage_MultiType_DifferentType()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700310());
            Assert.That(owner.passiveDetail.HasType(PokeType.Water), Is.True);
            Assert.That(owner.passiveDetail.HasType(PokeType.Flying), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Bug);
            Assert.That(card.HasType(PokeType.Bug), Is.True);

            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Slash);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        [Test(Description = "バトルページのタイプが複合タイプ持ちキャラクターのタイプ1と一致するかつ、防御ダイスを使用した場合はボーナスなし")]
        public void TestBeforeGiveDamage_MultiType_SameType1AndDefenceDice()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700310());
            Assert.That(owner.passiveDetail.HasType(PokeType.Water), Is.True);
            Assert.That(owner.passiveDetail.HasType(PokeType.Flying), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Water);
            Assert.That(card.HasType(PokeType.Water), Is.True);

            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        [Test(Description = "バトルページのタイプが複合タイプ持ちキャラクターのタイプ2と一致するかつ、防御ダイスを使用した場合はボーナスなし")]
        public void TestBeforeGiveDamage_MultiType_SameType2AndDefenceDice()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700310());
            Assert.That(owner.passiveDetail.HasType(PokeType.Water), Is.True);
            Assert.That(owner.passiveDetail.HasType(PokeType.Flying), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Flying);
            Assert.That(card.HasType(PokeType.Flying), Is.True);

            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatDamageUpHasNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        [Test(Description = "バトルページのタイプが複合タイプ持ちキャラクターのタイプ1と一致するかつ、攻撃ダイスを使用した場合はボーナス適用")]
        public void TestBeforeGiveDamage_MultiType_SameType1AndAttackDice()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700310());
            Assert.That(owner.passiveDetail.HasType(PokeType.Water), Is.True);
            Assert.That(owner.passiveDetail.HasType(PokeType.Flying), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Water);
            Assert.That(card.HasType(PokeType.Water), Is.True);

            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Slash);
        }

        [Test(Description = "バトルページのタイプが複合タイプ持ちキャラクターのタイプ2と一致するかつ、攻撃ダイスを使用した場合はボーナス適用")]
        public void TestBeforeGiveDamage_MultiType_SameType2AndAttackDice()
        {
            BattleEmulator.AddPassive(owner, new PassiveAbility_22700310());
            Assert.That(owner.passiveDetail.HasType(PokeType.Water), Is.True);
            Assert.That(owner.passiveDetail.HasType(PokeType.Flying), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);
            card.TryAddType(PokeType.Flying);
            Assert.That(card.HasType(PokeType.Flying), Is.True);

            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatDamageUpHasAppliedUsing(card, BehaviourDetail.Slash);
        }

        /// <summary>
        /// 指定したバトルページで指定した振る舞いのバトルダイスを使用した場合、
        /// パッシブ効果によるダメージ量増加が適用されたことを表明します。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="detail"></param>
        private void AssertThatDamageUpHasAppliedUsing(BattleDiceCardModel card, BehaviourDetail detail)
        {
            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, detail);
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

        /// <summary>
        /// 指定したバトルページで指定した振る舞いのバトルダイスを使用した場合、
        /// パッシブ効果によるダメージ量増加が適用されないことを表明します。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="detail"></param>
        private void AssertThatDamageUpHasNotAppliedUsing(BattleDiceCardModel card, BehaviourDetail detail)
        {
            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(card, enemy, detail);
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
    }
}
