using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAbilities.Test.Helpers.Battles
{
    /// <summary>
    /// 接待での処理を模倣します。
    /// </summary>
    public static class BattleEmulator
    {
        private static readonly StageClassInfo defaultStage = new StageClassInfo();

        /// <summary>
        /// 現在の接待情報を取得します。
        /// </summary>
        public static StageClassInfo CurrentStage { get; private set; }

        /// <summary>
        /// 接待を行っている階層を取得します。
        /// </summary>
        public static SephirahType CurrentFloor
        {
            get => StageController.Instance.CurrentFloor;
            private set => StageController.Instance.SetCurrentSephirah(value);
        }

        /// <summary>
        /// 接待処理を開始します。
        /// </summary>
        /// <param name="currentStage">接待情報。</param>
        /// <param name="currentFloor">接待を行う階層。</param>
        public static void StartBattleRoutine(StageClassInfo currentStage, SephirahType currentFloor)
        {
            if (currentStage == null) { throw new ArgumentNullException(nameof(currentStage)); }

            CurrentStage = currentStage;
            CurrentFloor = currentFloor;

            BattleObjectManager.instance.Clear();

            Singleton<StageClassInfoList>.Instance.Init(new List<StageClassInfo>() { CurrentStage });
            StageController.Instance.InitStageByInvitation(CurrentStage, new List<LorId>());
            StageController.Instance.SetCurrentSephirah(CurrentFloor);
        }

        /// <summary>
        /// 接待処理を開始します。
        /// </summary>
        /// <param name="currentFloor">接待を行う階層。</param>
        public static void StartBattleRoutine(SephirahType currentFloor = SephirahType.None)
            => StartBattleRoutine(defaultStage, currentFloor);
    }
}
