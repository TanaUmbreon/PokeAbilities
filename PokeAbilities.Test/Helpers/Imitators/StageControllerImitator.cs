using System.Collections.Generic;
using LOR_DiceSystem;
using PokeAbilities.Test.Helpers.StaticInfo;

namespace PokeAbilities.Test.Helpers.Imitators
{
    /// <summary>
    /// <see cref="StageControllerImitator"/> クラスのメソッドを模倣し、疑似的に再現します。
    /// </summary>
    internal static class StageControllerImitator
    {
        /// <summary>
        /// <see cref="StageController.CreateEnemyUnit(UnitBattleDataModel, int)"/> メソッドを疑似的に再現し、
        /// 敵キャラクターを生成します。
        /// </summary>
        /// <param name="unitBattleData"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BattleUnitModel ImitateCreateEnemyUnit(UnitBattleDataModel unitBattleData, int index, int id)
        {
            // unit.formationフィールドの設定で使われているが、既定値を代わりに設定するので不要
            // StageWaveModel currentWave = GetCurrentWaveModel();

            // ワーク変数として参照を保持するまでもないので不要
            // UnitDataModel unitData = unitBattleData.unitData;
            
            BattleUnitModel unit = BattleObjectManagerImitator.ImitateCreateDefaultUnit(Faction.Enemy, id); // BattleUnitModel unit = BattleObjectManager.CreateDefaultUnit(Faction.Enemy);
            unit.index = index;
            unit.formation = CreateDefaultFormationPosition(); // unit.formation = currentWave.GetFormationPosition(battleUnitModel.index);

            if (unitBattleData.isDead) { return unit; }
            
            unit.grade = unitBattleData.unitData.grade; // unit.grade = unitData.grade;
            unit.ImitateSetUnitData(unitBattleData); // unit.SetUnitData(unitBattleData);
            unit.OnCreated();
            
            // 現状不要
            // _enemyTeam.AddUnit(unit);
            // BattleObjectManager.instance.RegisterUnit(unit);
            
            unit.passiveDetail.OnUnitCreated();
            return unit;
        }

        /// <summary>
        /// <see cref="StageController.CreateLibrarianUnit(SephirahType, UnitBattleDataModel, int)"/> メソッドを疑似的に再現し、
        /// 味方キャラクターを生成します。
        /// </summary>
        /// <param name="sephirah"></param>
        /// <param name="battleData"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BattleUnitModel ImitateCreateLibrarianUnit(SephirahType sephirah, UnitBattleDataModel battleData, int index, int id)
        {
            // unit.formationフィールドの設定で使われているが、既定値を代わりに設定するので不要
            // StageLibraryFloorModel floor = _stageModel.GetFloor(sephirah);

            // ワーク変数として参照を保持するまでもないので不要
            // UnitDataModel unitData = battleData.unitData;

            BattleUnitModel unit = BattleObjectManagerImitator.ImitateCreateDefaultUnit(Faction.Player, id); // BattleUnitModel unit = BattleObjectManager.CreateDefaultUnit(Faction.Player);
            unit.index = index;
            unit.grade = battleData.unitData.grade; // unit.grade = unitData.grade;
            unit.formation = CreateDefaultFormationPosition(); // unit.formation = floor.GetFormationPosition(unit.index);
            unit.ImitateSetUnitData(battleData); // unit.SetUnitData(battleData);
            unit.OnCreated();

            // 現状不要
            // _librarianTeam.AddUnit(unit);
            // BattleObjectManager.instance.RegisterUnit(unit);

            unit.passiveDetail.OnUnitCreated();
            return unit;
        }

        /// <summary>
        /// 規定のフォーメーション位置を生成します。
        /// </summary>
        /// <returns></returns>
        private static FormationPosition CreateDefaultFormationPosition()
            => new FormationPosition(FormationInfo.DefaultPosition);
    }
}
