using System;
using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;
using PokeAbilities.Test.Helpers.Builders;
using BattleDiceBehaviorBuilder = PokeAbilities.Test.Helpers.Builders.BattleDiceBehaviorBuilder;
using BattleUnitModelBuilder = PokeAbilities.Test.Helpers.Builders.BattleUnitModelBuilder;
using BookXmlInfoBuilder = PokeAbilities.Test.Helpers.Builders.BookXmlInfoBuilder;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270000Test
    {
        /// <summary>
        /// 攻撃方法を表します。
        /// </summary>
        public enum AttackType
        {
            /// <summary>一方攻撃</summary>
            OneSide,
            /// <summary>マッチ攻撃</summary>
            Parrying,
            /// <summary>広域攻撃</summary>
            FarArea,
        }

        ///// <summary>
        ///// 追加する状態を表します。
        ///// </summary>
        //public enum BufToAdd
        //{
        //    /// <summary>出血</summary>
        //    Bleeding,
        //    /// <summary>麻痺</summary>
        //    Paralysis,
        //    /// <summary>火傷</summary>
        //    Burn,
        //    /// <summary>その他</summary>
        //    Other,
        //}

        private BattleUnitModel owner;
        private PassiveAbility_2270000 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270000();
            owner = new BattleUnitModelBuilder()
            {
                Passives = new[] { passive },
            }.Build();
        }

        ///// <summary>
        ///// 状態を生成します。
        ///// </summary>
        ///// <param name="bufToAdd"></param>
        //private static BattleUnitBuf CreateBuf(BufToAdd bufToAdd)
        //{
        //    switch (bufToAdd)
        //    {
        //        case BufToAdd.Bleeding:
        //            return new BattleUnitBuf_bleeding() { stack = 1 };
        //        case BufToAdd.Paralysis:
        //            return new BattleUnitBuf_paralysis() { stack = 1 };
        //        case BufToAdd.Burn:
        //            return new BattleUnitBuf_burn() { stack = 1 };
        //        case BufToAdd.Other:
        //            return new BattleUnitBuf_smoke() { stack = 1 };
        //    }

        //    throw new ArgumentOutOfRangeException(nameof(bufToAdd));
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="attacker"></param>
        ///// <param name="victim"></param>
        ///// <param name="attackBy"></param>
        //private static void SetUpAttack(BattleUnitModel attacker, BattleUnitModel victim, AttackType attackBy)
        //{
        //    switch (attackBy)
        //    {
        //        case AttackType.OneSide:
        //            BattleEmulator.SetUpOneSidePlay(attacker, victim);
        //            return;
        //        case AttackType.Parrying:
        //            throw new NotImplementedException();
        //        case AttackType.FarArea:
        //            throw new NotImplementedException();
        //    }

        //    throw new ArgumentOutOfRangeException(nameof(attackBy));
        //}

        public void TestAddBuf() { }

        [Test]
        public void TestFromAddBuf() { }

        [Test]
        public void TestFromAddBufWithoutDuplication() { }

        [Test]
        public void TestFromAddReadyBuf() { }

        [Test]
        public void TestFromAddReadyReadyBuf() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attackerFaction">パッシブ所有キャラクターから見た、攻撃してくるキャラクターの相対的な派閥。</param>
        /// <param name="attackBy">攻撃手段。</param>
        /// <param name="bufType">攻撃によって付与する状態。</param>
        /// <param name="expected">パッシブの効果により相手にも同じ状態が付与された事を示す、期待すべき結果。</param>
        [TestCase(RelativeFaction.Opponent, AttackType.OneSide, KeywordBuf.Bleeding, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.OneSide, KeywordBuf.Paralysis, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.OneSide, KeywordBuf.Burn, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.OneSide, KeywordBuf.Decay, false)]
        [TestCase(RelativeFaction.Opponent, AttackType.Parrying, KeywordBuf.Bleeding, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.Parrying, KeywordBuf.Paralysis, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.Parrying, KeywordBuf.Burn, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.Parrying, KeywordBuf.Decay, false)]
        [TestCase(RelativeFaction.Opponent, AttackType.FarArea, KeywordBuf.Bleeding, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.FarArea, KeywordBuf.Paralysis, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.FarArea, KeywordBuf.Burn, true)]
        [TestCase(RelativeFaction.Opponent, AttackType.FarArea, KeywordBuf.Decay, false)]
        [TestCase(RelativeFaction.Ally, AttackType.OneSide, KeywordBuf.Bleeding, false)]
        [TestCase(RelativeFaction.Ally, AttackType.OneSide, KeywordBuf.Paralysis, false)]
        [TestCase(RelativeFaction.Ally, AttackType.OneSide, KeywordBuf.Burn, false)]
        [TestCase(RelativeFaction.Ally, AttackType.OneSide, KeywordBuf.Decay, false)]
        [TestCase(RelativeFaction.Ally, AttackType.Parrying, KeywordBuf.Bleeding, false)]
        [TestCase(RelativeFaction.Ally, AttackType.Parrying, KeywordBuf.Paralysis, false)]
        [TestCase(RelativeFaction.Ally, AttackType.Parrying, KeywordBuf.Burn, false)]
        [TestCase(RelativeFaction.Ally, AttackType.Parrying, KeywordBuf.Decay, false)]
        [TestCase(RelativeFaction.Ally, AttackType.FarArea, KeywordBuf.Bleeding, false)]
        [TestCase(RelativeFaction.Ally, AttackType.FarArea, KeywordBuf.Paralysis, false)]
        [TestCase(RelativeFaction.Ally, AttackType.FarArea, KeywordBuf.Burn, false)]
        [TestCase(RelativeFaction.Ally, AttackType.FarArea, KeywordBuf.Decay, false)]
        public void TestFromAddKeywordBufByCard(RelativeFaction attackerFaction, AttackType attackBy, KeywordBuf bufType, bool expected)
        {
            BattleUnitModel attacker = new BattleUnitModelBuilder()
            {
                Faction = RelativeFactionUtil.GetFaction(owner, attackerFaction),
            }.Build();
            BattleDiceCardModel attackerCard = new BattleDiceCardModelBuilder().Build(attacker);

            switch (attackBy)
            {
                case AttackType.OneSide:
                    BattleEmulator.SetUpOneSidePlay(attackerCard: attackerCard, target: owner);
                    break;
                default:
                    Thrower.ThrowNotImplementedCase(attackBy.ToString());
                    break;
            }
            Assert.That(BattleInfo.HasBufAny(attacker, bufType), Is.False);
            Assert.That(BattleInfo.HasBufAny(owner, bufType), Is.False);

            owner.bufListDetail.AddKeywordBufByCard(bufType, 1, attacker);
            Assert.That(BattleInfo.HasBufAny(owner, bufType), Is.True);
            Assert.That(BattleInfo.HasBufAny(attacker, bufType), Is.EqualTo(expected));
        }

        [Test]
        public void TestFromAddKeywordBufNextNextByCard()
        {

        }

        [Test]
        public void TestFromAddKeywordBufThisRoundByCard()
        {

        }

        [Test]
        public void TestFromAddKeywordBufByEtc()
        {

        }

        [Test]
        public void TestFromAddKeywordBufThisRoundByEtc()
        {

        }
    }
}