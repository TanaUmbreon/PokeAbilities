using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.CardAbilities
{
    /// <summary>
    /// このページはほのおタイプ固定
    /// [使用時] 全てのキャラクターににほんばれ5を付与
    /// </summary>
    public class DiceCardSelfAbility_sunny5all : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new[] { "SunnyDay_Simple" };

        public override void OnRoundStart_inHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            try
            {
                self.TryAddType(PokeType.Fire, true);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        public override void OnUseCard()
        {
            try
            {
                foreach (BattleUnitModel target in BattleObjectManager.instance.GetAliveList())
                {
                    target.bufListDetail.RemoveAllWeather();
                    target.bufListDetail.AddBuf<BattleUnitBuf_SunnyDay>(5);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }
    }
}
