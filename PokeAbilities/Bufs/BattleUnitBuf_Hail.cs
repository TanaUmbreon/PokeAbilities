using PokeAbilities.Passives;
using UnityEngine;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// 状態「あられ」。
    /// 幕の終了時、最大体力の1/16だけダメージを受け(最大5ダメージ)、数値が1減少する(最大5)
    /// </summary>
    public class BattleUnitBuf_Hail : BattleUnitBufCustomBase
    {
        protected override string keywordId => "Hail";

        protected override string keywordIconId => "HailBuf";

        protected override int MaxStack => 5;

        /// <summary>
        /// <see cref="BattleUnitBuf_Hail"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitBuf_Hail()
            => LoadIcon();

        public override void OnRoundEnd()
        {
            TakeDamage();

            stack--;
            if (stack <= 0)
            {
                Destroy();
            }
        }

        /// <summary>
        /// スリップ ダメージを受けます。
        /// </summary>
        private void TakeDamage()
        {
            // 特定のパッシブを保有している場合はダメージを受けない
            if (_owner.passiveDetail.HasPassive<PassiveAbility_2270016>()) { return; }
            if (_owner.passiveDetail.HasPassive<PassiveAbility_2270017>()) { return; }

            const int MinDamage = 1;
            const int MaxDamage = 5;

            int num = _owner.MaxHp / 16;
            num = Mathf.Clamp(num, MinDamage, MaxDamage);
            _owner.TakeDamage(num, DamageType.Buf, _owner, KeywordBuf.None);
        }
    }
}
