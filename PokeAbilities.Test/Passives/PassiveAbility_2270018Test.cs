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
    public class PassiveAbility_2270018Test
    {
        private PassiveAbility_2270018 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270018();

            BattleUnitModel owner = new BattleUnitModelBuilder().ToBattleUnitModel();
            passive.Init(owner);
        }

        [Test(Description = "マッチ対象がいない場合は何も起こらない。")]
        public void TestOnStartParrying1()
        {
            var ownerCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Target = null,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(() => { passive.OnStartParrying(ownerCard); }, Throws.Nothing);
        }

        [Test(Description = "マッチ対象にバトルページがない場合は何も起こらない。")]
        public void TestOnStartParrying2()
        {
            var target = new BattleUnitModelBuilder()
            {
                CurrentDiceAction = null,
            }.ToBattleUnitModel();

            var ownerCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Target = target,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(() => { passive.OnStartParrying(ownerCard); }, Throws.Nothing);
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(斬撃ダイス)。")]
        public void TestOnStartParrying3_Slash()
        {
            BattlePlayingCardDataInUnitModel ownerCard = CreateParryingOwnerCard(
                targetDiceDetail: BehaviourDetail.Slash,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            // マッチ進行と最終ダイス値の計算
            passive.OnStartParrying(ownerCard);
            BattleDiceBehavior behavior = ownerCard.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(貫通ダイス)。")]
        public void TestOnStartParrying3_Penetrate()
        {
            BattlePlayingCardDataInUnitModel ownerCard = CreateParryingOwnerCard(
                targetDiceDetail: BehaviourDetail.Penetrate,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.OnStartParrying(ownerCard);
            BattleDiceBehavior behavior = ownerCard.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(打撃ダイス)。")]
        public void TestOnStartParrying3_Hit()
        {
            BattlePlayingCardDataInUnitModel ownerCard = CreateParryingOwnerCard(
                targetDiceDetail: BehaviourDetail.Hit,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.OnStartParrying(ownerCard);
            BattleDiceBehavior behavior = ownerCard.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(防御ダイス)。")]
        public void TestOnStartParrying3_Guard()
        {
            BattlePlayingCardDataInUnitModel ownerCard = CreateParryingOwnerCard(
                targetDiceDetail: BehaviourDetail.Guard,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.OnStartParrying(ownerCard);
            BattleDiceBehavior behavior = ownerCard.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイスの場合、相手のバトルダイスは威力の影響を受けない(威力プラス状態)。")]
        public void TestOnStartParrying4_PowerUp()
        {
            BattlePlayingCardDataInUnitModel ownerCard = CreateParryingOwnerCard(
                targetDiceDetail: BehaviourDetail.Evasion,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.OnStartParrying(ownerCard);
            BattleDiceBehavior behavior = ownerCard.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(5));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイスの場合、相手のバトルダイスは威力の影響を受けない(威力マイナス状態)。")]
        public void TestOnStartParrying4_PowerDown()
        {
            BattlePlayingCardDataInUnitModel ownerCard = CreateParryingOwnerCard(
                targetDiceDetail: BehaviourDetail.Evasion,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: -1);

            passive.OnStartParrying(ownerCard);
            BattleDiceBehavior behavior = ownerCard.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(5));
        }

        /// <summary>
        /// 指定したパラメータで、マッチ状態となっている自身のバトル ページを生成します。
        /// </summary>
        /// <param name="targetDiceDetail">マッチ対象のダイスの振る舞いの詳細。</param>
        /// <param name="targetDiceVanillaValue">マッチ対象の威力のボーナス値が適用される前のダイス値。</param>
        /// <param name="targetDicePowerStatBonus">マッチ対象の威力のボーナス値。</param>
        /// <returns></returns>
        private BattlePlayingCardDataInUnitModel CreateParryingOwnerCard(
            BehaviourDetail targetDiceDetail,
            int targetDiceVanillaValue,
            int targetDicePowerStatBonus)
        {
            BattleDiceBehavior targetDice = new BattleDiceBehaviorBuilder()
            {
                Detail = targetDiceDetail,
                DiceVanillaValue = targetDiceVanillaValue,
                PowerStatBonus = targetDicePowerStatBonus,
            }.ToBattleDiceBehavior();

            var targetCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                CurrentBehavior = targetDice,
            }.ToBattlePlayingCardDataInUnitModel();

            var target = new BattleUnitModelBuilder()
            {
                CurrentDiceAction = targetCard,
            }.ToBattleUnitModel();

            return new BattlePlayingCardDataInUnitModelBuilder()
            {
                Target = target,
            }.ToBattlePlayingCardDataInUnitModel();
        }
    }
}
