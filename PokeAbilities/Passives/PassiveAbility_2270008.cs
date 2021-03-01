#pragma warning disable CA1031 // Do not catch general exception types

using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ちくでん」
    /// 麻痺状態の影響を受けない。幕の終了時に「麻痺の値x5」体力が回復する。
    /// </summary>
    public class PassiveAbility_2270008 : PassiveAbilityBase
    {
        public override bool IsImmune(KeywordBuf buf)
            => buf == KeywordBuf.Paralysis || base.IsImmune(buf);

        public override void OnRoundEnd()
        {
            try
            {
                BattleUnitBuf paralysis = owner.bufListDetail.GetActivatedBuf(KeywordBuf.Paralysis);
                if (paralysis == null || paralysis.stack <= 0) { return; }

                owner.RecoverHP(paralysis.stack * 5);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
