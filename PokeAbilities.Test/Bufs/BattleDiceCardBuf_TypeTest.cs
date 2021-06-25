using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Test(Description = "コンストラクタで指定したタイプが Type プロパティの値になる。")]
        public void TestType()
        {
            {
                var buf = new BattleDiceCardBuf_Type(PokeType.Normal);
                Assert.That(buf.Type, Is.EqualTo(PokeType.Normal));
            }

            {
                var buf = new BattleDiceCardBuf_Type(PokeType.Dark);
                Assert.That(buf.Type, Is.EqualTo(PokeType.Dark));
            }
        }

        [Test(Description = "幕の終了時に破棄される。")]
        public void TestOnRoundEnd()
        {
            var buf = new BattleDiceCardBuf_Type(PokeType.Grass);

            Assert.That(buf.IsDestroyed(), Is.False);
            buf.OnRoundEnd();
            Assert.That(buf.IsDestroyed(), Is.True);
        }
    }
}
