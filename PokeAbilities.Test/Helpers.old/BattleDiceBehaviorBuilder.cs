using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// <see cref="BattleDiceBehavior"/> のインスタンスを構築します。
    /// </summary>
    public class BattleDiceBehaviorBuilder
    {
        /// <summary>
        /// バトル ダイスの振る舞いの詳細を取得または設定します。
        /// 初期値は <see cref="BehaviourDetail.None"/> です。
        /// </summary>
        public BehaviourDetail Detail { get; set; } = BehaviourDetail.None;

        /// <summary>
        /// 威力のボーナス値が適用される前のダイス値を取得または設定します。
        /// 初期値は 0 です。
        /// </summary>
        public int DiceVanillaValue { get; set; } = 0;

        /// <summary>
        /// 威力のボーナス値を取得または設定します。
        /// 初期値は 0 です。
        /// </summary>
        public int PowerStatBonus { get; set; } = 0;

        /// <summary>
        /// <see cref="BattleDiceBehaviorBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleDiceBehaviorBuilder() { }

        public BattleDiceBehavior ToBattleDiceBehavior()
        {
            var behaviour = new DiceBehaviour()
            {
                Detail = Detail,
            };
            var dice = new BattleDiceBehavior()
            {
                behaviourInCard = behaviour,
            };

            PrivateAccess.SetField(dice, "_diceResultValue", DiceVanillaValue);

            if (PowerStatBonus != 0)
            {
                dice.ApplyDiceStatBonus(new DiceStatBonus() { power = PowerStatBonus });
            }

            return dice;
        }
    }
}