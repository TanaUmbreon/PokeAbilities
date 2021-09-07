using System;
using System.Linq;
using LOR_DiceSystem;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「きけんよち」
    /// 弱点や脆弱属性の攻撃を受けるとき、その相手のダイス威力を-1
    /// </summary>
    public class PassiveAbility_2270005 : PassiveAbilityBase
    {
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            try
            {
                PowerDownDiceIfWeakOrVulnerable(attackerCard);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            try
            {
                BattlePlayingCardDataInUnitModel opponentCard = card?.target?.currentDiceAction;
                if (opponentCard == null) { return; }
                PowerDownDiceIfWeakOrVulnerable(opponentCard);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 指定した相手のバトル ページに弱点または脆弱属性となるダイスが含まれていた場合、そのダイス威力を -1 します。
        /// </summary>
        /// <param name="opponentCard">相手が使用しているバトル ページのデータ。</param>
        private void PowerDownDiceIfWeakOrVulnerable(BattlePlayingCardDataInUnitModel opponentCard)
        {
            // 広域攻撃・個別広域攻撃には無効
            CardRange range = opponentCard.card.GetSpec().Ranged;
            if (range == CardRange.FarArea || range == CardRange.FarAreaEach) { return; }

            var targetDices = opponentCard.GetDiceBehaviorList().Where(d => IsWeakOrVulnerable(d.Detail));
            foreach (BattleDiceBehavior dice in targetDices)
            {
                dice.ApplyDiceStatBonus(new DiceStatBonus() { power = -1 });
            }
            if (targetDices.Any())
            {
                owner.battleCardResultLog?.SetPassiveAbility(this);
            }
        }

        /// <summary>
        /// 指定したバトル ダイスに対して、耐性または混乱耐性に、弱点または脆弱である事を判定します。
        /// </summary>
        /// <param name="detail">バトル ダイスの振る舞いの詳細。</param>
        /// <returns>弱点または脆弱である場合は true、そうでない場合は false。</returns>
        private bool IsWeakOrVulnerable(BehaviourDetail detail)
        {
            AtkResist hpResist = owner.GetResistHP(detail);
            if (hpResist == AtkResist.Weak || hpResist == AtkResist.Vulnerable) { return true; }

            AtkResist bpResist = owner.GetResistBP(detail);
            if (bpResist == AtkResist.Weak || bpResist == AtkResist.Vulnerable) { return true; }

            return false;
        }
    }
}
