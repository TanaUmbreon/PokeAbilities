#pragma warning disable CA1031 // Do not catch general exception types

using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「こんじょう」。
    /// 状態異常のとき、50%の確率で攻撃ダイスの威力+1。
    /// </summary>
    public class PassiveAbility_2270007 : PassiveAbilityBase
    {
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            try
            {
                if (!IsAttackDice(behavior.Detail)) { return; }
                if (!owner.bufListDetail.ExistsPositiveType(BufPositiveType.Negative)) { return; }
                if (RandomUtil.valueForProb >= 0.5f) { return; }

                owner.battleCardResultLog?.SetPassiveAbility(this);
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(this, nameof(OnRollDice), "Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
        }
    }
}
