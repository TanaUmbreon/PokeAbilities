﻿using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ようりょくそ」
    /// にほんばれ状態のとき、幕の開始時にクイック2を得る
    /// </summary>
    public class PassiveAbility_2270015 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            try
            {
                if (!owner.bufListDetail.HasBuf<BattleUnitBuf_SunnyDay>()) { return; }

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
