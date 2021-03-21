using PokeAbilities.Bufs;

namespace PokeAbilities.CardAbilities
{
    /// <summary>
    /// [使用時] 全てのキャラクターにあられ5を付与
    /// </summary>
    public class DiceCardSelfAbility_hail5all : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new[] { "Hail" };

        public override void OnUseCard()
        {
            foreach (BattleUnitModel target in BattleObjectManager.instance.GetAliveList())
            {
                target.bufListDetail.RemoveAllWeather();
                target.bufListDetail.AddBuf<BattleUnitBuf_Hail>(5);
            }
        }
    }
}
