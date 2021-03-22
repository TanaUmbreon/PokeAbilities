using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270017Test
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

        [Test(Description = "あられ状態でない時、HPは変化しない。")]
        public void TestOnRoundEnd1()
        {
            var passive = new PassiveAbility_2270017();
            owner.passiveDetail.AddPassive(passive);
            owner.passiveDetail.OnCreated();
            Assert.That(owner.hp, Is.EqualTo(100));

            passive.OnRoundEnd();
            Assert.That(owner.hp, Is.EqualTo(100));
        }

        // Note: OnRoundEndであられ状態によるHP回復のパターンはテスト不可能。
        //   内部でUnitBattleDataModelを使用し、UnityEngineの参照を回避できないため。
    }
}
