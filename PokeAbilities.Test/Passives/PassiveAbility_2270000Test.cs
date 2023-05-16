using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;
using PokeAbilities.Test.Helpers.Builders;
using System.Collections.Generic;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270000Test
    {
        private PassiveAbility_2270000 passive;

        /// <summary>パッシブ効果の適用対象となる状態のコレクション</summary>
        private IEnumerable<BattleUnitBuf> targetBufs;
        /// <summary>パッシブ効果の適用対象とならない状態のコレクション</summary>
        private IEnumerable<BattleUnitBuf> nonTargetBufs;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270000();

            targetBufs = new BattleUnitBuf[]
            {
                new BattleUnitBuf_bleeding() { stack = 1 },
                new BattleUnitBuf_paralysis() { stack = 1 },
                new BattleUnitBuf_burn() { stack = 1 },
            };

            nonTargetBufs = new BattleUnitBuf[]
            {
                new BattleUnitBuf_bleeding() { stack = 0 },
                new BattleUnitBuf_paralysis() { stack = 0 },
                new BattleUnitBuf_burn() { stack = 0 },
                new BattleUnitBuf_smoke() { stack = 1 },
                new BattleUnitBuf_weak() { stack = 1 },
                new BattleUnitBuf_Decay() { stack = 1 },
            };
        }

        #region OnAddKeywordBufByCard

        [Test(Description = "attackerフィールドがnullの場合、何も起きない")]
        public void TestOnAddKeywordBufByCard1()
        {
            SetUpPassiveOwner();

            BattleUnitModel attacker = null;
            PrivateAccess.SetField(passive, "attacker", attacker);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));

            foreach (BattleUnitBuf givingBuf in targetBufs)
            {
                Assert.That(() => passive.OnAddKeywordBufByCard(givingBuf, givingBuf.stack), Throws.Nothing);
                Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));
            }

            foreach (BattleUnitBuf givingBuf in nonTargetBufs)
            {
                Assert.That(() => passive.OnAddKeywordBufByCard(givingBuf, givingBuf.stack), Throws.Nothing);
                Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));
            }
        }

        [Test(Description = "attackerフィールドが非nullで非対象の状態の場合、何も起きない")]
        public void TestOnAddKeywordBufByCard2()
        {
            SetUpPassiveOwner();

            BattleUnitModel attacker = new BattleUnitModelBuilder().Build();
            PrivateAccess.SetField(passive, "attacker", attacker);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));

            foreach (BattleUnitBuf givingBuf in nonTargetBufs)
            {
                attacker.bufListDetail.GetBufInfo()
                Assert.That(BattleInfo.HasBufAny(attacker, givingBuf), Is.False);

                passive.OnAddKeywordBufByCard(givingBuf, givingBuf.stack);
                Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));
                Assert.That(BattleInfo.HasBufAny(attacker, givingBuf), Is.False);
            }
        }

        [Test(Description = "attackerフィールドが非nullで非対象の状態の場合、何も起きない")]
        public void TestOnAddKeywordBufByCard2()
        {
            SetUpPassiveOwner();

            BattleUnitModel attacker = new BattleUnitModelBuilder().Build();
            PrivateAccess.SetField(passive, "attacker", attacker);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));

            foreach (BattleUnitBuf givingBuf in targetBufs)
            {
                Assert.That(BattleInfo.HasBufAny(attacker, givingBuf), Is.False);

                passive.OnAddKeywordBufByCard(givingBuf, givingBuf.stack);
                Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));
                Assert.That(BattleInfo.HasBufAny(attacker, givingBuf), Is.True);
            }

            foreach (BattleUnitBuf givingBuf in nonTargetBufs)
            {
                Assert.That(BattleInfo.HasBufAny(attacker, givingBuf), Is.False);

                passive.OnAddKeywordBufByCard(givingBuf, givingBuf.stack);
                Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));
                Assert.That(BattleInfo.HasBufAny(attacker, givingBuf), Is.False);
            }
        }

        #endregion

        #region OnStartTargetedOneSide

        [Test(Description = "引数がnullの場合、attackerフィールドがnullになる")]
        public void TestOnStartTargetedOneSide1()
        {
            BattleUnitModel attacker = new BattleUnitModelBuilder().Build();
            PrivateAccess.SetField(passive, "attacker", attacker);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));

            passive.OnStartTargetedOneSide(null);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.Null);
        }

        [Test(Description = "味方からの一方攻撃の場合、attackerフィールドがnullになる")]
        public void TestOnStartTargetedOneSide2()
        {
            BattleUnitModel attacker = new BattleUnitModelBuilder()
            {
                Faction = RelativeFactionUtil.GetFaction(passive.Owner, RelativeFaction.Ally),
            }.Build();
            PrivateAccess.SetField(passive, "attacker", attacker);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.EqualTo(attacker));

            BattleDiceCardModel attackerCard = new BattleDiceCardModelBuilder().Build(attacker);
            passive.OnStartTargetedOneSide(attackerCard.CreateDiceCardBehaviorList);
            Assert.That(PrivateAccess.GetField<BattleUnitModel>(passive, "attacker"), Is.Null);
        }

        #endregion

        private BattleUnitModel SetUpPassiveOwner(Faction faction = Faction.Player)
        {
            return new BattleUnitModelBuilder()
            {
                Passives = new[] { passive },
                Faction = faction,
            }.Build();
        }
    }
}