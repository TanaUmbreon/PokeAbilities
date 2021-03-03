namespace PokeAbilities.Bufs
{
    /// <summary>
    /// バトル ページ状態「ほのおタイプ」
    /// にほんばれ状態のとき、このページで与えるダメージ量+1
    /// </summary>
    public class BattleDiceCardBuf_FireType : BattleDiceCardBufCustomBase
    {
        protected override string keywordId => "FireType";

        protected override string keywordIconId => "FireTypeBuf";

        /// <summary>
        /// <see cref="BattleDiceCardBuf_FireType"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleDiceCardBuf_FireType()
            => LoadIcon();

        public override void OnRoundEnd() 
            => Destroy();
    }
}
