namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「にげあし」
    /// 自分の体力が50%以下なら、幕の開始時にクイック2を得て、25%の確率で行動不能
    /// </summary>
    public class PassiveAbility_2270003 : PassiveAbilityBase
    {
        public override void OnRoundStartAfter()
        {
            if (owner.hp > (owner.MaxHp * 0.5f)) { return; }

            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2, owner);

            if (RandomUtil.valueForProb >= 0.25f) { return; }

            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Stun, 1, owner);
        }
    }
}
