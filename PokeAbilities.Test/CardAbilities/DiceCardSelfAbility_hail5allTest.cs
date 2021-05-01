using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.CardAbilities;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.CardAbilities
{
    [TestFixture]
    public class DiceCardSelfAbility_hail5allTest
    {
        private BattleUnitModel owner;
        private BattleUnitModel alivingAlly;
        private BattleUnitModel deadAlly;
        private BattleUnitModel alivingOpponent;
        private BattleUnitModel deadOpponent;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder()
            {
                Id = 0,
                Faction = Faction.Player,
            }.ToBattleUnitModel();
            BattleObjectManagerAccess.RegisterUnit(owner);

            alivingAlly = new BattleUnitModelBuilder()
            {
                Id = 1,
                Faction = Faction.Player,
            }.ToBattleUnitModel();
            BattleObjectManagerAccess.RegisterUnit(alivingAlly);

            deadAlly = new BattleUnitModelBuilder()
            {
                Id = 2,
                Faction = Faction.Player,
                IsDie = true,
            }.ToBattleUnitModel();
            BattleObjectManagerAccess.RegisterUnit(deadAlly);

            alivingOpponent = new BattleUnitModelBuilder()
            {
                Id = 3,
                Faction = Faction.Enemy,
            }.ToBattleUnitModel();
            BattleObjectManagerAccess.RegisterUnit(alivingOpponent);

            deadOpponent = new BattleUnitModelBuilder()
            {
                Id = 4,
                Faction = Faction.Enemy,
                IsDie = true,
            }.ToBattleUnitModel();
            BattleObjectManagerAccess.RegisterUnit(deadOpponent);
        }

        [Test(Description = "生存している敵味方全てのキャラクターにあられ5が付与される")]
        public void TestOnUseCard1()
        {
            Assert.That(BattleObjectManager.instance.GetAliveList().Count, Is.EqualTo(3));

            Assert.That(owner.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(owner.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(alivingAlly.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(alivingAlly.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(deadAlly.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(deadAlly.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(alivingOpponent.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(alivingOpponent.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(deadOpponent.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(deadOpponent.bufListDetail.GetReadyBufList().Any(), Is.False);

            var card = new DiceCardSelfAbility_hail5all()
            {
                card = new BattlePlayingCardDataInUnitModel()
                {
                    owner = owner,
                },
            };
            card.OnUseCard();

            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
            Assert.That(owner.bufListDetail.GetActivatedBufList().FirstOrDefault(b => b is BattleUnitBuf_Hail).stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(alivingAlly.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
            Assert.That(alivingAlly.bufListDetail.GetActivatedBufList().FirstOrDefault(b => b is BattleUnitBuf_Hail).stack, Is.EqualTo(5));
            Assert.That(alivingAlly.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(deadAlly.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(deadAlly.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(alivingOpponent.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
            Assert.That(alivingOpponent.bufListDetail.GetActivatedBufList().FirstOrDefault(b => b is BattleUnitBuf_Hail).stack, Is.EqualTo(5));
            Assert.That(alivingOpponent.bufListDetail.GetReadyBufList().Any(), Is.False);

            Assert.That(deadOpponent.bufListDetail.GetActivatedBufList().Any(), Is.False);
            Assert.That(deadOpponent.bufListDetail.GetReadyBufList().Any(), Is.False);
        }

        [Test(Description = "すでに他の天気バフが付与されている場合は、そのバフが削除されて新しくあられ5が付与される")]
        public void TestOnUseCard2()
        {
            var bufList = PrivateAccess.GetField<List<BattleUnitBuf>>(owner.bufListDetail, "_bufList");
            bufList.Add(new BattleUnitBuf_Rain() { stack = 5 });
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Rain>(), Is.True);
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.False);
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.False);
            Assert.That(owner.bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_Hail>().Any(), Is.False);

            var card = new DiceCardSelfAbility_hail5all()
            {
                card = new BattlePlayingCardDataInUnitModel()
                {
                    owner = owner,
                },
            };
            card.OnUseCard();

            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Rain>(), Is.False);
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.False);
            Assert.That(owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
            Assert.That(owner.bufListDetail.GetActivatedBufList().FirstOrDefault(b => b is BattleUnitBuf_Hail).stack, Is.EqualTo(5));
            Assert.That(owner.bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_Hail>().Any(), Is.False);
        }
    }
}
