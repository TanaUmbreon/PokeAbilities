using System;
using System.Collections.Generic;
using System.Linq;
using PokeAbilities.Bufs;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// タイプ系パッシブの基底クラスです。このクラスを継承することでタイプ系パッシブとして扱われます。
    /// </summary>
    public abstract class PassiveAbilityTypeBase : PassiveAbilityBase
    {
        /// <summary>
        /// 使用する疑似乱数ジェネレーターを取得または設定します。
        /// </summary>
        public IRandomizer Randomizer { get; set; } = DefaultRandomizer.Instance;

        /// <summary>
        /// 所有キャラクターと手元のバトル ページに付与させるタイプを取得します。
        /// </summary>
        public abstract IEnumerable<PokeType> Types { get; }

        public override void OnRoundStartAfter()
        {
            try
            {
                AddTypeBufToHand();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 所有キャラクターの手元の、タイプ付与されていないバトルページ2枚にタイプをランダムに付与します。
        /// このタイプ系パッシブが単体タイプの場合はそのタイプを、
        /// 複合タイプの場合はそれぞれのタイプをバトルページに付与します。
        /// </summary>
        private void AddTypeBufToHand()
        {
            const int MaxGivenCount = 2;

            int count = 0;
            var hand = new List<BattleDiceCardModel>(owner.allyCardDetail.GetHand().Where(c => !c.HasBuf<BattleDiceCardBuf_Type>()));
            var givingTypes = new List<PokeType>(Types);
            while (count < MaxGivenCount && hand.Count > 0)
            {
                if (givingTypes.Count <= 0)
                {
                    givingTypes.AddRange(Types);
                }

                BattleDiceCardModel givenCard = Randomizer.SelectOne(hand);
                PokeType givingType = Randomizer.SelectOne(givingTypes);

                givenCard.TryAddType(givingType);

                hand.Remove(givenCard);
                givingTypes.Remove(givingType);
                count++;
            }
        }

        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            try
            {
                ApplyBonusIfTypeMatched(behavior);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        /// <summary>
        /// タイプ一致である場合、指定したバトル ダイスに対してボーナスを適用します。
        /// </summary>
        /// <param name="behavior">ボーナスを適用させる対象のバトル ダイス。</param>
        private void ApplyBonusIfTypeMatched(BattleDiceBehavior behavior)
        {
            if (!IsAttackDice(behavior.Detail)) { return; }
            if (!behavior.card.card.HasType(Types)) { return; }

            owner.battleCardResultLog?.SetPassiveAbility(PassiveAbility_22710000.Instance);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1 });
        }

        /// <summary>
        /// 指定したタイプを所有している事を判定します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasType(PokeType type)
        {
            return Types.Contains(type);
        }
    }
}
