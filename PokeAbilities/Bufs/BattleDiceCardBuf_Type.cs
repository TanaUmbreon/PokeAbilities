namespace PokeAbilities.Bufs
{
    /// <summary>
    /// バトル ページ状態「○○タイプ」
    /// ○○タイプを持つキャラクターが使用したとき、このページで与えるダメージ量+1
    /// </summary>
    public class BattleDiceCardBuf_Type : BattleDiceCardBufCustomBase
    {
        /// <summary>
        /// タイプを取得します。
        /// </summary>
        public PokeType Type { get; private set; }

        /// <summary>
        /// 幕をまたいでも永続的に状態が付与される事を示す値を取得します。
        /// </summary>
        public bool IsPermanent { get; private set; }

        protected override string keywordId => _keywordId;
        private readonly string _keywordId;

        protected override string keywordIconId => _keywordIconId;
        private readonly string _keywordIconId;

        /// <summary>
        /// <see cref="BattleDiceCardBuf_Type"/> の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="type">付与するタイプ。</param>
        /// <param name="isPermanent">幕をまたいでも永続的に状態が付与される事を示す値。省略時はその幕限り。</param>
        public BattleDiceCardBuf_Type(PokeType type, bool isPermanent = false)
        {
            Type = type;
            IsPermanent = isPermanent;

            _keywordId = ToKeywordId(type);
            _keywordIconId = ToKeywordId(type) + "Buf";
            LoadIcon();
        }

        /// <summary>
        /// 指定されたタイプを <see cref="keywordId"/> 用の文字列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ToKeywordId(PokeType value)
        {
            switch (value)
            {
                case PokeType.Normal:
                    return "NormalType";
                case PokeType.Fire:
                    return "FireType";
                case PokeType.Water:
                    return "WaterType";
                case PokeType.Electric:
                    return "ElectricType";
                case PokeType.Grass:
                    return "GrassType";
                case PokeType.Ice:
                    return "IceType";
                case PokeType.Fighting:
                    return "FightingType";
                case PokeType.Poison:
                    return "PoisonType";
                case PokeType.Ground:
                    return "GroundType";
                case PokeType.Flying:
                    return "FlyingType";
                case PokeType.Psychic:
                    return "PsychicType";
                case PokeType.Bug:
                    return "BugType";
                case PokeType.Rock:
                    return "RockType";
                case PokeType.Ghost:
                    return "GhostType";
                case PokeType.Dragon:
                    return "DragonType";
                case PokeType.Dark:
                    return "DarkType";
                case PokeType.Steel:
                    return "SteelType";
                case PokeType.Fairy:
                    return "FairyType";
                default:
                    return "Type";
            }
        }

        public override void OnRoundEnd()
        {
            if (IsPermanent) { return; }

            Destroy();
        }
    }
}
