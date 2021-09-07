namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「びびり」
    /// 1幕で虚弱状態を受けるたびに今回の舞台の間、幕の開始時にクイック1を得る(最大6)
    /// </summary>
    public class PassiveAbility_2270022 : PassiveAbilityBase
    {
        /// <summary>このパッシブで付与できるクイックの最大スタック数</summary>
        private const int MaxStack = 6;

        /// <summary>このパッシブで毎幕付与するクイックのスタック数</summary>
        private int stack = 0;

        public override void OnRoundEnd()
        {
            if (!owner.bufListDetail.HasBuf(KeywordBuf.Weak)) { return; }
            if (stack >= MaxStack) { return; }

            stack++;
        }

        public override void OnRoundStart()
        {
            if (stack <= 0) { return; }

            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, stack, owner);
        }
    }
}
