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
    }
}
