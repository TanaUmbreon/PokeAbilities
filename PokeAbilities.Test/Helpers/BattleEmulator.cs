using System;
using System.Collections.Generic;
using LOR_DiceSystem;
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

        /// <summary>
        /// 指定した攻撃キャラクターが指定した対象キャラクターに対して一方攻撃を行う状態を構築します。
        /// 一方攻撃で使用するバトル ページおよびバトル ダイスは規定のものを使用します。
        /// </summary>
        /// <param name="attacker">攻撃キャラクター。</param>
        /// <param name="target">対象キャラクター。</param>
        public static void SetUpOneSidePlay(BattleUnitModel attacker, BattleUnitModel target)
        {
            if (attacker == null) { throw new ArgumentNullException(nameof(attacker)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }

            BattleDiceCardModel card = new BattleDiceCardModelBuilder().Build(attacker);

            SetUpOneSidePlay(attacker, target, card);
        }

        /// <summary>
        /// 指定した攻撃キャラクターが指定した対象キャラクターに対して指定したバトル ページで一方攻撃を行う状態を構築します。
        /// </summary>
        /// <param name="attacker">攻撃キャラクター。</param>
        /// <param name="attackerCard">使用するバトル ページ。</param>
        /// <param name="target">対象キャラクター。</param>
        public static void SetUpOneSidePlay(BattleUnitModel attacker, BattleDiceCardModel attackerCard, BattleUnitModel target)
        {
            if (attacker == null) { throw new ArgumentNullException(nameof(attacker)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (attackerCard == null) { throw new ArgumentNullException(nameof(attackerCard)); }

            SetUpOneSidePlay(new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = attacker,
                Target = target,
                Card = attackerCard,
            }.ToBattlePlayingCardDataInUnitModel());
        }

        public static void SetUpOneSidePlay(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null) { throw new ArgumentNullException(nameof(card)); }

            // ToDo: BattleOneSidePlayManager.StartOneSidePlay(BattlePlayingCardDataInUnitModel) メソッドを模倣する
            throw new NotImplementedException();
        }
    }
}
