using System;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// <see cref="BattlePlayingCardDataInUnitModel"/> のインスタンスを構築します。
    /// </summary>
    public class BattlePlayingCardDataInUnitModelBuilder
    {
        /// <summary>
        /// バトル ページの対象を取得または設定します。
        /// </summary>
        public BattleUnitModel Target { get; set; }

        /// <summary>
        /// 現在のバトル ダイスの振る舞いを取得または設定します。
        /// </summary>
        public BattleDiceBehavior CurrentBehavior { get; set; }

        /// <summary>
        /// <see cref="BattlePlayingCardDataInUnitModelBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattlePlayingCardDataInUnitModelBuilder() { }

        /// <summary>
        /// 現在設定さている情報から、
        /// <see cref="BattlePlayingCardDataInUnitModel"/> のインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BattlePlayingCardDataInUnitModel ToBattlePlayingCardDataInUnitModel()
        {
            var result = new BattlePlayingCardDataInUnitModel()
            {
                target = Target,
                currentBehavior = CurrentBehavior,
            };

            if (CurrentBehavior != null)
            {
                CurrentBehavior.card = result;
            }

            return result;
        }
    }
}