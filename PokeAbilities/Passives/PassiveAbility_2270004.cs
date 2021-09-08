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
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!behavior.IsSameType()) { return; }

            owner.battleCardResultLog?.SetPassiveAbility(this);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
    }
}
