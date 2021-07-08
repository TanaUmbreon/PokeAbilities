using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.CardAbilities
{
    /// <summary>
    /// このページはみずタイプ固定
    /// [使用時] 全てのキャラクターにあめ5を付与
    /// </summary>
    public class DiceCardSelfAbility_rain5all : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new[] { "Rain_Simple" };

        public override void OnRoundStart_inHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            try
            {
                self.TryAddType(PokeType.Water, true);
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
                    target.bufListDetail.AddBuf<BattleUnitBuf_Rain>(5);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }
    }
}
