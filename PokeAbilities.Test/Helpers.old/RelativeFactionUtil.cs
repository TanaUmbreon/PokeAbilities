using System;

namespace PokeAbilities.Test.Helpers
{
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
        /// <param name="baseFaction">基準となる派閥。</param>
        /// <param name="relativeFaction"><paramref name="baseFaction"/> から見た相対的な派閥。</param>
        /// <returns></returns>
        public static Faction GetFaction(Faction baseFaction, RelativeFaction relativeFaction)
        {
            switch (relativeFaction)
            {
                case RelativeFaction.Ally:
                case RelativeFaction.Self:
                    return AllySelector(baseFaction);
                case RelativeFaction.Opponent:
                    return OpponentSelector(baseFaction);
                default:
                    throw new ArgumentOutOfRangeException(nameof(relativeFaction));
            }
        }

        /// <summary>
        /// 指定したキャラクターの派閥に対する派閥を取得します。
        /// </summary>
        /// <param name="baseUnit">基準となるキャラクター。</param>
        /// <param name="relativeFaction"><paramref name="baseUnit"/> から見た相対的な派閥。</param>
        /// <returns></returns>
        public static Faction GetFaction(BattleUnitModel baseUnit, RelativeFaction relativeFaction)
            => GetFaction(baseUnit.faction, relativeFaction);
    }
}
