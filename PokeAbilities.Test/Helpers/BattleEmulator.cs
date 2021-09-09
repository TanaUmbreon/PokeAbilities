using System;
using LOR_DiceSystem;

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
        /// 指定したバトル ページを指定した対象キャラクターに向かって使用する状態のバトル ダイスの振る舞いを生成します。
        /// </summary>
        /// <param name="card">使用するバトル ページ。</param>
        /// <param name="target">使用するバトル ページの対象となるキャラクター。</param>
        /// <param name="detail"></param>
        /// <param name="diceVanillaValue">ロール ダイス後かつ、威力のボーナス値が適用される前のダイス値。</param>
        /// <returns></returns>
        public static BattleDiceBehavior CreateAttackingBehavior(BattleDiceCardModel card, BattleUnitModel target, BehaviourDetail detail = BehaviourDetail.None, int diceVanillaValue = 0)
        {
            BattleDiceBehavior behavior = new BattleDiceBehaviorBuilder()
            {
                Detail = detail,
                DiceVanillaValue = diceVanillaValue,
            }.ToBattleDiceBehavior();
            _ = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Owner = card.owner,
                Target = target,
                CurrentBehavior = behavior,
                Card = card,
            }.ToBattlePlayingCardDataInUnitModel();

            return behavior;
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
            }.ToBattleUnitModel();

            attaker.currentDiceAction = new BattlePlayingCardDataInUnitModelBuilder()
            {
                Target = target,
                Card = new BattleDiceCardModelBuilder()
                {
                    Owner = attaker,
                }.ToBattleDiceCardModel(),
                CurrentBehavior = new BattleDiceBehaviorBuilder()
                {
                    Detail = currentBehaviourDetail,
                }.ToBattleDiceBehavior(),
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
            }.ToBattleUnitModel();

            if (maxHp != currentHp)
            {
                owner.SetHp(currentHp);
            }

            return owner;
        }
    }
}
