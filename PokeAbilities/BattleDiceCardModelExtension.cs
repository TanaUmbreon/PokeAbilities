using System.Collections.Generic;
using System.Linq;
using PokeAbilities.Bufs;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="BattleDiceCardModel"/> の拡張メソッドを提供します。
    /// </summary>
    public static class BattleDiceCardModelExtension
    {
        /// <summary>
        /// 指定したタイプがバトル ページ状態として付与されている事を判定します。
        /// </summary>
        /// <param name="target">判定する対象のバトル ページ。</param>
        /// <param name="type">判定するタイプ。</param>
        /// <returns>指定したタイプがバトル ページ状態として付与されている場合は true、そうでない場合は false を返します。</returns>
        public static bool HasType(this BattleDiceCardModel target, PokeType type)
            => target.GetBufList().OfType<BattleDiceCardBuf_Type>().Any(b => b.Type == type);

        /// <summary>
        /// 指定したタイプのコレクションに含まれるいずれかのタイプがバトル ページ状態として付与されている事を判定します。
        /// </summary>
        /// <param name="target">判定する対象のバトル ページ。</param>
        /// <param name="types">判定するタイプのコレクション。</param>
        /// <returns>いずれかのタイプがバトル ページ状態として付与されている場合は true、そうでない場合は false を返します。</returns>
        public static bool HasType(this BattleDiceCardModel target, IEnumerable<PokeType> types)
            =>target.GetBufList().OfType<BattleDiceCardBuf_Type>().Any(b => types.Contains(b.Type));
    }
}
