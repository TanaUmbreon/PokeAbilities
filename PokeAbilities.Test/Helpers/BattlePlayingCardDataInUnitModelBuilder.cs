using LOR_DiceSystem;
using System;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// <see cref="BattlePlayingCardDataInUnitModel"/> のインスタンスを構築します。
    /// </summary>
    public class BattlePlayingCardDataInUnitModelBuilder
    {
        /// <summary>
        /// バトル ページの所有キャラクターを取得または設定します。このプロパティは必須です。
        /// </summary>
        public BattleUnitModel Owner { get; set; }

        /// <summary>
        /// 生成元となるバトル ページを取得または設定します。このプロパティは必須です。
        /// </summary>
        public BattleDiceCardModel Card { get; set; }

        /// <summary>
        /// バトル ページの対象キャラクターを取得または設定します。このプロパティは必須です。
        /// </summary>
        public BattleUnitModel Target { get; set; }

        /// <summary>
        /// バトル ページの対象キャラクターの速度ダイス スロット番号を取得または設定します。
        /// </summary>
        public int TargetOrder { get; set; }

        /// <summary>
        /// バトル ページ効果を取得または設定します。
        /// インスタンスが設定されている場合、<see cref="DiceCardSelfAbilityBase.OnApplyCard()"/> メソッドも呼び出されます。
        /// </summary>
        public DiceCardSelfAbilityBase DiceCardSelfAbility { get; set; }

        ///// <summary>
        ///// 現在のバトル ダイスの振る舞いを取得または設定します。
        ///// </summary>
        //public BattleDiceBehavior CurrentBehavior { get; set; }

        /// <summary>
        /// <see cref="BattlePlayingCardDataInUnitModelBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattlePlayingCardDataInUnitModelBuilder() { }

        /// <summary>
        /// 現在設定さている情報から、
        /// <see cref="BattlePlayingCardDataInUnitModel"/> のインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BattlePlayingCardDataInUnitModel ToBattlePlayingCardDataInUnitModel()
        {
            Thrower.ThrowIfPropertyIsNull(() => Owner);
            Thrower.ThrowIfPropertyIsNull(() => Card);
            Thrower.ThrowIfPropertyIsNull(() => Target);

            return ImitateAddCard();
        }

        /// <summary>
        /// <see cref="BattlePlayingCardSlotDetail.AddCard(BattleDiceCardModel, BattleUnitModel, int, bool)"/> メソッドを疑似的に再現し、
        /// <see cref="BattlePlayingCardDataInUnitModel"/> のインスタンスを生成します。
        /// </summary>
        /// <returns></returns>
        private BattlePlayingCardDataInUnitModel ImitateAddCard()
        {
            var card = new BattlePlayingCardDataInUnitModel()
            {
                owner = Owner,
                card = Card,
                target = Target,
                earlyTarget = Target,
                earlyTargetOrder = TargetOrder,
                cardAbility = DiceCardSelfAbility,
                //currentBehavior = CurrentBehavior,
            };

            // HACK: 必要であれば、広域攻撃のサブ対象キャラクターリストを設定する (subTargets)

            if (card.cardAbility != null)
            {
                card.cardAbility.OnApplyCard();
            }

            ImitiateResetCardQueue(card);

            //if (Owner != null)
            //{
            //    Owner.currentDiceAction = card;
            //}
            //if (CurrentBehavior != null)
            //{
            //    CurrentBehavior.card = card;
            //}

            return card;
        }

        /// <summary>
        /// <see cref="BattlePlayingCardDataInUnitModel.ResetCardQueue()"/> メソッドを疑似的に再現し、
        /// 指定された戦闘で使用するバトル ページのストックされているバトル ダイスのキューを初期化します。
        /// </summary>
        /// <param name="card">ストックされているバトル ダイスのキューを初期化する、戦闘で使用するバトル ページ。</param>
        private static void ImitiateResetCardQueue(BattlePlayingCardDataInUnitModel card)
        {
            if (card.card == null) { return; }

            card.cardBehaviorQueue.Clear();
            int index = 0;
            foreach (DiceBehaviour behaviour in card.card.XmlData.DiceBehaviourList)
            {
                // HACK: 必要であれば、BattleDiceBehaviorBuilder クラスにここでインスタンス生成している過程を統合する。
                var behaviour2 = new BattleDiceBehavior() { behaviourInCard = behaviour };
                behaviour2.SetIndex(index);

                // HACK: 必要であれば、バトルダイス効果を生成して behaviour2.AddAbilityを呼び出す。

                ImitiateAddDice(card, behaviour2);

                index++;
            }
        }

        /// <summary>
        /// <see cref="BattlePlayingCardDataInUnitModel.AddDice(BattleDiceBehavior)"/> メソッドを疑似的に再現し、
        /// 指定された戦闘で使用するバトル ページのバトル ダイスのキューに指定されたバトル ダイスの振る舞いを追加します。
        /// </summary>
        /// <param name="card">戦闘で使用するバトル ページ。</param>
        /// <param name="diceBehavior">バトル ダイスのキューに追加するバトル ダイスの振る舞い。</param>
        private static void ImitiateAddDice(BattlePlayingCardDataInUnitModel card, BattleDiceBehavior diceBehavior)
        {
            card.cardBehaviorQueue.Enqueue(diceBehavior);
            diceBehavior.card = card;
        }
    }
}