using System;
using System.Linq;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「フェアリースキン」
    /// 幕の開始時、手元のページ1枚に「ノーマルタイプ」をランダムに付与。
    /// ノーマルタイプ付きページ使用時、ページのタイプがフェアリータイプに変化し、ページの全攻撃ダイスに「[的中] 妖精1 付与」効果を追加
    /// </summary>
    public class PassiveAbility_2270013 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            try
            {
                if (behavior.abilityList.Any()) { return; }

                BattleUnitModel target = behavior.card.target;
                if (target == null) { return; }

                owner.battleCardResultLog?.SetPassiveAbility(this);
                target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Fairy, 1, owner);
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }
    }
}
