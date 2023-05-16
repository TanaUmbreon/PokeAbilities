using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「シンクロ」
    /// 敵からページで出血・麻痺・火傷が付与されたとき、相手にも同じ状態を付与
    /// </summary>
    public class PassiveAbility_2270000 : PassiveAbilityBase
    {
        /// <summary>このパッシブ効果を与える対象となる攻撃キャラクター</summary>
        /// <remarks>
        /// 一方攻撃またはマッチ攻撃開始時に対象とするかどうか判断して値をセットします。
        /// ページによる状態付与時は値がnullか否かで判断できるようにします。
        /// </remarks>
        private BattleUnitModel attacker = null;

        public override int OnAddKeywordBufByCard(BattleUnitBuf buf, int stack)
        {
            try
            {
                if (attacker == null || !IsTargetBuf(buf) || stack <= 0)
                {
                    return base.OnAddKeywordBufByCard(buf, stack);
                }

                owner.battleCardResultLog?.SetPassiveAbility(this);
                attacker.bufListDetail.AddKeywordBufByEtc(buf.bufType, stack);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
            return base.OnAddKeywordBufByCard(buf, stack);
        }

        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            try
            {
                BattleUnitModel attacker = attackerCard.owner;
                if (!IsTargetAttacker(attacker)) { return; }

                this.attacker = attacker;
            }
            catch (Exception ex)
            {
                attacker = null;
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        public override void OnEndOneSideVictim(BattlePlayingCardDataInUnitModel attackerCard)
        {
            attacker = null;
        }

        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            try
            {
                // 例外が発生した場合はnullがセットされている状態を保証する
                this.attacker = null;

                BattleUnitModel attacker = card.owner;
                if (!IsTargetAttacker(attacker)) { return; }

                this.attacker = attacker;
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        public override void OnEndParrying()
        {
            attacker = null;
        }

        public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
        {
            attacker = null;
        }

        public override void OnRoundStart()
        {
            attacker = null;
        }

        public override void OnRoundEnd()
        {
            attacker = null;
        }

        /// <summary>
        /// 指定したバフがこのパッシブの効果対象である事を判定します。
        /// </summary>
        /// <param name="buf">判定するバフ。</param>
        /// <returns>効果対象の場合は true、そうでない場合は false。</returns>
        private bool IsTargetBuf(BattleUnitBuf buf)
            => buf.bufType == KeywordBuf.Burn || buf.bufType == KeywordBuf.Bleeding || buf.bufType == KeywordBuf.Paralysis;

        /// <summary>
        /// 指定した攻撃キャラクターがこのパッシブの効果対象であることを判定します。
        /// </summary>
        /// <param name="attacker">判定する攻撃キャラクター。</param>
        /// <returns>攻撃キャラクターが null でない、かつ死亡していない、かつ派閥が敵の場合は true、そうでない場合は false。</returns>
        private bool IsTargetAttacker(BattleUnitModel attacker)
            => attacker != null && !attacker.IsDead() && attacker.faction != owner.faction;
    }
}
