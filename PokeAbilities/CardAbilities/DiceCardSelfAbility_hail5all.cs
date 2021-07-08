using System;
using PokeAbilities.Bufs;

namespace PokeAbilities.CardAbilities
{
    /// <summary>
    /// このページはこおりタイプ固定
    /// [使用時] 全てのキャラクターにあられ5を付与
    /// </summary>
    public class DiceCardSelfAbility_hail5all : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new[] { "Hail" };

        public override void OnRoundStart_inHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            try
            {
                self.TryAddType(PokeType.Ice, true);
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
                    target.bufListDetail.AddBuf<BattleUnitBuf_Hail>(5);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }
    }
}
