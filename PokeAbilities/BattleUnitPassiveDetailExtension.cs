using System.Linq;
using PokeAbilities.Passives;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="BattleUnitPassiveDetail"/> の拡張メソッドを提供します。
    /// </summary>
    public static class BattleUnitPassiveDetailExtension
    {
        /// <summary>
        /// 指定したタイプを持っている事を判定します。
        /// </summary>
        /// <param name="target">判定する対象のパッシブ詳細。</param>
        /// <param name="type">判定するタイプ。</param>
        /// <returns>指定したタイプを持つ場合は true、そうでない場合は false。</returns>
        public static bool HasType(this BattleUnitPassiveDetail target, PokeType type)
            => target.PassiveList.OfType<PassiveAbilityTypeBase>().Any(p => p.Types.Contains(type));
    }
}
