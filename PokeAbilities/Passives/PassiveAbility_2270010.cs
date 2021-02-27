﻿using System;
using System.Collections.Generic;
using System.Linq;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ちょすい」
    /// 「みずタイプ」が付与されたバトルページでダメージを受けず、体力を2回復する。
    /// </summary>
    public class PassiveAbility_2270010 : PassiveAbilityBase
    {
        /// <summary>被ダメージ減衰量</summary>
        private int dmgReduction;

        public override void OnCreated()
        {
            dmgReduction = 0;
        }

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            try
            {
                dmgReduction = 0;

                BattleDiceCardModel card = attacker?.currentDiceAction?.card;
                if (card == null || !card.HasBuf<BattleDiceCardBuf_WaterType>())
                {
                    return base.BeforeTakeDamage(attacker, dmg);
                }

                dmgReduction = 9999;
                owner.RecoverHP(2);
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(this, nameof(BeforeTakeDamage), "Exception thrown.");
                Log.Instance.AppendLine(ex);
            }
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override int GetDamageReductionAll() => dmgReduction;

        public override int GetBreakDamageReductionAll(int dmg, DamageType dmgType, BattleUnitModel attacker)
            => dmgReduction;
    }
}
