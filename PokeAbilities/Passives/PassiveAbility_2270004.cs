#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Linq;
using LOR_DiceSystem;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「てきおうりょく」。
    /// 反撃を除く、全てのダイスが同じ種類のページを使用したとき、全てのダイス威力を+1。
    /// </summary>
    public class PassiveAbility_2270004 : PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            try
            {
                int detailCount = curCard.GetOriginalDiceBehaviorList()
                    .Where(d => d.Type != BehaviourType.Standby)
                    .GroupBy(d => d.Detail)
                    .Count();

                if (detailCount == 1)
                {
                    owner.battleCardResultLog?.SetPassiveAbility(this);
                    curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 1 });
                }
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(this, nameof(OnUseCard), "Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
        }
    }
}
