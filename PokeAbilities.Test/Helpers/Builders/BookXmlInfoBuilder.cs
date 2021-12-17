namespace PokeAbilities.Test.Helpers.Builders
{
    /// <summary>
    /// コア ページ XML 情報のインスタンスを構築します。
    /// </summary>
    public class BookXmlInfoBuilder
    {
        /// <summary>
        /// 最大体力を取得または設定します。
        /// 既定値は 30 です。
        /// </summary>
        public int Hp { get; set; } = 30;

        /// <summary>
        /// 体力の減少量を取得または設定します。
        /// 既定値は 0 です。
        /// </summary>
        public int HpReduction { get; set; } = 0;

        /// <summary>
        /// 最大混乱耐性を取得または設定します。
        /// 既定値は 20 です。
        /// </summary>
        public int Break { get; set; } = 20;

        /// <summary>
        /// 斬撃耐性を取得または設定します。
        /// 既定値は普通です。
        /// </summary>
        public AtkResist SResist { get; set; } = AtkResist.Normal;

        /// <summary>
        /// 貫通耐性を取得または設定します。
        /// 既定値は普通です。
        /// </summary>
        public AtkResist PResist { get; set; } = AtkResist.Normal;

        /// <summary>
        /// 打撃耐性を取得または設定します。
        /// 既定値は普通です。
        /// </summary>
        public AtkResist HResist { get; set; } = AtkResist.Normal;

        /// <summary>
        /// 斬撃混乱耐性を取得または設定します。
        /// 既定値は普通です。
        /// </summary>
        public AtkResist SBResist { get; set; } = AtkResist.Normal;

        /// <summary>
        /// 貫通混乱耐性を取得または設定します。
        /// 既定値は普通です。
        /// </summary>
        public AtkResist PBResist { get; set; } = AtkResist.Normal;

        /// <summary>
        /// 打撃混乱耐性を取得または設定します。
        /// 既定値は普通です。
        /// </summary>
        public AtkResist HBResist { get; set; } = AtkResist.Normal;

        /// <summary>
        /// 速度ダイスの最小値を取得または設定します。
        /// 既定値は 1 です。
        /// </summary>
        public int SppedMin { get; set; } = 1;

        /// <summary>
        /// 速度ダイスの最大値を取得または設定します。
        /// 既定値は 4 です。
        /// </summary>
        public int Spped { get; set; } = 4;

        /// <summary>
        /// 速度ダイス数を取得または設定します。
        /// 既定値は 1 です。
        /// </summary>
        public int SpeedDiceNum { get; set; } = 1;

        /// <summary>
        /// 光の最大値を取得または設定します。
        /// 既定値は 3 です。
        /// </summary>
        public int MaxPlayPoint { get; set; } = 3;

        /// <summary>
        /// 光の開始値を取得または設定します。
        /// 既定値は 3 です。
        /// </summary>
        public int StartPlayPoint { get; set; } = 3;

        /// <summary>
        /// 使用できるバトル ページの攻撃範囲を取得または設定します。
        /// 既定値は近距離バトル ページのみです。
        /// </summary>
        public EquipRangeType RangeType { get; set; } = EquipRangeType.Melee;

        /// <summary>
        /// 現在設定さている情報から、コア ページ XML 情報のインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BookXmlInfo Build()
            => new BookXmlInfo()
            {
                _id = 0, // IDはこのテスト上で使用しない (UnitDataModelの生成直前にこのインスタンスを生成し、BookXmlListを都度初期化して登録するのでID重複を発生させない)
                EquipEffect = new BookEquipEffect()
                {
                    Hp = Hp,
                    HpReduction = HpReduction,
                    Break = Break,
                    SResist = SResist,
                    PResist = PResist,
                    HResist = HResist,
                    SBResist = SBResist,
                    PBResist = PBResist,
                    HBResist = HBResist,
                    SpeedMin = SppedMin,
                    Speed = Spped,
                    MaxPlayPoint = MaxPlayPoint,
                    StartPlayPoint = StartPlayPoint,
                },
                speedDiceNumber = SpeedDiceNum, // 速度ダイス数はBookEquipEffectではなくBookXmlInfoで指定する (BookEquipEffectではフィールドが定義されているだけで使用されない)
                RangeType = RangeType,
            };
    }
}
