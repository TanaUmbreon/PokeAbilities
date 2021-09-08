using System.Linq;
using LOR_DiceSystem;
using PokeAbilities.Bufs;
using PokeAbilities.Passives;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="BattleDiceBehavior"/> の拡張メソッドを提供します。
    /// </summary>
    public static class BattleDiceBehaviorExtension
    {
        /// <summary>
        /// タイプ一致状態である事を判定します。
        /// タイプ一致は、このバトル ダイスが攻撃ダイスであるかつ、このバトル ダイスのバトル ページに付与されたタイプが、このバトル ダイスの所有キャラクターが持つタイプと同じである状態を表します。
        /// </summary>
        /// <param name="behavior">判定する対象のバトル ダイスの振る舞い。</param>
        /// <returns>タイプ一致状態である場合は true、そうでない場合は false。</returns>
        public static bool IsSameType(this BattleDiceBehavior behavior)
        {
            if (!IsAttackDice(behavior.Detail)) { return false; }

            // 所有キャラクターのタイプ系パッシブを取得
            // タイプ系パッシブがない場合はタイプ一致にならず、タイプ系パッシブが複数ある場合は異常とする
            var unitType = behavior.owner.passiveDetail.PassiveList.OfType<PassiveAbilityTypeBase>().SingleOrDefault();
            if (unitType == null) { return false; }

            // バトルページに付与されたタイプ系状態を取得
            // こちらはタイプ系状態が複数付与されている場合を考慮する
            foreach (var cardType in behavior.card.card.GetBufList().OfType<BattleDiceCardBuf_Type>())
            {
                if (unitType.HasType(cardType.Type)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// 指定されたバトル ダイスの振る舞いの詳細が攻撃ダイスである事を判定します。
        /// </summary>
        /// <param name="detail">判定する対象のバトル ダイスの振る舞いの詳細。</param>
        /// <returns>攻撃ダイスである場合は true、そうでない場合は false。</returns>
        private static bool IsAttackDice(BehaviourDetail detail)
            => detail == BehaviourDetail.Slash ||
               detail == BehaviourDetail.Penetrate ||
               detail == BehaviourDetail.Hit;
    }
}
