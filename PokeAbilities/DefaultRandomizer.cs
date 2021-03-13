using System.Collections.Generic;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="RandomUtil"/> を使用した疑似乱数ジェネレーターです。
    /// </summary>
    public sealed class DefaultRandomizer : IRandomizer
    {
        #region Singleton 実装

        /// <summary>
        /// このクラスの既定のインスタンスを取得します。
        /// </summary>
        public static DefaultRandomizer Instance => new DefaultRandomizer();

        /// <summary>
        /// <see cref="DefaultRandomizer"/> の新しいインスタンスを生成します。
        /// </summary>
        private DefaultRandomizer() { }

        #endregion

        public float ValueForProb => RandomUtil.valueForProb;

        public T SelectOne<T>(List<T> list) => RandomUtil.SelectOne(list);
    }
}
