#pragma warning disable CA1031 // Do not catch general exception types

using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「もらいび」。
    /// 幕の終了時に火傷状態なら、火傷ダメージを受けず、50%の確率で攻撃ダイスの威力が1増加する「もらいび」状態になる。
    /// </summary>
    public class PassiveAbility_2270006 : PassiveAbilityBase
    {

        public override void OnRoundEnd()
        {
            try
            {
                if (!owner.bufListDetail.ExistsKeywordBuf(KeywordBuf.Burn)) { return; }
                if (owner.bufListDetail.HasBuf<BattleUnitBuf_FlashFire>()) { return; }

                owner.bufListDetail.AddBuf<BattleUnitBuf_FlashFire>();
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(this, nameof(OnRoundEnd), "Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
        }
    }
}
