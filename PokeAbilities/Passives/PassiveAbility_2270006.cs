using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「もらいび
    /// ほのおタイプ付きページの攻撃で受けるダメージ・混乱ダメージ量-2。火傷ダメージを受けない。
    /// また、ほのおタイプ付きページの攻撃を受けるか火傷が付与されたとき、今回の舞台の間、ほのおタイプ付きページの攻撃ダイスの威力が1増加する「もらいび」状態になる
    /// </summary>
    public class PassiveAbility_2270006 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            try
            {
                if (!owner.bufListDetail.HasBuf(KeywordBuf.Burn)) { return; }
                if (owner.bufListDetail.HasBuf<BattleUnitBuf_FlashFire>()) { return; }

                owner.bufListDetail.AddBuf<BattleUnitBuf_FlashFire>();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
