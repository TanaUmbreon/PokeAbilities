using System;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// 敵またはプレイヤーのどちらか任意の派閥を基準とした相対的な派閥を表します。
    /// </summary>
    public enum RelativeFaction
    {
        /// <summary>味方</summary>
        Ally,
        /// <summary>相手</summary>
        Opponent,
    }

    /// <summary>
    /// <see cref="RelativeFaction"/> を使用したユーティリティ クラスです。
    /// </summary>
    public static class RelativeFactionUtil
    {
        /// <summary>
        /// 味方の派閥を選択するメソッドを取得します。
        /// </summary>
        private static Func<Faction, Faction> AllySelector => (f => f);

        /// <summary>
        /// 相手の派閥を選択するメソッドを取得します。
        /// </summary>
        private static Func<Faction, Faction> OpponentSelector => (f => f == Faction.Enemy ? Faction.Player : Faction.Enemy);

        /// <summary>
        /// 指定した派閥に対する派閥を取得します。
        /// </summary>
        /// <param name="baseFaction"></param>
        /// <param name="relativeFaction"></param>
        /// <returns></returns>
        public static Faction GetFaction(Faction baseFaction, RelativeFaction relativeFaction)
            => relativeFaction == RelativeFaction.Ally ? AllySelector(baseFaction) : OpponentSelector(baseFaction);

        /// <summary>
        /// 指定したキャラクターの派閥に対する派閥を取得します。
        /// </summary>
        /// <param name="baseUnit"></param>
        /// <param name="relativeFaction"></param>
        /// <returns></returns>
        public static Faction GetFaction(BattleUnitModel baseUnit, RelativeFaction relativeFaction)
            => GetFaction(baseUnit.faction, relativeFaction);
    }
}
