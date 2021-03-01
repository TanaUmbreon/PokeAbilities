namespace PokeAbilities.Bufs
{
    /// <summary>
    /// バトル ページ状態「みずタイプ」
    /// あめ状態のとき、このページで与えるダメージ量+1
    /// </summary>
    public class BattleDiceCardBuf_WaterType : BattleDiceCardBufCustomBase
    {
        protected override string keywordId => "WaterType";

        protected override string keywordIconId => "WaterTypeBuf";

        /// <summary>
        /// <see cref="BattleDiceCardBuf_WaterType"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleDiceCardBuf_WaterType()
            => LoadIcon();

        public override void OnRoundEnd() 
            => Destroy();
    }
}
