namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「あられ」。
    /// 幕の終了時、最大体力の1/16だけダメージを受け(最大5ダメージ)、数値が1減少する(最大5)
    /// </summary>
    public class BattleUnitBuf_Hail : BattleUnitBufCustomBase
    {
        protected override string keywordId => "Hail";

        protected override string keywordIconId => "HailBuf";

        protected override int MaxStack => 5;

        /// <summary>
        /// <see cref="BattleUnitBuf_Hail"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitBuf_Hail()
            => LoadIcon();
    }
}
