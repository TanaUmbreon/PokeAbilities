#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「フェアリースキン」
    /// 効果を持たない攻撃ダイスでの攻撃的中時、80%の確率で妖精を1付与。
    /// </summary>
    public class PassiveAbility_2270013 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            try
            {
                Log.Instance.AppendLine(this, nameof(OnSucceedAttack), "Called.");
                Log.Instance.AppendLine($"- BattleDiceAbilityList.Count: {behavior.abilityList.Count} (CardName: '{behavior.card.card.GetName()}', DiceIndex: {behavior.Index})");

                if (behavior.abilityList.Any()) { return; }
                if (RandomUtil.valueForProb >= 0.8f) { return; }

                owner.battleCardResultLog?.SetPassiveAbility(this);
                behavior.card.target?.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Fairy, 1, owner);
                Log.Instance.AppendLine($"- Add fairy 1");
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(this, nameof(OnSucceedAttack), "Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
        }
    }
}
