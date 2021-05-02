using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「あめふらし」
    /// 舞台の開始時、全てのキャラクターにあめ5を付与。
    /// </summary>
    public class PassiveAbility_2270020 : PassiveAbilityBase
    {
        // Note: OnStartBattle メソッドでバフを付与した場合、最初の幕の戦闘を開始するまで表示されない。

        /// <summary>この舞台中に効果が発揮したことを示すフラグ</summary>
        private bool activated = false;
        
        public override void OnRoundStart()
        {
            if (activated) { return; }
        
            foreach (BattleUnitModel target in BattleObjectManager.instance.GetAliveList())
            {
                target.bufListDetail.RemoveAllWeather();
                target.bufListDetail.AddBuf<BattleUnitBuf_Rain>(5);
            }
        
            activated = true;
        }
    }
}
