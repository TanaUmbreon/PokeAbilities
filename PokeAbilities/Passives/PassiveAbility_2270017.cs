using PokeAbilities.Bufs;
using UnityEngine;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「アイスボディ」
    /// あられ状態のとき、幕の終了時にあられダメージを受けず、最大体力の1/16だけ体力が回復する(最大5)
    /// </summary>
    public class PassiveAbility_2270017 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            if (!owner.bufListDetail.HasBuf<BattleUnitBuf_Hail>()) { return; }

            const int MinRecover = 1;
            const int MaxRecover = 5;

            int num = owner.MaxHp / 16;
            num = Mathf.Clamp(num, MinRecover, MaxRecover);
            owner.RecoverHP(num);
        }
    }
}
