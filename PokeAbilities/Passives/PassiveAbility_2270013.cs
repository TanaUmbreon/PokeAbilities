#pragma warning disable CA1031 // Do not catch general exception types

using System;
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
                if (behavior.abilityList.Any() || RandomUtil.valueForProb >= 0.8f) { return; }

                BattleUnitModel target = behavior.card.target;
                if (target == null) { return; }

                owner.battleCardResultLog?.SetPassiveAbility(this);
                target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Fairy, 1, owner);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
