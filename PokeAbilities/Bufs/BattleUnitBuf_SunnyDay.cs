using System;
using System.Collections.Generic;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「にほんばれ」。
    /// 幕の開始時、手元のバトルページ2枚に「ほのおタイプ」をランダムに付与。
    /// 幕の終了時、数値が1減少する(最大5)
    /// </summary>
    public class BattleUnitBuf_SunnyDay : BattleUnitBufCustomBase
    {
        /// <summary>疑似乱数ジェネレーター</summary>
        private readonly IRandomizer randomizer;

        protected override string keywordId => "SunnyDay";

        protected override string keywordIconId => "SunnyDayBuf";

        protected override int MaxStack => 5;

        /// <summary>
        /// 既定の疑似乱数ジェネレーターを使用する、
        /// <see cref="BattleUnitBuf_SunnyDay"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitBuf_SunnyDay()
            : this(DefaultRandomizer.Instance) { }

        /// <summary>
        /// 指定した疑似乱数ジェネレーターを使用する
        /// <see cref="BattleUnitBuf_SunnyDay"/> の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="randomizer">このオブジェクトで使用する疑似乱数ジェネレーター。</param>
        public BattleUnitBuf_SunnyDay(IRandomizer randomizer)
        {
            this.randomizer = randomizer;
            LoadIcon();
        }

        public override void OnRoundStartAfter()
        {
            try
            {
                List<BattleDiceCardModel> hand = _owner.allyCardDetail.GetHand();
                int num = 0;
                while (num < 2 && hand.Count > 0)
                {
                    BattleDiceCardModel card = randomizer.SelectOne(hand);
                    card.AddBuf(new BattleDiceCardBuf_FireType());
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
            if (!behavior.card.card.HasBuf<BattleDiceCardBuf_FireType>()) { return; }

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
