using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test.Bufs
{
    [TestFixture]
    public class BattleUnitBuf_HailTest
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;
        private BattleAllyCardDetail allyCardDetail;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().ToBattleUnitModel();
            bufListDetail = owner.bufListDetail;
            allyCardDetail = owner.allyCardDetail;
        }

        #region stack

        [Test(Description = "インスタンス生成直後はスタック数1")]
        public void TestStack1()
        {
            var buf = new BattleUnitBuf_Hail();
            bufListDetail.AddBuf(buf);
            buf.OnAddBuf();

            Assert.That(buf.stack, Is.EqualTo(1));
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "AddBufメソッドでは付与数0でも付与可能")]
        public void TestStack2()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 0 };
            bufListDetail.AddBuf(buf);
            buf.OnAddBuf();

            Assert.That(buf.stack, Is.EqualTo(0));
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        [Test(Description = "付与数の上限は5")]
        public void TestStack3()
        {
            var buf = new BattleUnitBuf_Hail() { stack = 6 };
            bufListDetail.AddBuf(buf);
            buf.OnAddBuf();

            Assert.That(buf.stack, Is.EqualTo(5));
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Hail>(), Is.True);
        }

        #endregion

        // Note: OnRoundEndでは間接的にUnitData (UnitBattleDataModel) を参照するのでテスト不化
    }
}
