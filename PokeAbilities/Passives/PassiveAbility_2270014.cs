using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「リーフガード」
    /// にほんばれ状態のとき、状態異常に対して免疫
    /// </summary>
    public class PassiveAbility_2270014 : PassiveAbilityBase
    {
        public override bool CanAddBuf(BattleUnitBuf buf)
        {
            try
            {
                if (!owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>())
                {
                    return base.CanAddBuf(buf);
                }

                if (buf.positiveType != BufPositiveType.Negative)
                {
                    return base.CanAddBuf(buf);
                }

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
