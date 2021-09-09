using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「こんじょう」
    /// 状態異常のとき、虚弱状態の影響を受けず、攻撃ダイスの威力+1
    /// </summary>
    public class PassiveAbility_2270007 : PassiveAbilityBase
    {
        public override bool IsImmune(KeywordBuf buf)
            => buf == KeywordBuf.Weak || base.IsImmune(buf);

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            try
            {
                if (!IsAttackDice(behavior.Detail)) { return; }
                if (!owner.bufListDetail.HasBuf(BufPositiveType.Negative)) { return; }

                owner.battleCardResultLog?.SetPassiveAbility(this);
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
