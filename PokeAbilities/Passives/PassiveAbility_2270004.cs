using System;
using System.Linq;
using LOR_DiceSystem;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「てきおうりょく」
    /// タイプ一致のとき、更に与えるダメージ・混乱ダメージ量+1
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
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
