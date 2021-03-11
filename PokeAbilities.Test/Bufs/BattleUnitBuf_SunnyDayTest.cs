using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PokeAbilities.Bufs;
using UnityEngine;

namespace PokeAbilities.Test.Bufs
{
    [TestFixture]
    public class BattleUnitBuf_SunnyDayTest
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;

        [SetUp]
        public void SetUp()
        {
            BaseMod.Harmony_Patch.ArtWorks = new Dictionary<string, Sprite>();

            owner = new BattleUnitModel(0);
            bufListDetail = owner.bufListDetail;
        }

        [Test(Description = "インスタンス生成直後はスタック数1")]
        public void TestStack1()
        {
            var buf = new BattleUnitBuf_SunnyDay();
            bufListDetail.AddBuf(buf);
            buf.OnAddBuf();

            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "AddBufメソッドでは付与数0でも付与可能")]
        public void TestStack2()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 0 };
            bufListDetail.AddBuf(buf);
            buf.OnAddBuf();

            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "付与数の上限は5")]
        public void TestStack3()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 6 };
            bufListDetail.AddBuf(buf);
            buf.OnAddBuf();

            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>(), Is.True);
        }

        [Test(Description = "")]
        public void TestOnRoundStartAfter()
        {
            var buf = new BattleUnitBuf_SunnyDay() { stack = 5 };
            bufListDetail.AddBuf(buf);

            Assert.That(owner.allyCardDetail.GetHand().Count, Is.Zero);
            buf.OnRoundStartAfter();
        }
    }
}
