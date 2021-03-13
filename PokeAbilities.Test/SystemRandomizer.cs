using System;
using System.Collections.Generic;

namespace PokeAbilities.Test
{
    /// <summary>
    /// <see cref="Random"/> を使用した疑似乱数ジェネレーターです。
    /// Unity の疑似乱数ジェネレーターへの参照を避けるために、ユニット テスト上で使用します。
    /// </summary>
    public class SystemRandomizer : IRandomizer
    {
        private readonly Random random;

        /// <summary>
        /// <see cref="SystemRandomizer"/> の新しいインスタンスを生成します。
        /// </summary>
        public SystemRandomizer()
            => random = new Random();

        public float ValueForProb => random.Next(1000000) / 1000000f;

        public T SelectOne<T>(List<T> list) => list[random.Next(list.Count)];
    }
}
