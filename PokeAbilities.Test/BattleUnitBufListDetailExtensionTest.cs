using System.Linq;
using NUnit.Framework;
using PokeAbilities.Bufs;
using PokeAbilities.Test.Helpers;

namespace PokeAbilities.Test
{
    [TestFixture]
    public class BattleUnitBufListDetailExtensionTest
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;

        [SetUp]
        public void SetUp()
        {
            owner = new BattleUnitModelBuilder().Build();
            bufListDetail = owner.bufListDetail;
        }

        [Test]
        public void TestRemoveAllWeather()
        {
            bufListDetail.AddBuf(new BattleUnitBuf_Rain() { stack = 1 });
            bufListDetail.AddBuf(new BattleUnitBuf_SunnyDay() { stack = 1 });
            bufListDetail.AddBuf(new BattleUnitBuf_Hail() { stack = 1 });
            bufListDetail.AddBuf(new BattleUnitBuf_strength() { stack = 1 });
            bufListDetail.AddReadyBuf(new BattleUnitBuf_Rain() { stack = 1 });
            bufListDetail.AddReadyBuf(new BattleUnitBuf_SunnyDay() { stack = 1 });
            bufListDetail.AddReadyBuf(new BattleUnitBuf_Hail() { stack = 1 });
            bufListDetail.AddReadyBuf(new BattleUnitBuf_strength() { stack = 1 });
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_Rain>().Any(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_SunnyDay>().Any(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_Hail>().Any(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_strength>().Any(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(4));
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_Rain>().Any(), Is.True);
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_SunnyDay>().Any(), Is.True);
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_Hail>().Any(), Is.True);
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_strength>().Any(), Is.True);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(4));

            bufListDetail.RemoveAllWeather();
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_Rain>().Any(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_SunnyDay>().Any(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_Hail>().Any(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().OfType<BattleUnitBuf_strength>().Any(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_Rain>().Any(), Is.False);
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_SunnyDay>().Any(), Is.False);
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_Hail>().Any(), Is.False);
            Assert.That(bufListDetail.GetReadyBufList().OfType<BattleUnitBuf_strength>().Any(), Is.True);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class BattleUnitBufListDetailExtensionTest_HasBuf1
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;

        [SetUp]
        public void InitializeFields()
        {
            owner = new BattleUnitModelBuilder().Build();
            bufListDetail = owner.bufListDetail;
        }

        [Test(Description = "初期状態では全てfalseが返る")]
        public void TestHasBuf_BufPositiveType1()
        {
            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.False);
        }

        [Test(Description = "この幕にPositiveなバフが適用されていると、それのみtrueが返る")]
        public void TestHasBuf_BufPositiveType2()
        {
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.False);
        }

        [Test(Description = "この幕にNegativeなバフが適用されていると、それのみtrueが返る")]
        public void TestHasBuf_BufPositiveType3()
        {
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.True);
        }

        [Test(Description = "この幕にNoneなバフが適用されていると、それのみtrueが返る")]
        public void TestHasBuf_BufPositiveType4()
        {
            bufListDetail.AddBuf(new BattleUnitBuf_smoke() { stack = 1 });
            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.False);
        }

        [Test(Description = "この幕に全BufPositiveTypeが適用されていると、全てtrueが返る")]
        public void TestHasBuf_BufPositiveType5()
        {
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            bufListDetail.AddBuf(new BattleUnitBuf_smoke() { stack = 1 });
            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.True);
        }

        [Test(Description = "次の幕に全BufPositiveTypeが適用されていても、falseのまま")]
        public void TestHasBuf_BufPositiveType6()
        {
            bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1);
            bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1);
            bufListDetail.AddReadyBuf(new BattleUnitBuf_smoke() { stack = 1 });
            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.False);
        }

        [Test(Description = "この幕に全BufPositiveTypeが適用されているが、Noneのみ破棄状態だとそれだけfalseが返る")]
        public void TestHasBuf_BufPositiveType7()
        {
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            var noneBuf = new BattleUnitBuf_smoke() { stack = 1 };
            bufListDetail.AddBuf(noneBuf);

            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.True);

            noneBuf.Destroy();

            Assert.That(bufListDetail.HasBuf(BufPositiveType.None), Is.False);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Positive), Is.True);
            Assert.That(bufListDetail.HasBuf(BufPositiveType.Negative), Is.True);
        }
    }

    [TestFixture]
    public class BattleUnitBufListDetailExtensionTest_HasBuf2
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;

        [SetUp]
        public void InitializeFields()
        {
            owner = new BattleUnitModelBuilder().Build();
            bufListDetail = owner.bufListDetail;
        }

        [Test(Description = "初期状態ではfalseが返る")]
        public void TestHasBuf_KeywordBuf1()
        {
            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.False);
        }

        [Test(Description = "この幕に特定のKeywordBufが適用されていると、それに対してtrueが返る")]
        public void TestHasBuf_KeywordBuf2()
        {
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.False);

            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);

            bufListDetail.AddBuf(new BattleUnitBuf_theCryingWeak());
            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);
        }

        [Test(Description = "次の幕に特定のKeywordBufが適用されていても、falseのまま")]
        public void TestHasBuf_KeywordBuf3()
        {
            bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 1);
            bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1);
            bufListDetail.AddReadyBuf(new BattleUnitBuf_theCryingWeak());
            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.False);
        }

        [Test(Description = "この幕に特定のKeywordBufが適用されているが、破棄状態だとそれだけfalseが返る")]
        public void TestHasBuf_KeywordBuf4()
        {
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
            bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            var noneBuf = new BattleUnitBuf_theCryingWeak() { stack = 1 };
            bufListDetail.AddBuf(noneBuf);

            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);

            noneBuf.Destroy();

            Assert.That(bufListDetail.HasBuf(KeywordBuf.None), Is.False);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Quickness), Is.True);
            Assert.That(bufListDetail.HasBuf(KeywordBuf.Weak), Is.True);
        }
    }

    [TestFixture]
    public class BattleUnitBufListDetailExtensionTest_AddBuf
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;

        [SetUp]
        public void InitializeFields()
        {
            owner = new BattleUnitModelBuilder().Build();
            bufListDetail = owner.bufListDetail;
        }

        [Test(Description = "付与数が0以下の場合は付与できない")]
        public void AddBufTest1()
        {
            bufListDetail.AddBuf<BattleUnitBuf_Decay>(0);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);

            bufListDetail.AddBuf<BattleUnitBuf_Decay>(-1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "付与不可能バフがこの幕に付与されている場合は付与できない")]
        public void AddBufTest2()
        {
            bufListDetail.AddBuf(new BattleUnitBuf_CanNotAddDecay());
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);

            bufListDetail.AddBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "付与不可能バフが次の幕に付与されている場合は付与できる")]
        public void AddBufTest3()
        {
            bufListDetail.AddReadyBuf(new BattleUnitBuf_CanNotAddDecay());
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(0));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));

            bufListDetail.AddBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "付与不可能パッシブを所有している場合は付与できない")]
        public void AddBufTest5()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_CanNotAddDecay());
            owner.passiveDetail.OnCreated(); // 追加したパッシブを有効化

            bufListDetail.AddBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.False);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "付与可能/効果無効化パッシブを所有している場合は付与できる")]
        public void AddBufTest6()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_CanAddDecayButDisabled());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_Decay>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddBufTest7_1()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_vulnerable>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_vulnerable>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddBufTest7_2()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_vulnerable>(2);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_vulnerable>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddBufTest7_3()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_vulnerable>(3);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_vulnerable>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddBufTest7_4()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_vulnerable>(4);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_vulnerable>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(2));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddBufTest7_5()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_vulnerable>(5);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_vulnerable>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(2));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddBufTest7_6()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddBuf<BattleUnitBuf_vulnerable>(6);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_vulnerable>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(3));
        }

        [Test(Description = "付与数上限を超えて付与すると上限値に補正される")]
        public void AddBufTest8()
        {
            bufListDetail.AddBuf<BattleUnitBuf_MaxStack0>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_MaxStack0>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().FirstOrDefault(b => b is BattleUnitBuf_MaxStack0).stack, Is.EqualTo(0));

            bufListDetail.AddBuf<BattleUnitBuf_MaxStack5>(6);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_MaxStack5>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBufList().FirstOrDefault(b => b is BattleUnitBuf_MaxStack5).stack, Is.EqualTo(5));
        }

        [Test(Description = "複数回付与すると付与数が加算される。")]
        public void AddBufTest9()
        {
            bufListDetail.AddBuf<BattleUnitBuf_burn>(1);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Burn).stack, Is.EqualTo(1));

            bufListDetail.AddBuf<BattleUnitBuf_burn>(9);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Burn).stack, Is.EqualTo(10));

            bufListDetail.AddBuf<BattleUnitBuf_burn>(7);
            Assert.That(bufListDetail.HasBuf<BattleUnitBuf_burn>(), Is.True);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Burn).stack, Is.EqualTo(17));
        }
    }

    [TestFixture]
    public class BattleUnitBufListDetailExtensionTest_AddReadyBuf
    {
        private BattleUnitModel owner;
        private BattleUnitBufListDetail bufListDetail;

        [SetUp]
        public void InitializeFields()
        {
            owner = new BattleUnitModelBuilder().Build();
            bufListDetail = owner.bufListDetail;
        }

        [Test(Description = "付与数が0以下の場合は付与できない")]
        public void AddReadyBufTest1()
        {
            bufListDetail.AddReadyBuf<BattleUnitBuf_Decay>(0);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);

            bufListDetail.AddReadyBuf<BattleUnitBuf_Decay>(-1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "付与不可能バフがこの幕に付与されている場合は付与できない")]
        public void AddReadyBufTest2()
        {
            bufListDetail.AddBuf(new BattleUnitBuf_CanNotAddDecay());
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);

            bufListDetail.AddReadyBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "付与不可能バフが次の幕に付与されている場合は付与できる")]
        public void AddReadyBufTest3()
        {
            bufListDetail.AddReadyBuf(new BattleUnitBuf_CanNotAddDecay());
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));

            bufListDetail.AddReadyBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Not.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(2));
        }

        [Test(Description = "付与不可能パッシブを所有している場合は付与できない")]
        public void AddReadyBufTest5()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_CanNotAddDecay());
            owner.passiveDetail.OnCreated(); // 追加したパッシブを有効化

            bufListDetail.AddReadyBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }

        [Test(Description = "付与可能/効果無効化パッシブを所有している場合は付与できる")]
        public void AddReadyBufTest6()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_CanAddDecayButDisabled());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_Decay>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Decay), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Decay), Is.Not.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddReadyBufTest7_1()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_vulnerable>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(1));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddReadyBufTest7_2()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_vulnerable>(2);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(1));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddReadyBufTest7_3()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_vulnerable>(3);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(1));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddReadyBufTest7_4()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_vulnerable>(4);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(2));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddReadyBufTest7_5()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_vulnerable>(5);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(2));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "パッシブ「覇下」を所有している場合は火傷以外の状態異常は付与数半減")]
        public void AddReadyBufTest7_6()
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_250336());
            owner.passiveDetail.OnCreated();

            bufListDetail.AddReadyBuf<BattleUnitBuf_vulnerable>(6);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Vulnerable).stack, Is.EqualTo(3));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "付与数上限を超えて付与すると上限値に補正される")]
        public void AddReadyBufTest8()
        {
            bufListDetail.AddReadyBuf<BattleUnitBuf_MaxStack0>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBufList().FirstOrDefault(b => b is BattleUnitBuf_MaxStack0).stack, Is.EqualTo(0));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));

            bufListDetail.AddReadyBuf<BattleUnitBuf_MaxStack5>(6);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Vulnerable), Is.Null);
            Assert.That(bufListDetail.GetReadyBufList().FirstOrDefault(b => b is BattleUnitBuf_MaxStack5).stack, Is.EqualTo(5));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(2));
        }

        [Test(Description = "複数回付与すると付与数が加算される。")]
        public void AddReadyBufTest9()
        {
            bufListDetail.AddReadyBuf<BattleUnitBuf_paralysis>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Paralysis), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Paralysis).stack, Is.EqualTo(1));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));

            bufListDetail.AddReadyBuf<BattleUnitBuf_paralysis>(9);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Paralysis), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Paralysis).stack, Is.EqualTo(10));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));

            bufListDetail.AddReadyBuf<BattleUnitBuf_paralysis>(7);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Paralysis), Is.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Paralysis).stack, Is.EqualTo(17));
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.Zero);
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.EqualTo(1));
        }

        [Test(Description = "次の幕に火傷を付与しようとするとこの幕に付与される。")]
        public void AddReadyBufTest10()
        {
            bufListDetail.AddReadyBuf<BattleUnitBuf_burn>(1);
            Assert.That(bufListDetail.GetActivatedBuf(KeywordBuf.Burn), Is.Not.Null);
            Assert.That(bufListDetail.GetReadyBuf(KeywordBuf.Burn), Is.Null);
            Assert.That(bufListDetail.GetActivatedBufList().Count, Is.EqualTo(1));
            Assert.That(bufListDetail.GetReadyBufList().Count, Is.Zero);
        }
    }

    #region Mockオブジェクト

    /// <summary>
    /// 付与数上限0のバフ。
    /// </summary>
    internal class BattleUnitBuf_Negative : BattleUnitBuf
    {
        public override BufPositiveType positiveType => BufPositiveType.Negative;
    }

    internal class BattleUnitBuf_MaxStack0 : BattleUnitBuf
    {
        public override void OnAddBuf(int addedStack)
        {
            if (stack > 0)
            {
                stack = 0;
            }
        }
    }

    /// <summary>
    /// 付与数上限5のバフ。
    /// </summary>
    internal class BattleUnitBuf_MaxStack5 : BattleUnitBuf
    {
        public override void OnAddBuf(int addedStack)
        {
            if (stack > 5)
            {
                stack = 5;
            }
        }
    }

    /// <summary>
    /// 腐食を付与できないバフ。
    /// </summary>
    internal class BattleUnitBuf_CanNotAddDecay : BattleUnitBuf
    {
        public override bool IsImmune(BattleUnitBuf buf)
            => (buf.bufType == KeywordBuf.Decay) || base.IsImmune(buf);
    }

    /// <summary>
    /// 腐食を付与できないパッシブ。
    /// </summary>
    internal class PassiveAbility_CanNotAddDecay : PassiveAbilityBase
    {
        public override bool CanAddBuf(BattleUnitBuf buf)
            => (buf.bufType != KeywordBuf.Decay) && base.CanAddBuf(buf);
    }

    /// <summary>
    /// 腐食を付与できるが効果を発揮しないパッシブ。
    /// </summary>
    internal class PassiveAbility_CanAddDecayButDisabled : PassiveAbilityBase
    {
        public override bool IsImmune(KeywordBuf buf)
            => (buf == KeywordBuf.Decay) || base.IsImmune(buf);
    }

    #endregion
}
