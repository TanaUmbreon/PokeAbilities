using System.Linq;
using PokeAbilities.Bufs;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// 戦闘で使われるオブジェクトに対する様々な情報を取得するユーティリティ クラスです。
    /// </summary>
    public static class BattleInfo
    {
        /// <summary>
        /// 指定したキャラクターの手札の枚数を取得します。
        /// </summary>
        /// <param name="target">手札の枚数を取得する対象のキャラクター。</param>
        /// <returns></returns>
        public static int GetHandCount(BattleUnitModel target)
            => target.allyCardDetail.GetHand().Count;

        /// <summary>
        /// 指定したキャラクターから、何らかのタイプが付与された手札の枚数を取得します。
        /// </summary>
        /// <param name="target">手札の枚数を取得する対象のキャラクター。</param>
        /// <returns></returns>
        public static int GetTypeHandCount(BattleUnitModel target)
            => target.allyCardDetail.GetHand().Count(
                h => h.GetBufList().OfType<BattleDiceCardBuf_Type>().Any());

        /// <summary>
        /// 指定したキャラクターから、指定したタイプが付与された手札の枚数を取得します。
        /// </summary>
        /// <param name="target">手札の枚数を取得する対象のキャラクター。</param>
        /// <param name="type">手札の枚数を取得する対象のタイプ。</param>
        /// <returns></returns>
        public static int GetTypeHandCount(BattleUnitModel target, PokeType type)
            => target.allyCardDetail.GetHand().Count(
                h => h.GetBufList().OfType<BattleDiceCardBuf_Type>().Any(b => b.Type == type));

        /// <summary>
        /// 指定したキャラクターから、何らかのタイプが 2 個以上同時に付与された手札の枚数を取得します。
        /// </summary>
        /// <param name="target">手札の枚数を取得する対象のキャラクター。</param>
        /// <returns></returns>
        public static int GetMultiTypesHandCount(BattleUnitModel target)
            => target.allyCardDetail.GetHand().Count(
                h => h.GetBufList().OfType<BattleDiceCardBuf_Type>().Count() >= 2);
    }
}
