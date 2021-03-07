#pragma warning disable CA1031 // Do not catch general exception types

using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「リーフガード」
    /// にほんばれ状態のとき、状態異常に対して免疫。
    /// </summary>
    public class PassiveAbility_2270014 : PassiveAbilityBase
    {
        public override bool CanAddBuf(BattleUnitBuf buf)
        {
            try
            {
                if (!owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>())
                {
                    Log.Instance.InfomationWithCaller($"Has not sunny day buf. (owner: '{owner.UnitData.unitData.name}')");
                    return base.CanAddBuf(buf);
                }

                if (buf.positiveType != BufPositiveType.Negative)
                {
                    Log.Instance.InfomationWithCaller($"Is not negative buf. (buf: '{buf.bufActivatedName}')");
                    return base.CanAddBuf(buf);
                }

                Log.Instance.InfomationWithCaller($"Is immuned buf. (buf: '{buf.bufActivatedName}')");
                return false;
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
                return base.CanAddBuf(buf);
            }
        }
    }
}
