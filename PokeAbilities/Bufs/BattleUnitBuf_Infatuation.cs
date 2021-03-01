namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「メロメロ」。
    /// 幕の開始時、50%の確率で行動不能状態になる。
    /// この状態を付与したキャラクターが死亡した時、メロメロ状態が解除される
    /// </summary>
    public class BattleUnitBuf_Infatuation : BattleUnitBufCustomBase
    {
        protected override string keywordId => "Infatuation";

        protected override string keywordIconId => "InfatuationBuf";

        protected override int MaxStack => 0;

        /// <summary>この状態を付与したキャラクター</summary>
        private readonly BattleUnitModel target;

        /// <summary>
        /// <see cref="BattleUnitBuf_Infatuation"/> の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="target">この状態を付与したキャラクター。</param>
        public BattleUnitBuf_Infatuation(BattleUnitModel target)
        {
            this.target = target;
            LoadIcon();
        }

        public override void OnRoundStart()
        {
            if (RandomUtil.valueForProb >= 0.5f) { return; }

            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Stun, 0);
        }

        public override void OnRoundEndTheLast()
        {
            if (target.IsDead())
            {
                Destroy();
            }
        }
    }
}
