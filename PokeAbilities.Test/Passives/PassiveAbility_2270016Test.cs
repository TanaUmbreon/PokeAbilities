using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270016Test
    {
        private BattleUnitModel owner;
        private BattleUnitPassiveDetail passiveDetail;
        private BattleUnitModel enemy;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().ToBattleUnitModel();
            owner.SetHp(100);
            passiveDetail = owner.passiveDetail;

            enemy = new BattleUnitModelBuilder() 
            { 
                Faction = Faction.Enemy,
            }.ToBattleUnitModel();
        }

        [Test]
        public void TestOnCreated()
        {
            var passive = passiveDetail.AddPassive(new PassiveAbility_2270016());
            passive.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));
        }

        [Test(Description = "20%の確率に外れた場合はダメージ軽減数0。")]
        public void TestBeforeTakeDamage1()
        {
            var randomizer = new FixedRandomizer();
            var passive = passiveDetail.AddPassive(new PassiveAbility_2270016(randomizer));
            passiveDetail.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));

            randomizer.ValueForProbReturnValue = 0.2f;
            passive.BeforeTakeDamage(enemy, 10);
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));
        }

        [Test(Description = "20%の確率に当たった場合はダメージ軽減数9999。")]
        public void TestBeforeTakeDamage2()
        {
            var randomizer = new FixedRandomizer();
            var passive = passiveDetail.AddPassive(new PassiveAbility_2270016(randomizer));
            passiveDetail.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));

            randomizer.ValueForProbReturnValue = 0.19999f;
            passive.BeforeTakeDamage(enemy, 10);
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(9999));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(9999));
        }
    }
}
