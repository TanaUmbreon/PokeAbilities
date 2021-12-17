using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270019Test
    {
        private PassiveAbility_2270019 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270019();
        }

        [Test(Description = "あめ状態でない時、HPは変化しない。")]
        public void TestOnRoundEnd1()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 100, currentHp: 90, hasRainBuf: false);
            Assert.That(owner.hp, Is.EqualTo(90));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(90));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp15()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 15, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(2));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp16()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 16, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(2));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp17()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 17, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(2));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp31()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 31, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(2));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp32()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 32, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(3));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp47()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 47, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(3));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp48()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 48, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(4));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp79()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 79, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(5));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp80()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 80, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(6));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。")]
        public void TestOnRoundEnd2_MaxHp95()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 95, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(6));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。(回復上限5に達する)")]
        public void TestOnRoundEnd2_MaxHp96()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 96, currentHp: 1, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(1));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(6));
        }

        [Test(Description = "あめ状態の時、最大体力の1/16だけ体力が回復する。(最大HPまで回復)")]
        public void TestOnRoundEnd2_RecovertToMaxHp()
        {
            BattleUnitModel owner = CreateOwner(maxHp: 50, currentHp: 49, hasRainBuf: true);
            Assert.That(owner.hp, Is.EqualTo(49));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(50));
        }

        /// <summary>
        /// 指定したパラメータで、パッシブ所有キャラクターを生成します。
        /// </summary>
        /// <param name="maxHp">パッシブ所有キャラクターの最大体力。</param>
        /// <param name="currentHp">パッシブ所有キャラクターの現在体力。</param>
        /// <param name="hasRainBuf">あめ状態を保有している事を示す値。</param>
        /// <returns></returns>
        private BattleUnitModel CreateOwner(int maxHp, int currentHp, bool hasRainBuf)
        {
            BookXmlInfo equipBook = new BookXmlInfoBuilder()
            {
                Hp = maxHp,
            }.ToBookXmlInfo();

            BattleUnitModel owner = new BattleUnitModelBuilder()
            {
                EquipBook = equipBook,
                Passives = new[] { passive },
            }.Build();

            if (maxHp != currentHp)
            {
                owner.SetHp(currentHp);
            }

            if (hasRainBuf)
            {
                owner.bufListDetail.AddBuf(new BattleUnitBuf_Rain() { stack = 5 });
            }

            return owner;
        }
    }
}
