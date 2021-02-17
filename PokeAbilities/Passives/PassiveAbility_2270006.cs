#pragma warning disable CA1031 // Do not catch general exception types

using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「もらいび」。
    /// 火傷ダメージを受けず、火傷を受けている間は50%の確率で攻撃ダイスの威力+1。
    /// </summary>
    public class PassiveAbility_2270006 : PassiveAbilityBase
    {
        public override bool IsImmune(KeywordBuf buf) 
            => buf == KeywordBuf.Burn || base.IsImmune(buf);

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            try
            {
                if (!IsAttackDice(behavior.Detail)) { return; }
                if (!owner.bufListDetail.ExistsKeywordBuf(KeywordBuf.Burn)) { return; }
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
