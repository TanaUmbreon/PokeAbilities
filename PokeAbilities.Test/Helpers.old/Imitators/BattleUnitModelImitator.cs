using System.Collections.Generic;
using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers.Imitators
{
    /// <summary>
    /// <see cref="BattleUnitModel"/> クラスのメソッドを模倣し、疑似的に再現します。
    /// </summary>
    internal static class BattleUnitModelImitator
    {
		/// <summary>
		/// <see cref="BattleUnitModel.SetUnitData(UnitBattleDataModel)"/> メソッドを疑似的に再現し、
		/// 指定したキャラクターの戦闘データを設定します。
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="unitBattleData"></param>
		public static void ImitateSetUnitData(this BattleUnitModel unit, UnitBattleDataModel unitBattleData)
        {
			PrivateAccess.SetField(unit, "_unitData", unitBattleData); // unit._unitData = unitBattleData;
			unit.equipment.SetUnitData(unitBattleData.unitData);
			unitBattleData.emotionDetail.SetUnit(unit);

			// デッキの初期化はここでは行わないようにする
			// (BattleDiceCardModelの生成をBattleDiceCardModelBuilderに移譲させ、
			//  バトルページ効果とバトルダイス効果の設定も行えるようにするため)
			//if (unitBattleData.unitData.EnemyUnitId != -1)
			//{
			//	List<DiceCardXmlInfo> deck = unitBattleData.unitData.GetDeck();
			//	if (deck != null)
			//	{
			//		unit.allyCardDetail.Init(deck);
			//	}
			//}
			//else
			//{
			//	List<DiceCardXmlInfo> deckForBattle = unitBattleData.unitData.GetDeckForBattle(0);
			//	if (deckForBattle != null)
			//	{
			//		unit.allyCardDetail.Init(deckForBattle);
			//	}
			//}

			unit.personalEgoDetail.Init();
			unit.passiveDetail.Init();
		}
	}
}
