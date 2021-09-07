using PokeAbilities.Bufs;
using UnityEngine;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「あめうけざら」
    /// あめ状態のとき、最大体力の1/16だけ体力が回復する(最大5)
    /// </summary>
    public class PassiveAbility_2270019 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            if (!owner.bufListDetail.HasBuf<BattleUnitBuf_Rain>()) { return; }

            const int MinRecover = 1;
            const int MaxRecover = 5;

            int num = owner.MaxHp / 16;
            num = Mathf.Clamp(num, MinRecover, MaxRecover);
            owner.RecoverHP(num);
        }
    }
}
