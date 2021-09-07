using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ちくでん」
    /// でんきタイプ付きページの攻撃で受けるダメージ・混乱ダメージ量-2。麻痺状態の影響を受けない。
    /// また、でんきタイプ付きバトルページの攻撃を受けるか麻痺が付与されたとき、充電1を得る
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
