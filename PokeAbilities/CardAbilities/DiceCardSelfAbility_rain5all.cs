using PokeAbilities.Bufs;

namespace PokeAbilities.CardAbilities
{
    /// <summary>
    /// [使用時] 全てのキャラクターにあめ5を付与
    /// </summary>
    public class DiceCardSelfAbility_rain5all : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new[] { "Rain_Simple" };

        public override void OnUseCard()
        {
            foreach (BattleUnitModel target in BattleObjectManager.instance.GetAliveList())
            {
                target.bufListDetail.AddBuf<BattleUnitBuf_Rain>(5);
            }
        }
    }
}
