using System;
using System.Linq;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="BattleUnitBufListDetail"/> の拡張メソッドを提供します。
    /// </summary>
    internal static class BattleUnitBufListDetailExtension
    {
        /// <summary>
        /// 指定したポジティブ タイプが、この幕に適用されているバフに有効な状態で存在している事を判定します。
        /// </summary>
        /// <param name="target">判定する対象キャラクターのバフ一覧。</param>
        /// <param name="type">判定するポジティブ タイプ。</param>
        /// <returns>指定したポジティブ タイプが有効な状態で存在している場合は true、存在していない場合は false を返します。</returns>
        public static bool ExistsPositiveType(this BattleUnitBufListDetail target, BufPositiveType type)
            => target.GetActivatedBufList().Any(b => b.positiveType == type && !b.IsDestroyed());

        /// <summary>
        /// 指定したバフの種類が、この幕に適用されているバフに有効な状態で存在している事を判定します。
        /// </summary>
        /// <param name="target">判定する対象キャラクターのバフ一覧。</param>
        /// <param name="type">判定するバフの種類。</param>
        /// <returns>指定したバフの種類が有効な状態で存在している場合は true、存在していない場合は false を返します。</returns>
        public static bool ExistsKeywordBuf(this BattleUnitBufListDetail target, KeywordBuf buf)
            => target.GetActivatedBufList().Any(b => b.bufType == buf && !b.IsDestroyed());

        /// <summary>
        /// 指定した型のバフをこの幕から付与します。
        /// </summary>
        /// <typeparam name="T">付与するバフの型。</typeparam>
        /// <param name="target">バフを付与する対象キャラクターのバフ一覧。</param>
        /// <param name="stack">付与するバフのスタック数。</param>
        public static void AddBuf<T>(this BattleUnitBufListDetail target, int stack) where T : BattleUnitBuf, new()
        {
            BattleUnitBuf buf = target.GetActivatedBufList().FirstOrDefault(b => b is T);
            if (buf == null)
            {
                buf = new T() { stack = 0 };
                target.AddBuf(buf);
            }
            buf.stack += stack;
            buf.OnAddBuf();
        }

        /// <summary>
        /// 指定した型のバフを次の幕から付与します。
        /// </summary>
        /// <typeparam name="T">付与するバフの型。</typeparam>
        /// <param name="target">バフを付与する対象キャラクターのバフ一覧。</param>
        /// <param name="stack">付与するバフのスタック数。</param>
        public static void AddReadyBuf<T>(this BattleUnitBufListDetail target, int stack) where T : BattleUnitBuf, new()
        {
            BattleUnitBuf buf = target.GetReadyBufList().FirstOrDefault(b => b is T);
            if (buf == null)
            {
                buf = new T() { stack = 0 };
                target.AddReadyBuf(buf);
            }
            buf.stack += stack;
            buf.OnAddBuf();
        }
    }
}
