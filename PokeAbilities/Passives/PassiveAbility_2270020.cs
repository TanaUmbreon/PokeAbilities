using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「あめふらし」
    /// 舞台の開始時、全てのキャラクターにあめ5を付与。
    /// </summary>
    public class PassiveAbility_2270020 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            foreach (BattleUnitModel target in BattleObjectManager.instance.GetAliveList())
            {
                target.bufListDetail.RemoveAllWeather();
                target.bufListDetail.AddBuf<BattleUnitBuf_Rain>(5);
            }
        }
    }
}
