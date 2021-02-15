using System.Linq;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="BattleUnitBufListDetail"/> の拡張メソッドを提供します。
    /// </summary>
    internal static class BattleUnitBufListDetailExtension
    {
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
                target.AddBuf(buf);
            }
            buf.stack += stack;
        }
    }
}
