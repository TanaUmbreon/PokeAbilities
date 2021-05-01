using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270017Test
    {
        private PassiveAbility_2270017 passive;

        [SetUp]
        public void SetUp()
        {
            passive = new PassiveAbility_2270017();

        }

        [Test(Description = "あられ状態でない時、HPは変化しない。")]
        public void TestOnRoundEnd1()
        {
            BookXmlInfo equipBook = new BookXmlInfoBuilder()
            {
                Hp = 100,
            }.ToBookXmlInfo();

            BattleUnitModel owner = new BattleUnitModelBuilder()
            {
                EquipBook = equipBook,
                Passives = new[] { passive },
            }.ToBattleUnitModel();
            Assert.That(owner.hp, Is.EqualTo(100));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100));
        }

        // ToDo: OnRoundEndであられ状態によるHP回復のテストを実装する。
    }
}
