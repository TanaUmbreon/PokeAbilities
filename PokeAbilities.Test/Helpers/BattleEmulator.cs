using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Test.Helpers.Builders;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// 戦闘で行われている処理を疑似的に模倣するユーティリティ クラスです。
    /// </summary>
    public static class BattleEmulator
    {
        /// <summary>
        /// 指定したキャラクターに指定した状態をこの幕から付与し、
        /// <see cref="BattleUnitBuf.OnAddBuf(int)"/> を呼び出します。
        /// </summary>
        /// <param name="target">状態を付与する対象のキャラクター。</param>
        /// <param name="buf">付与する状態。</param>
        public static void AddBuf(BattleUnitModel target, BattleUnitBuf buf)
        {
            target.bufListDetail.AddBuf(buf);
            buf.OnAddBuf(buf.stack);
        }

        /// <summary>
        /// 指定したキャラクターに指定したパッシブを追加し、パッシブをアクティブ状態にします。
        /// </summary>
        /// <param name="target">パッシブを追加する対象のキャラクター。</param>
        /// <param name="passive">追加するパッシブ。</param>
        public static void AddPassive(BattleUnitModel target, PassiveAbilityBase passive)
        {
            target.passiveDetail.AddPassive(passive);
            target.passiveDetail.OnCreated();
        }

        /// <summary>
        /// 攻撃キャラクターが所有する指定したバトル ページを使用して、
        /// 指定したキャラクターに向かって一方攻撃する状態のバトル ダイスの振る舞いを生成します。
        /// </summary>
        /// <param name="attackerCard">一方攻撃するキャラクターが使用するバトル ページ。</param>
        /// <param name="target">一方攻撃を受ける対象キャラクター。</param>
        /// <param name="detail">一方攻撃するバトル ダイスの振る舞いの詳細。</param>
        /// <param name="diceVanillaValue">一方攻撃するバトル ダイスの、ロール ダイス後かつ、威力のボーナス値が適用される前のダイス値。</param>
        /// <returns></returns>
        public static BattleDiceBehavior CreateOneSideBehavior(BattleDiceCardModel attackerCard, BattleUnitModel target, BehaviourDetail detail = BehaviourDetail.None, int diceVanillaValue = 1)
        {
            BattleDiceBehavior behavior = new BattleDiceBehaviorBuilder()
            {
                Detail = detail,
                DiceVanillaValue = diceVanillaValue,
            }.ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = attackerCard.owner,
                Target = target,
                //CurrentBehavior = behavior,
                Card = attackerCard,
            }.ToBattlePlayingCardDataInUnitModel();
            return behavior;
        }

        ///// <summary>
        ///// 攻撃キャラクターと対象キャラクターがそれぞれ所有する指定したバトル ページを使用して、
        ///// マッチ進行する状態のバトル ダイスの振る舞いを生成します。
        ///// </summary>
        ///// <param name="attackerCard">攻撃キャラクターが使用するバトル ページ。</param>
        ///// <param name="targetCard">対象キャラクターが使用するバトル ページ。</param>
        ///// <param name="attackerDetail">攻撃キャラクター側でマッチ進行するバトル ダイスの振る舞いの詳細。</param>
        ///// <param name="attackerDiceVanillaValue">攻撃キャラクター側でマッチ進行するバトル ダイスの、ロール ダイス後かつ、威力のボーナス値が適用される前のダイス値。</param>
        ///// <param name="targetDetail">対象キャラクター側でマッチ進行するバトル ダイスの振る舞いの詳細。</param>
        ///// <param name="targetDiceVanillaValue">対象キャラクター側でマッチ進行するバトル ダイスの、ロール ダイス後かつ、威力のボーナス値が適用される前のダイス値。</param>
        ///// <returns>順番に、攻撃キャラクター、対象キャラクターが使用するバトル ダイスの振る舞いを格納したタプル。</returns>
        //public static (BattleDiceBehavior attackerBehavior, BattleDiceBehavior targetBehavior) CreateParryingBehaviors(BattleDiceCardModel attackerCard, BattleDiceCardModel targetCard, BehaviourDetail attackerDetail = BehaviourDetail.None, int attackerDiceVanillaValue = 1, BehaviourDetail targetDetail = BehaviourDetail.None, int targetDiceVanillaValue = 1)
        //{
        //    BattleDiceBehavior attackerBehavior = new BattleDiceBehaviorBuilder()
        //    {
        //        Detail = attackerDetail,
        //        DiceVanillaValue = attackerDiceVanillaValue,
        //    }.ToBattleDiceBehavior();
        //    _ = new BattlePlayingCardDataInUnitModelBuilder()
        //    {
        //        Owner = attackerCard.owner,
        //        Target = targetCard.owner,
        //        //CurrentBehavior = attackerBehavior,
        //        Card = attackerCard,
        //    }.ToBattlePlayingCardDataInUnitModel();
        //
        //    BattleDiceBehavior targetBehavior = new BattleDiceBehaviorBuilder()
        //    {
        //        Detail = targetDetail,
        //        DiceVanillaValue = targetDiceVanillaValue,
        //    }.ToBattleDiceBehavior();
        //    _ = new BattlePlayingCardDataInUnitModelBuilder()
        //    {
        //        Owner = targetCard.owner,
        //        Target = attackerCard.owner,
        //        //CurrentBehavior = targetBehavior,
        //        Card = targetCard,
        //    }.ToBattlePlayingCardDataInUnitModel();
        //
        //    PrivateAccess.SetField(attackerBehavior, "_targetDice", targetBehavior);
        //    PrivateAccess.SetField(targetBehavior, "_targetDice", attackerBehavior);
        //
        //    return (attackerBehavior, targetBehavior);
        //}

        internal static BattleDiceBehavior CreateAttackingBehavior(BattleDiceCardModel enemyCardInHand, BattleUnitModel owner, BehaviourDetail hit, int diceVanillaValue = 1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定したバトル ダイスの種類で指定したキャラクターに向かって使用する状態のキャラクターを生成します。
        /// </summary>
        /// <param name="currentBehaviourDetail">使用するバトル ダイスの種類。</param>
        /// <param name="target">使用するバトル ダイスの対象となるキャラクター。</param>
        /// <returns></returns>
        public static BattleUnitModel CreateAttacker(BehaviourDetail currentBehaviourDetail, BattleUnitModel target)
        {
            BattleUnitModel attaker = new BattleUnitModelBuilder()
            {
                Faction = Faction.Enemy,
            }.Build();

            attaker.currentDiceAction = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Target = target,
                Card = new BattleDiceCardModelBuilder()
                {
                }.Build(attaker),
                //CurrentBehavior = new BattleDiceBehaviorBuilder()
                //{
                //    Detail = currentBehaviourDetail,
                //}.ToBattleDiceBehavior(),
            }.ToBattlePlayingCardDataInUnitModel();

            return attaker;
        }

        /// <summary>
        /// 指定した最大体力と現在体力を持つキャラクターを生成します。
        /// </summary>
        /// <param name="maxHp">キャラクターの最大体力。</param>
        /// <param name="currentHp">キャラクターの現在体力。</param>
        /// <returns></returns>
        public static BattleUnitModel CreateDamagedUnit(int maxHp, int currentHp)
        {
            BattleUnitModel owner = new BattleUnitModelBuilder()
            {
                EquipBook = new BookXmlInfoBuilder()
                {
                    Hp = maxHp,
                }.ToBookXmlInfo(),
            }.Build();

            if (maxHp != currentHp)
            {
                owner.SetHp(currentHp);
            }

            return owner;
        }

        ///// <summary>
        ///// 指定した攻撃キャラクターが指定した対象キャラクターに対して一方攻撃を行う状態を構築します。
        ///// 一方攻撃で使用するバトル ページおよびバトル ダイスは規定のものを使用します。
        ///// </summary>
        ///// <param name="attacker">攻撃キャラクター。</param>
        ///// <param name="target">対象キャラクター。</param>
        //public static void SetUpOneSidePlay(BattleUnitModel attacker, BattleUnitModel target)
        //{
        //    if (attacker == null) { throw new ArgumentNullException(nameof(attacker)); }
        //    if (target == null) { throw new ArgumentNullException(nameof(target)); }

        //    BattleDiceCardModel card = new BattleDiceCardModelBuilder().Build(attacker);

        //    SetUpOneSidePlay(attacker, card, target);
        //}

        ///// <summary>
        ///// 指定した攻撃キャラクターが指定した対象キャラクターに対して指定したバトル ページで一方攻撃を行う状態を構築します。
        ///// </summary>
        ///// <param name="attacker">攻撃キャラクター。</param>
        ///// <param name="attackerCard">使用するバトル ページ。</param>
        ///// <param name="target">対象キャラクター。</param>
        //public static void SetUpOneSidePlay(BattleUnitModel attacker, BattleDiceCardModel attackerCard, BattleUnitModel target)
        //{
        //    if (attacker == null) { throw new ArgumentNullException(nameof(attacker)); }
        //    if (target == null) { throw new ArgumentNullException(nameof(target)); }
        //    if (attackerCard == null) { throw new ArgumentNullException(nameof(attackerCard)); }

        //    SetUpOneSidePlay(new BattlePlayingCardDataInUnitModelBuilder()
        //    {
        //        Owner = attacker,
        //        Target = target,
        //        Card = attackerCard,
        //    }.ToBattlePlayingCardDataInUnitModel());
        //}

        //public static void SetUpOneSidePlay(BattlePlayingCardDataInUnitModel card)
        //{
        //    if (card == null) { throw new ArgumentNullException(nameof(card)); }

        //    // ToDo: BattleOneSidePlayManager.StartOneSidePlay(BattlePlayingCardDataInUnitModel) メソッドを模倣する
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 指定したキャラクターが速度ダイスを振る状態を構築します。
        /// </summary>
        /// <param name="units">速度ダイスを振るキャラクターの配列。</param>
        public static void SetUpRollSpeedDice(params BattleUnitModel[] units)
        {
            // [参考] StageController.SortUnitPhase()

            foreach (BattleUnitModel unit in units)
            {
                unit.RollSpeedDice();
            }
            foreach (BattleUnitModel unit in units)
            {
                unit.AfterRollSpeedDice();
            }
        }

        /// <summary>
        /// 指定したバトル ページで対象キャラクターを一方攻撃する状態を構築します。
        /// </summary>
        /// <param name="attackerCard">攻撃に使用するバトル ページ。</param>
        /// <param name="target">一方攻撃の対象キャラクター。</param>
        public static (BattlePlayingCardDataInUnitModel attackerCurrentDiceAction, BattleDiceBehavior attackerCurrentBehavior) SetUpOneSidePlay(BattleDiceCardModel attackerCard, BattleUnitModel target)
        {
            if (attackerCard == null) { throw new ArgumentNullException(nameof(attackerCard)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (attackerCard.owner == null)
            {
                throw new ArgumentException("攻撃に使用するバトル ページの所有キャラクターを指定してください。", nameof(attackerCard));
            }

            BattlePlayingCardDataInUnitModel cardData = CreateBattlePlayingCardDataInUnitModel(attackerCard.owner, attackerCard, 0, target, 0);
            StartOneSidePlay(cardData);

            Assert.That(cardData.owner, Is.EqualTo(attackerCard.owner));
            return (cardData.owner.currentDiceAction, cardData.currentBehavior);
        }

        private static BattlePlayingCardDataInUnitModel CreateBattlePlayingCardDataInUnitModel(BattleUnitModel attacker, BattleDiceCardModel attackerCard, int attackerSlotOrder, BattleUnitModel target, int targetSlotOrder)
        {
            if (attacker == null) { throw new ArgumentNullException(nameof(attacker)); }
            if (attackerCard == null) { throw new ArgumentNullException(nameof(attackerCard)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }

            // [参考] BattlePlayingCardSlotDetail.AddCard(BattleDiceCardModel, BattleUnitModel, int, bool)

            // 参考側では装着時発動だと専用の処理を行っているが、ここでは現状不要なので例外とする
            if (attackerCard.GetSpec().Ranged == CardRange.Instance)
            {
                throw new NotImplementedException("装着時発動バトル ページでの一方攻撃の構築はサポートされていません。");
            }

            var cardData = new BattlePlayingCardDataInUnitModel()
            {
                owner = attacker,
                card = attackerCard,
                target = target,
            };

            // 参考側では、広域攻撃バトル ページならば攻撃対象のキャラクターリストを取得し、
            // 自分自身とメインで指定した対象キャラクター以外のキャラクターをサブ対象キャラクターとする処理を行っている。
            // ここでは現状不要なので実装を省略している

            cardData.earlyTarget = target;
            cardData.earlyTargetOrder = targetSlotOrder;
            cardData.cardAbility = attackerCard.CreateDiceCardSelfAbilityScript();
            if (cardData.cardAbility != null)
            {
                cardData.cardAbility.card = cardData;
                cardData.cardAbility.OnApplyCard();
            }
            cardData.ResetCardQueue();

            // 参考側では、敵側が行う攻撃対象の選択状態(引数isEnemyAutoがtrue)ならば専用の処理をしている。
            // ここでは現状不要なので実装を省略している

            // またそれ以外で、攻撃キャラクターの速度ダイススロットが正しく選択されている状態ならば、
            // その速度ダイススロットに攻撃で使用するバトルページのデータをセットしている。

            // さらにその速度ダイススロットに既に別のバトルページのデータがセットされている場合、
            // そのバトルページのデータを破棄して手札にバトルページを戻すようにしている。

            // さらにその使用するバトルページによって攻撃対象のマッチが取れるならば、
            // そのマッチ先となる相手のバトルページのデータを書き換えている。

            // ここでは戦闘で使用するバトルページ枠(BattlePlayingCardSlotDetail)より
            // バトルページのデータ(BattlePlayingCardDataInUnitModel)のインスタンス生成を優先しているので不要なものは実装を省略している

            if (attackerCard.XmlData.IsFloorEgo())
            {
                Singleton<SpecialCardListModel>.Instance.UseCard(attackerCard);
            }
            else if (attackerCard.XmlData.IsPersonal())
            {
                attacker.personalEgoDetail.UseCard(attackerCard);
            }
            else
            {
                attacker.allyCardDetail.UseCard(attackerCard);
            }
            cardData.targetSlotOrder = targetSlotOrder;
            cardData.speedDiceResultValue = attacker.GetSpeedDiceResult(attackerSlotOrder).value;
            cardData.slotOrder = attackerSlotOrder;

            return cardData;
        }

        /// <summary>
        /// 指定したバトル ページのデータで一方攻撃を開始します。
        /// 一方攻撃の開始に関わる一連のイベントを呼び出します。
        /// </summary>
        /// <param name="card">一方攻撃に使用するバトル ページのデータ。</param>
        private static void StartOneSidePlay(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null) { throw new ArgumentNullException(nameof(card)); }
            if (card.owner == null)
            {
                throw new ArgumentException("バトル ページのデータの所有キャラクターを指定してください。", nameof(card)); 
            }
            if (card.target == null)
            {
                throw new ArgumentException("バトル ページのデータの対象キャラクターを指定してください。", nameof(card));
            }

            // [参考] BattleOneSidePlayManager.StartOneSidePlay(BattlePlayingCardDataInUnitModel)
            // イベント呼び出しを主目的としているため、参考元で実装していても現状不要なものはここでは省略している

            card.owner.currentDiceAction = card;
            card.owner.battleCardResultLog = new BattleCardTotalResult(card);
            card.target.battleCardResultLog = new BattleCardTotalResult(card);

            card.OnUseCard_before();
            card.owner.OnUseCard(card);

            card.owner.OnStartCardAction(card);

            card.OnStartOneSideAction();
            card.owner.OnStartOneSideAction(card);
            card.target.OnStartTargetedOneSide(card);

            card.NextDice();
        }
    }
}
