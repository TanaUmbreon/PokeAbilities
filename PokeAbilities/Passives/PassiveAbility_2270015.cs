#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Collections.Generic;
using System.Linq;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ようりょくそ」
    /// にほんばれ状態のとき、幕の開始時にクイック2を得る。
    /// </summary>
    public class PassiveAbility_2270015 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            try
            {
                if (!owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>())
                {
                    Log.Instance.InfomationWithCaller($"Has not sunny day buf. (owner: '{owner.UnitData.unitData.name}')");
                    return;
                }

                Log.Instance.InfomationWithCaller($"Has sunny day buf. Added quickness2 buf. (owner: '{owner.UnitData.unitData.name}')");
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2, owner);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
