using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270007Test
    {
        private PassiveAbility_2270007 passive;
        private BattleUnitModel owner;
        private BattleUnitModel enemy;

        [SetUp]
        public void Setup()
        {
            var card = new BattleDiceCardModelBuilder()
            {
            };

            passive = new PassiveAbility_2270007();
            owner = new BattleUnitModelBuilder()
            {
                Passives = new PassiveAbilityBase[] { passive },
            }.ToBattleUnitModel();
            enemy = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.ToBattleUnitModel();
        }

        #region 虚弱状態に対する耐性のテスト

        [Test(Description = "虚弱状態に対して耐性を返す。")]
        public void TestIsImmune_Weak()
        {
            Assert.That(passive.IsImmune(KeywordBuf.Weak), Is.True);
        }

        [Test(Description = "虚弱状態以外に対して非耐性を返す。")]
        public void TestIsImmune_ExclusiveWeak()
        {
            Assert.That(passive.IsImmune(KeywordBuf.Burn), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Paralysis), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Bleeding), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Decay), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Fairy), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Stun), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Binding), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Disarm), Is.False);
            Assert.That(passive.IsImmune(KeywordBuf.Vulnerable), Is.False);
        }

        [Test(Description = "虚弱状態が次の幕に付与されることは可能。")]
        public void Test_CanAddWeakNextRound()
        {
            Assert.That(owner.bufListDetail.HasBuf(KeywordBuf.Weak), Is.False);

            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1);
            Assert.That(owner.bufListDetail.HasBuf(KeywordBuf.Weak), Is.False);

            owner.OnRoundStart_before();
            Assert.That(owner.bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);
        }

        [Test(Description = "虚弱状態がこの幕に付与されることは可能。")]
        public void Test_CanAddWeakThisRound()
        {
            Assert.That(owner.bufListDetail.HasBuf(KeywordBuf.Weak), Is.False);

            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            Assert.That(owner.bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);
        }

        [Test(Description = "虚弱状態が付与されていてもダイス威力低下は発生しない。")]
        public void Test_NotPowerDownOnWeak()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 3);
            Assert.That(owner.bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);

            AssertThatPowerDownNotAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatPowerDownNotAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatPowerDownNotAppliedUsing(card, BehaviourDetail.Slash);
        }

        /// <summary>
        /// 指定したバトル ページで指定した振る舞いのバトル ダイスを使用した場合、
        /// パッシブ効果によるダイス威力低下が適用されない事を表明します。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="detail"></param>
        private void AssertThatPowerDownNotAppliedUsing(BattleDiceCardModel card, BehaviourDetail detail)
        {
            BattleDiceBehavior behavior = BattleEmulator.CreateAttackingBehavior(
                card, enemy, detail, diceVanillaValue: 5);
            behavior.UpdateDiceFinalValue();
            Assert.That(behavior.DiceResultValue, Is.EqualTo(5));
        }

        #endregion

        #region ダイス威力増加のテスト

        [Test(Description = "攻撃ダイスかつ、状態異常である場合はダイス威力+1のボーナスが適用")]
        public void TestBoforeRollDice_AttackDiceAndHasNegative()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 1);
            Assert.That(owner.bufListDetail.HasBuf(BufPositiveType.Negative), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);

            AssertThatPowerUpAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatPowerUpAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatPowerUpAppliedUsing(card, BehaviourDetail.Slash);
        }

        [Test(Description = "防御ダイスかつ、状態異常である場合はボーナスなし")]
        public void TestBoforeRollDice_DefenceDiceAndHasNegative()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 1);
            Assert.That(owner.bufListDetail.HasBuf(BufPositiveType.Negative), Is.True);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);

            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        [Test(Description = "攻撃ダイスかつ、状態異常でない場合はボーナスなし")]
        public void TestBoforeRollDice_AttackDiceAndHasNotNegative()
        {
            Assert.That(owner.bufListDetail.HasBuf(BufPositiveType.Negative), Is.False);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);

            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Hit);
            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Penetrate);
            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Slash);
        }

        [Test(Description = "防御ダイスかつ、状態異常でない場合はボーナスなし")]
        public void TestBoforeRollDice_DefenceDiceAndHasNotNegative()
        {
            Assert.That(owner.bufListDetail.HasBuf(BufPositiveType.Negative), Is.False);

            owner.allyCardDetail.DrawCards(1);
            BattleDiceCardModel card = BattleInfo.GetHandAt(owner, 0);

            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Guard);
            AssertThatPowerUpNotAppliedUsing(card, BehaviourDetail.Evasion);
        }

        /// <summary>
        /// 指定したバトルページで指定した振る舞いのバトルダイスを使用した場合、
        /// パッシブ効果による威力増加が適用された事を表明します。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="detail"></param>
        private void AssertThatPowerUpAppliedUsing(BattleDiceCardModel card, BehaviourDetail detail)
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

            passive.BeforeRollDice(behavior);
            Assert.That(behavior.DamageAdder, Is.EqualTo(0));
            Assert.That(behavior.BreakAdder, Is.EqualTo(0));
            Assert.That(behavior.PowerAdder, Is.EqualTo(1));
            Assert.That(behavior.DiceFaceAdder, Is.EqualTo(0));
            Assert.That(behavior.GetDiceMin(), Is.EqualTo(1));
            Assert.That(behavior.GetDiceMax(), Is.EqualTo(1));
            Assert.That(behavior.GuardBreakAdder, Is.EqualTo(0));
            Assert.That(behavior.GuardBreakMultiplier, Is.EqualTo(1));
        }

        /// <summary>
        /// 指定したバトルページで指定した振る舞いのバトルダイスを使用した場合、
        /// パッシブ効果による威力増加が適用されない事を表明します。
        /// </summary>
        /// <param name="card"></param>
        /// <param name="detail"></param>
        private void AssertThatPowerUpNotAppliedUsing(BattleDiceCardModel card, BehaviourDetail detail)
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

            passive.BeforeRollDice(behavior);
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
}
