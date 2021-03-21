using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// 固定値を返す疑似乱数ジェネレーターです。
    /// Unity の疑似乱数ジェネレーターへの参照を避けるために、ユニット テスト上で使用します。
    /// </summary>
    public class FixedRandomizer : IRandomizer
    {
        /// <summary>
        /// <see cref="ValueForProb"/> で返す値を取得または設定します。
        /// </summary>
        public float ValueForProbReturnValue { get; set; }

        /// <summary>
        /// <see cref="SelectOne"/> で選ばれるリストのインデックスを取得または設定します。
        /// </summary>
        public int SelectOneIndex { get; set; }

        /// <summary>
        /// <see cref="FixedRandomizer"/> の新しいインスタンスを生成します。
        /// </summary>
        public FixedRandomizer() { }

        public float ValueForProb => ValueForProbReturnValue;

        public T SelectOne<T>(List<T> list) => list[SelectOneIndex];
    }
}
