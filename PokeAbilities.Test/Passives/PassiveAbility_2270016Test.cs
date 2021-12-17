using NUnit.Framework;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;
using BattleDiceCardModelBuilder = PokeAbilities.Test.Helpers.Builders.BattleDiceCardModelBuilder;
using BattleUnitModelBuilder = PokeAbilities.Test.Helpers.Builders.BattleUnitModelBuilder;
using BookXmlInfoBuilder = PokeAbilities.Test.Helpers.Builders.BookXmlInfoBuilder;

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
                EquipBook = new BookXmlInfoBuilder()
                {
                    Hp = 100,
                },
            }.Build();
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
            }.Build();

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
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
                //CurrentDiceAction = cardData,
            }.Build();
            BattleDiceCardModel card = new BattleDiceCardModelBuilder().Build(attaker);
            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                card = card,
            };

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
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.Build();
            BattleDiceCardModel card = new BattleDiceCardModelBuilder().Build(attaker);
            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                card = card,
            };

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
