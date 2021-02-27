using System;
using System.Collections.Generic;
using System.Linq;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「うるおいボディ」
    /// あめ状態のとき、幕の終了時に全ての状態異常を解除する。
    /// </summary>
    public class PassiveAbility_2270011 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            if (!owner.bufListDetail.HasBuf<BattleUnitBuf_Rain>()) { return; }
            owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
        }
    }
}
