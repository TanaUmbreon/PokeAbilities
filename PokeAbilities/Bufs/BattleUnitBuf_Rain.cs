#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Collections.Generic;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「あめ」。
    /// 幕の開始時、手元のバトルページ2枚に「みずタイプ」をランダムに付与。
    /// 幕の終了時、数値が1減少する(最大5)
    /// </summary>
    public class BattleUnitBuf_Rain : BattleUnitBufCustomBase
    {
        protected override string keywordId => "Rain";

        protected override string keywordIconId => "RainBuf";

        protected override int MaxStack => 5;

        /// <summary>
        /// <see cref="BattleUnitBuf_Rain"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitBuf_Rain()
            => LoadIcon();

        public override void OnRoundStartAfter()
        {
            try
            {
                List<BattleDiceCardModel> hand = _owner.allyCardDetail.GetHand();
                int num = 0;
                while (num < 2 && hand.Count > 0)
                {
                    BattleDiceCardModel card = RandomUtil.SelectOne(hand);
                    card.AddBuf(new BattleDiceCardBuf_WaterType());
                    hand.Remove(card);
                    num++;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!behavior.card.card.HasBuf<BattleDiceCardBuf_WaterType>()) { return; }

            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1 });
        }

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
