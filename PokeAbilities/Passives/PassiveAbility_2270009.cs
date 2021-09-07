using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「はやあし」
    /// 束縛状態の影響を受けない。幕の終了時に状態異常なら、次の幕にクイック1を得る
    /// </summary>
    public class PassiveAbility_2270009 : PassiveAbilityBase
    {
        public override bool IsImmune(KeywordBuf buf)
            => buf == KeywordBuf.Binding || base.IsImmune(buf);

        public override void OnRoundEnd()
        {
            try
            {
                if (!owner.bufListDetail.HasBuf(BufPositiveType.Negative)) { return; }
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 1, owner);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
