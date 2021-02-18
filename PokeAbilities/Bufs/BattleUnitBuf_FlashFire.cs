#pragma warning disable CA1031 // Do not catch general exception types

using System;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「もらいび」。
    /// 火傷ダメージを受けず、50%の確率で攻撃ダイスの威力+1。
    /// </summary>
    public class BattleUnitBuf_FlashFire : BattleUnitBufCustomBase
    {
        protected override string keywordId => "FlashFire";

        protected override string keywordIconId => "FlashFireBuf";

        protected override int MaxStack => 1;

        /// <summary>
        /// <see cref="BattleUnitBuf_FlashFire"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitBuf_FlashFire()
            => LoadIcon();

        public override bool IsImmune(KeywordBuf buf)
            => buf == KeywordBuf.Burn || base.IsImmune(buf);

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            try
            {
                if (!IsAttackDice(behavior.Detail)) { return; }
                if (RandomUtil.valueForProb >= 0.5f) { return; }

                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine("[BattleUnitBuf_FlashFire.BeforeRollDice] Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
        }
    }
}
