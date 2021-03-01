#pragma warning disable CA1031 // Do not catch general exception types

using System;
using LOR_DiceSystem;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「メロメロボディ」
    /// 近接バトルページの攻撃を受けたとき、10%の確率で相手にメロメロ状態を付与。
    /// </summary>
    public class PassiveAbility_2270012 : PassiveAbilityBase
    {
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            try
            {
                CardRange range = atkDice.card.card.GetSpec().Ranged;
                if (range != CardRange.Near || RandomUtil.valueForProb >= 0.1f) { return; }

                BattleUnitModel target = atkDice.owner;
                if (target.bufListDetail.HasBuf<BattleUnitBuf_Infatuation>()) { return; }

                var buf = new BattleUnitBuf_Infatuation(owner);
                target.bufListDetail.AddBuf(buf);
                buf.OnAddBuf();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
