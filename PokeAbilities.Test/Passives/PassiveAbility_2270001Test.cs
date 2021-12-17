using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;
using System;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270001Test
    {
        #region SetUp

        /// <summary>
        /// MOD で追加されるマイナス属性状態 (状態異常)。
        /// </summary>
        private class CustomNegativeBuf : BattleUnitBuf
        {
            public override BufPositiveType positiveType => BufPositiveType.Negative;
        }

        /// <summary>
        /// MOD で追加されるプラス属性状態。
        /// </summary>
        private class CustomPositiveBuf : BattleUnitBuf
        {
            public override BufPositiveType positiveType => BufPositiveType.Positive;
        }

        /// <summary>
        /// MOD で追加される無属性状態。
        /// </summary>
        private class CustomNoneBuf : BattleUnitBuf
        {
            public override BufPositiveType positiveType => BufPositiveType.None;
        }

        /// <summary>テスト対象のパッシブ</summary>
        private PassiveAbility_2270001 passive;
        /// <summary>パッシブの所有キャラクター</summary>
        private BattleUnitModel owner;

        /// <summary>味方キャラクター</summary>
        private BattleUnitModel ally;
        /// <summary>味方キャラクターが手札に所有しているバトルページ</summary>
        private BattleDiceCardModel allyCardInHand;

        /// <summary>敵キャラクター</summary>
        private BattleUnitModel enemy;
        /// <summary>敵キャラクターが手札に所有しているバトルページ</summary>
        private BattleDiceCardModel enemyCardInHand;

        [SetUp]
        public void Setup()
        {
            passive = new PassiveAbility_2270001();
            owner = new BattleUnitModelBuilder()
            {
                Passives = new PassiveAbilityBase[] { passive },
                Faction = Faction.Player,
            }.Build();

            ally = new BattleUnitModelBuilder()
            {
                Faction = Faction.Player,
            }.Build();
            ally.allyCardDetail.DrawCards(1);
            allyCardInHand = BattleInfo.GetHandAt(ally, 0);

            enemy = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.Build();
            enemy.allyCardDetail.DrawCards(1);
            enemyCardInHand = BattleInfo.GetHandAt(enemy, 0);

            Assert.That(owner.bufListDetail.GetActivatedBufList(), Is.Empty);
            Assert.That(owner.bufListDetail.GetReadyBufList(), Is.Empty);
            Assert.That(owner.bufListDetail.GetReadyReadyBufList(), Is.Empty);
        }

        #endregion

        public enum Attacker
        {
            Enemy,
            Ally,
        }

        public enum Attack
        {
            OneSide,
            Parrying,
        }

        public enum AdditionalBuf
        {
            Neutral,
            Negative,
            Positive,
        }

        private BattleDiceBehavior CreateAttackingBehavior(Attacker attacker)
        {
            switch (attacker)
            {
                case Attacker.Enemy:
                    return null;
                case Attacker.Ally:
                    return null;
            }
            throw new ArgumentException(nameof(attacker));
        }

        #region BattleUnitBufListDetail.AddBuf メソッドによる被状態付与

        //[TestCase(Attacker.Enemy, Attack.OneSide, AdditionalBuf.Negative)]
        //public void TestAddBuf(Attacker attacker, Attack attack, AdditionalBuf buf)
        //{
        //    owner.bufListDetail.AddBuf(buf);
        //}

        [Test(Description = "敵が一方攻撃でマイナス属性状態 (状態異常) を与えた場合は付与できない。")]
        public void TestAddBuf_TakeNegativeBufFromEnemyOneSideAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartTargetedOneSide(enemyBehavior.card);
            AssertThatCanNotAddBufToOwner(new BattleUnitBuf_burn() { stack = 1 });
        }

        [Test(Description = "敵が一方攻撃でプラス属性状態を与えた場合は付与できる。")]
        public void TestAddBuf_TakePositiveBufFromEnemyOneSideAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartTargetedOneSide(enemyBehavior.card);
            AssertThatCanAddBufToOwner(new BattleUnitBuf_quickness() { stack = 1 });
        }

        [Test(Description = "敵が一方攻撃で無属性状態を与えた場合は付与できる。")]
        public void TestAddBuf_TakeNoneBufFromEnemyOneSideAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartTargetedOneSide(enemyBehavior.card);
            AssertThatCanAddBufToOwner(new BattleUnitBuf_smoke() { stack = 1 });
        }

        [Test(Description = "敵が一方攻撃でMODのマイナス属性状態 (状態異常) を与えた場合は付与できない。")]
        public void TestAddBuf_TakeCustomNegativeBufFromEnemyOneSideAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartTargetedOneSide(enemyBehavior.card);
            AssertThatCanNotAddBufToOwner(new CustomNegativeBuf() { stack = 1 });
        }

        [Test(Description = "敵が一方攻撃でMODのプラス属性状態を与えた場合は付与できる。")]
        public void TestAddBuf_TakeCustomPositiveBufFromEnemyOneSideAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartTargetedOneSide(enemyBehavior.card);
            AssertThatCanAddBufToOwner(new CustomPositiveBuf() { stack = 1 });
        }

        [Test(Description = "敵が一方攻撃でMODの無属性状態を与えた場合は付与できる。")]
        public void TestAddBuf_TakeCustomNoneBufFromEnemyOneSideAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartTargetedOneSide(enemyBehavior.card);
            AssertThatCanAddBufToOwner(new CustomNoneBuf() { stack = 1 });
        }



        [Test(Description = "敵がマッチでマイナス属性状態 (状態異常) を与えた場合は付与できない。")]
        public void TestAddBuf_TakeNegativeBufFromEnemyParryingAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartParrying(enemyBehavior.card);
            AssertThatCanNotAddBufToOwner(new BattleUnitBuf_burn() { stack = 1 });
        }

        [Test(Description = "敵がマッチでプラス属性状態を与えた場合は付与できる。")]
        public void TestAddBuf_TakePositiveBufFromEnemyParryingAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartParrying(enemyBehavior.card);
            AssertThatCanAddBufToOwner(new BattleUnitBuf_quickness() { stack = 1 });
        }

        [Test(Description = "敵がマッチで無属性状態を与えた場合は付与できる。")]
        public void TestAddBuf_TakeNoneBufFromEnemyParryingAttack()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartParrying(enemyBehavior.card);
            AssertThatCanAddBufToOwner(new BattleUnitBuf_smoke() { stack = 1 });
        }


        [Test(Description = "敵からバトルページによるマッチで状態異常を受けた場合は無効化できる。")]
        public void TestAddBuf_ParryingAttackFromEnemy()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartParrying(enemyBehavior.card);
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);

            owner.bufListDetail.AddBuf(new BattleUnitBuf_burn() { stack = 1 });
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);
        }

        [Test(Description = "味方からバトルページによる一方攻撃で状態異常を受けた場合は被状態付与を無効化できない。")]
        public void TestAddBuf_OneSideAttackFromAlly()
        {
            var enemyBehavior = BattleEmulator.CreateAttackingBehavior(enemyCardInHand, owner, BehaviourDetail.Hit);
            passive.OnStartParrying(enemyBehavior.card);
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);

            owner.bufListDetail.AddBuf(new BattleUnitBuf_burn() { stack = 1 });
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);
        }

        private void AssertThatCanNotAddBufToOwner<T>(T buf) where T : BattleUnitBuf
        {
            Assert.That(owner.bufListDetail.HasBuf<T>(), Is.False);

            owner.bufListDetail.AddBuf(buf);
            Assert.That(owner.bufListDetail.HasBuf<T>(), Is.False);
        }

        private void AssertThatCanAddBufToOwner<T>(T buf) where T : BattleUnitBuf
        {
            Assert.That(owner.bufListDetail.HasBuf<T>(), Is.False);

            owner.bufListDetail.AddBuf(buf);
            Assert.That(owner.bufListDetail.HasBuf<T>(), Is.True);
        }

        #endregion

        private void Test()
        {
        }

        [Test()]
        public void TestAddBuf_CanAddPositive()
        {
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);

            owner.bufListDetail.AddBuf(new BattleUnitBuf_burn());
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);
        }

        [Test()]
        public void TestAddBufWithConnection_CanAddNegative()
        {
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);

            owner.bufListDetail.AddBufWithConnection(new BattleUnitBuf_burn(), ally);
            owner.bufListDetail.AddBufWithoutDuplication(new BattleUnitBuf_burn());
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, enemy);
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1, enemy);
            owner.bufListDetail.AddKeywordBufNextNextByCard(KeywordBuf.Bleeding, 1, enemy);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 1, enemy);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Bleeding, 1, enemy);
            owner.bufListDetail.AddReadyBuf(new BattleUnitBuf_burn());
            owner.bufListDetail.AddReadyReadyBuf(new BattleUnitBuf_burn());
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.False);
        }

        [Test(Description = "敵のバトルページによって状態異常を付与される場合")]
        public void OnAddKeywordBufByCard_AddedFromEnemyBattlePage()
        {

        }
    }
}
