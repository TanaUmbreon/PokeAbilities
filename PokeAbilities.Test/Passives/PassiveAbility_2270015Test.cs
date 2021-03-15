using System.Collections.Generic;
using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;
using PokeAbilities.Test.Helpers;
using UnityEngine;

namespace PokeAbilities.Test.Passives
{
    [TestFixture]
    public class PassiveAbility_2270015Test
    {
        private BattleUnitModel owner;

        [SetUp]
        public void SetUp()
        {
            BaseMod.Harmony_Patch.ArtWorks = new Dictionary<string, Sprite>();

            var builder = new BattleUnitModelBuilder();
            owner = builder.ToBattleUnitModel();
        }

        [Test(Description = "にほんばれ状態でない時、何も付与されない。")]
        public void TestOnRoundStart1()
        {
            var passive = new PassiveAbility_2270015();
            passive.Init(owner);
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);
        }

        [Test(Description = "にほんばれ状態の時、この幕にクイック2が付与される。")]
        public void TestOnRoundStart2()
        {
            var passive = new PassiveAbility_2270015();
            passive.Init(owner);
            owner.bufListDetail.AddBuf(new BattleUnitBuf_SunnyDay() { stack = 5 });
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Null);
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);

            passive.OnRoundStart();
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness), Is.Not.Null);
            Assert.That(owner.bufListDetail.GetActivatedBuf(KeywordBuf.Quickness).stack, Is.EqualTo(2));
            Assert.That(owner.bufListDetail.GetReadyBuf(KeywordBuf.Quickness), Is.Null);
        }
    }
}
