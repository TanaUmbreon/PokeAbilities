using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「にほんばれ」。
    /// 幕の開始時、タイプ付与されていない手元のバトルページ2枚に「ほのおタイプ」をランダムに付与。
    /// 「ほのおタイプ」が付与されたバトルページで受けるダメージ量+1
    /// 「みずタイプ」が付与されたバトルページで受けるダメージ量-1
    /// 幕の終了時、数値が1減少する(最大5)
    /// </summary>
    public class BattleUnitBuf_SunnyDay : BattleUnitBufCustomBase
    {
        /// <summary>被ダメージ軽減量</summary>
        private int dmgReduction;

        /// <summary>
        /// 使用する疑似乱数ジェネレーターを取得または設定します。
        /// </summary>
        public IRandomizer Randomizer { get; set; } = DefaultRandomizer.Instance;

        protected override string keywordId => "SunnyDay";

        protected override string keywordIconId => "SunnyDayBuf";

        protected override int MaxStack => 5;

        /// <summary>
        /// <see cref="BattleUnitBuf_SunnyDay"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitBuf_SunnyDay()
            => LoadIcon();

        public override void OnRoundStartAfter()
        {
            try
            {
                AddTypeBufToHand();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        private void AddTypeBufToHand()
        {
            const int MaxGivenCount = 2;
            const PokeType GiveningType = PokeType.Fire;

            int count = 0;
            var hand = new List<BattleDiceCardModel>(_owner.allyCardDetail.GetHand().Where(c => !c.HasBuf<BattleDiceCardBuf_Type>()));
            while (count < MaxGivenCount && hand.Count > 0)
            {
                BattleDiceCardModel givenCard = Randomizer.SelectOne(hand);

                givenCard.TryAddType(GiveningType);

                hand.Remove(givenCard);
                count++;
            }
        }

        public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            try
            {
                dmgReduction = 0;

                BattleDiceCardModel attackerDiceAction = attacker?.currentDiceAction?.card;
                BattleDiceBehavior attackerBehavior = attacker?.currentDiceAction?.currentBehavior;
                if (attackerDiceAction == null || attackerBehavior == null) { return; }
                if (!IsAttackDice(attackerBehavior.Detail)) { return; }

                if (attackerDiceAction.HasType(PokeType.Fire))
                {
                    dmgReduction = -1;
                    return;
                }

                if (attackerDiceAction.HasType(PokeType.Water))
                {
                    dmgReduction = 1;
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        public override int GetDamageReductionAll()
            => dmgReduction;

        public override void OnRoundEnd()
        {
            stack--;
            if (stack <= 0)
            {
                Destroy();
            }
        }
    }
}
