using PokeAbilities.Bufs;

namespace PokeAbilities.CardAbilities
{
    /// <summary>
    /// [使用時] 全てのキャラクターににほんばれ5を付与
    /// </summary>
    public class DiceCardSelfAbility_sunny5all : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new[] { "SunnyDay_Simple" };

        public override void OnUseCard()
        {
            foreach (BattleUnitModel target in BattleObjectManager.instance.GetAliveList())
            {
                target.bufListDetail.RemoveAllWeather();
                target.bufListDetail.AddBuf<BattleUnitBuf_SunnyDay>(5);
            }
        }
    }
}
