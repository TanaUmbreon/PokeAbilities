using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270016Test
    {
        private BattleUnitModel owner;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder()
            {
                Hp = 100,
            }.ToBattleUnitModel();
        }

        #region OnCreated

        [Test]
        public void TestOnCreated()
        {
            var passive = new PassiveAbility_2270016();
            passive.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));
        }

        #endregion

        #region BeforeTakeDamage

        [Test(Description = "バトルページ以外の被ダメージはダメージ軽減数0。")]
        public void TestBeforeTakeDamage1()
        {
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.ToBattleUnitModel();

            var passive = new PassiveAbility_2270016();
            owner.passiveDetail.AddPassive(passive);
            owner.passiveDetail.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));

            passive.BeforeTakeDamage(attaker, 10);
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));
        }

        [Test(Description = "20%の確率に外れた場合はダメージ軽減数0。")]
        public void TestBeforeTakeDamage2()
        {
            BattleDiceCardModel card = new BattleDiceCardModelBuilder().ToBattleDiceCardModel();
            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                card = card,
            };
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
                CurrentDiceAction = cardData,
            }.ToBattleUnitModel();

            var randomizer = new FixedRandomizer();
            var passive = new PassiveAbility_2270016(randomizer);
            owner.passiveDetail.AddPassive(passive);
            owner.passiveDetail.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));

            randomizer.ValueForProbReturnValue = 0.2f;
            passive.BeforeTakeDamage(attaker, 10);
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));
        }

        [Test(Description = "20%の確率に当たった場合はダメージ軽減数9999。")]
        public void TestBeforeTakeDamage3()
        {
            BattleDiceCardModel card = new BattleDiceCardModelBuilder().ToBattleDiceCardModel();
            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                card = card,
            };
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
                CurrentDiceAction = cardData,
            }.ToBattleUnitModel();

            var randomizer = new FixedRandomizer();
            var passive = new PassiveAbility_2270016(randomizer);
            owner.passiveDetail.AddPassive(passive);
            owner.passiveDetail.OnCreated();
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(0));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(0));

            randomizer.ValueForProbReturnValue = 0.19999f;
            passive.BeforeTakeDamage(attaker, 10);
            Assert.That(passive.GetDamageReductionAll, Is.EqualTo(9999));
            Assert.That(passive.GetBreakDamageReductionAll(0, DamageType.Attack, null), Is.EqualTo(9999));
        }

        #endregion
    }
}
