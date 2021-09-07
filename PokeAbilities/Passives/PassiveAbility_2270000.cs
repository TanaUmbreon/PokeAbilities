using System;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「シンクロ」
    /// 敵からページで出血・麻痺・火傷が付与されたとき、相手にも同じ状態を付与
    /// </summary>
    public class PassiveAbility_2270000 : PassiveAbilityBase
    {
        /// <summary>攻撃を行った敵キャラクター</summary>
        private BattleUnitModel attacker = null;

        public override int OnAddKeywordBufByCard(BattleUnitBuf buf, int stack)
        {
            try
            {
                if (attacker == null || attacker.IsDead() || !IsTargetBuf(buf))
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

        /// <summary>
        /// 指定したバフがこのパッシブの効果対象である事を判定します。
        /// </summary>
        /// <param name="buf">判定するバフ。</param>
        /// <returns>効果対象の場合は true、そうでない場合は false。</returns>
        private bool IsTargetBuf(BattleUnitBuf buf)
            => buf.bufType == KeywordBuf.Burn || buf.bufType == KeywordBuf.Bleeding || buf.bufType == KeywordBuf.Paralysis;

        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
            => attacker = (attackerCard.owner.faction != owner.faction) ? attackerCard.owner : null;

        public override void OnEndOneSideVictim(BattlePlayingCardDataInUnitModel attackerCard)
            => attacker = null;

        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            => attacker = (card.target.faction != owner.faction) ? card.target : null;

        public override void OnEndParrying()
            => attacker = null;

        public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
            => attacker = null;

        public override void OnRoundStart() 
            => attacker = null;

        public override void OnRoundEnd()
            => attacker = null;
    }
}
