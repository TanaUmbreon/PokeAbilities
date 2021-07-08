using NUnit.Framework;
using PokeAbilities.Bufs;

namespace PokeAbilities.Test.Bufs
{
    [TestFixture]
    public class BattleDiceCardBuf_TypeTest
    {
        [Test]
        public void TestNew()
        {
            Assert.That(() => { new BattleDiceCardBuf_Type(PokeType.Normal); }, Throws.Nothing);
        }

        #region Type プロパティ

        [Test(Description = "コンストラクタで指定したタイプが Type プロパティの値になる。")]
        public void TestType1()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Normal);
            Assert.That(buf.Type, Is.EqualTo(PokeType.Normal));
        }

        [Test(Description = "コンストラクタで指定したタイプが Type プロパティの値になる。")]
        public void TestType2()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Dark);
            Assert.That(buf.Type, Is.EqualTo(PokeType.Dark));
        }

        #endregion

        #region IsPermanent プロパティ

        [Test(Description = "コンストラクタで指定しない場合、値は false。")]
        public void TestIsPermanent_NotSpecified()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Grass);
            Assert.That(buf.IsPermanent, Is.False);
        }

        [Test(Description = "コンストラクタで true を指定すれば値は true。")]
        public void TestIsPermanent_True()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Grass, true);
            Assert.That(buf.IsPermanent, Is.True);
        }

        [Test(Description = "コンストラクタで false を指定すれば値は true。")]
        public void TestIsPermanent_False()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Grass, false);
            Assert.That(buf.IsPermanent, Is.False);
        }

        #endregion

        #region OnRoundEnd メソッド

        [Test(Description = "永続化でない場合、幕の終了時に破棄される。")]
        public void TestOnRoundEnd_IsNotPermanent()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Grass, false);

            Assert.That(buf.IsDestroyed(), Is.False);
            buf.OnRoundEnd();
            Assert.That(buf.IsDestroyed(), Is.True);
        }

        [Test(Description = "永続化の場合、幕の終了時に破棄されない。")]
        public void TestOnRoundEnd_IsPermanent()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Grass, true);

            Assert.That(buf.IsDestroyed(), Is.False);
            buf.OnRoundEnd();
            Assert.That(buf.IsDestroyed(), Is.False);
        }

        #endregion
    }
}
