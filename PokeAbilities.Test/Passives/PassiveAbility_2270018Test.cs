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
        public void TestBeforeRollDice1()
        {
            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            BattlePlayingCardDataInUnitModel ownerCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = null,
                CurrentBehavior = ownerDice,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(() => { passive.BeforeRollDice(ownerDice); }, Throws.Nothing);
        }

        [Test(Description = "マッチ対象にバトルページがない場合は何も起こらない。")]
        public void TestBeforeRollDice2()
        {
            BattleUnitModel target = new BattleUnitModelBuilder().ToBattleUnitModel();
            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            BattlePlayingCardDataInUnitModel ownerCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = target,
                CurrentBehavior = ownerDice,
            }.ToBattlePlayingCardDataInUnitModel();

            Assert.That(() => { passive.BeforeRollDice(ownerDice); }, Throws.Nothing);
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(斬撃ダイス)。")]
        public void TestBeforeRollDice3_Slash()
        {
            BattleDiceBehavior ownerDice = CreateParryingOwnerDice(
                targetDiceDetail: BehaviourDetail.Slash,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            // マッチ進行と最終ダイス値の計算
            passive.BeforeRollDice(ownerDice);
            BattleDiceBehavior behavior = ownerDice.card.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(貫通ダイス)。")]
        public void TestBeforeRollDice3_Penetrate()
        {
            BattleDiceBehavior ownerDice = CreateParryingOwnerDice(
                targetDiceDetail: BehaviourDetail.Penetrate,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.BeforeRollDice(ownerDice);
            BattleDiceBehavior behavior = ownerDice.card.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(打撃ダイス)。")]
        public void TestBeforeRollDice3_Hit()
        {
            BattleDiceBehavior ownerDice = CreateParryingOwnerDice(
                targetDiceDetail: BehaviourDetail.Hit,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.BeforeRollDice(ownerDice);
            BattleDiceBehavior behavior = ownerDice.card.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイス以外の場合、相手のバトルダイスは威力の影響を受ける(防御ダイス)。")]
        public void TestBeforeRollDice3_Guard()
        {
            BattleDiceBehavior ownerDice = CreateParryingOwnerDice(
                targetDiceDetail: BehaviourDetail.Guard,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.BeforeRollDice(ownerDice);
            BattleDiceBehavior behavior = ownerDice.card.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(6));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイスの場合、相手のバトルダイスは威力の影響を受けない(威力プラス状態)。")]
        public void TestBeforeRollDice4_PowerUp()
        {
            BattleDiceBehavior ownerDice = CreateParryingOwnerDice(
                targetDiceDetail: BehaviourDetail.Evasion,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: 1);

            passive.BeforeRollDice(ownerDice);
            BattleDiceBehavior behavior = ownerDice.card.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(5));
        }

        [Test(Description = "マッチ対象のバトルダイスが回避ダイスの場合、相手のバトルダイスは威力の影響を受けない(威力マイナス状態)。")]
        public void TestBeforeRollDice4_PowerDown()
        {
            BattleDiceBehavior ownerDice = CreateParryingOwnerDice(
                targetDiceDetail: BehaviourDetail.Evasion,
                targetDiceVanillaValue: 5,
                targetDicePowerStatBonus: -1);

            passive.BeforeRollDice(ownerDice);
            BattleDiceBehavior behavior = ownerDice.card.target.currentDiceAction.currentBehavior;
            behavior.UpdateDiceFinalValue();

            Assert.That(behavior.DiceResultValue, Is.EqualTo(5));
        }

        /// <summary>
        /// 指定したパラメータで、マッチ状態となっている自身のバトル ダイスを生成します。
        /// </summary>
        /// <param name="targetDiceDetail">マッチ対象のダイスの振る舞いの詳細。</param>
        /// <param name="targetDiceVanillaValue">マッチ対象の威力のボーナス値が適用される前のダイス値。</param>
        /// <param name="targetDicePowerStatBonus">マッチ対象の威力のボーナス値。</param>
        /// <returns></returns>
        private BattleDiceBehavior CreateParryingOwnerDice(
            BehaviourDetail targetDiceDetail,
            int targetDiceVanillaValue,
            int targetDicePowerStatBonus)
        {
            BattleUnitModel target = new BattleUnitModelBuilder().ToBattleUnitModel();
            BattleDiceBehavior targetDice = new BattleDiceBehaviorBuilder()
            {
                Detail = targetDiceDetail,
                DiceVanillaValue = targetDiceVanillaValue,
                PowerStatBonus = targetDicePowerStatBonus,
            }.ToBattleDiceBehavior();
            BattlePlayingCardDataInUnitModel targetCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = target,
                Target = passive.Owner,
                CurrentBehavior = targetDice,
            }.ToBattlePlayingCardDataInUnitModel();

            BattleDiceBehavior ownerDice = new BattleDiceBehaviorBuilder().ToBattleDiceBehavior();
            BattlePlayingCardDataInUnitModel ownerCard = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = passive.Owner,
                Target = target,
                CurrentBehavior = ownerDice,
            }.ToBattlePlayingCardDataInUnitModel();

            return ownerDice;
        }
    }
}
