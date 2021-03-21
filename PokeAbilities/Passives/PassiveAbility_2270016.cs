namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ゆきがくれ」
    /// あられ状態のとき、20%の確率でバトルページによるダメージを受けない。あられダメージを受けない。
    /// </summary>
    public class PassiveAbility_2270016 : PassiveAbilityBase
    {
        /// <summary>被ダメージ軽減量</summary>
        private int dmgReduction;
        /// <summary>疑似乱数ジェネレーター</summary>
        private readonly IRandomizer randomizer;

        /// <summary>
        /// 既定の疑似乱数ジェネレーターを使用する、
        /// <see cref="PassiveAbility_2270016"/> の新しいインスタンスを生成します。
        /// </summary>
        public PassiveAbility_2270016()
            : this(DefaultRandomizer.Instance) { }

        /// <summary>
        /// 指定した疑似乱数ジェネレーターを使用する
        /// <see cref="PassiveAbility_2270016"/> の新しいインスタンスを生成します。
        /// </summary>
        public PassiveAbility_2270016(IRandomizer randomizer) 
            => this.randomizer = randomizer;

        public override void OnCreated()
            => dmgReduction = 0;

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            dmgReduction = 0;

            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override int GetDamageReductionAll()
            => dmgReduction;

        public override int GetBreakDamageReductionAll(int dmg, DamageType dmgType, BattleUnitModel attacker)
            => dmgReduction;
    }
}
