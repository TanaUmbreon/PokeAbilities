using System.Collections.Generic;

namespace PokeAbilities
{
    /// <summary>
    /// 疑似乱数ジェネレーターを実装します。
    /// このインタフェースはユニット テストをサポートする為に用いられます。
    /// </summary>
    public interface IRandomizer
    {
        /// <summary>
        /// 0 以上 1 未満のランダムな実数 (単精度浮動小数点数) を取得します。
        /// </summary>
        float ValueForProb { get; }

        /// <summary>
        /// 指定したリストからランダムに要素を一つ選択して返します。
        /// </summary>
        /// <typeparam name="T">選択対象となるリスト。</typeparam>
        /// <param name="list">リストの要素の型。</param>
        /// <returns><paramref name="list"/> に含まれている要素。</returns>
        T SelectOne<T>(List<T> list);
    }
}
