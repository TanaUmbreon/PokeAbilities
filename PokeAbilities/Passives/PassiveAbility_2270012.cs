#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「メロメロボディ」
    /// 近接バトルページの攻撃を受けたとき、30%の確率で相手にメロメロ状態を付与。
    /// </summary>
    public class PassiveAbility_2270012 : PassiveAbilityBase
    {
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            try
            {
                Log.Instance.AppendLine(this, nameof(OnTakeDamageByAttack), "Called.");

                CardRange range = atkDice.card.card.GetSpec().Ranged;
                if (range != CardRange.Near) { return; }
                if (RandomUtil.valueForProb >= 0.3f) { return; }

                BattleUnitModel target = atkDice.card.target;
                if (target == null || target.bufListDetail.HasBuf<BattleUnitBuf_Infatuation>()) { return; }

                target.bufListDetail.AddBuf(new BattleUnitBuf_Infatuation(owner));
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(this, nameof(OnTakeDamageByAttack), "Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
        }
    }
}
