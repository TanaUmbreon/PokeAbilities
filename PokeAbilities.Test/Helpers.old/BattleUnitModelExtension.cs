using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeAbilities.Test.Helpers
{
    public static class BattleUnitModelExtension
    {
        /// <summary>
        /// <see cref="BattleObjectManager.CreateDefaultUnit(Faction)"/> メソッドを疑似的に再現し、
        /// <see cref="BattleUnitModel"/> のインスタンスを生成します。
        /// </summary>
        /// <param name="faction">敵または味方の派閥。</param>
        /// <returns></returns>
        public static BattleUnitModel ImitateCreateDefaultUnit(int id, Faction faction)
            => new BattleUnitModel(id) { faction = faction };

        /// <summary>
        /// <see cref="BattleUnitModel.SetUnitData(UnitBattleDataModel)"/> メソッドを疑似的に再現し、
        /// 指定したバトル データを設定します。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="unitBattleData"></param>
        public static void ImitiateSetUnitData(this BattleUnitModel model, UnitBattleDataModel unitBattleData)
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }
            if (unitBattleData == null) { throw new ArgumentNullException(nameof(unitBattleData)); }

            PrivateAccess.SetField(model, "_unitData", unitBattleData);
            model.equipment.SetUnitData(unitBattleData.unitData);
            unitBattleData.emotionDetail.SetUnit(model);

            if (unitBattleData.unitData.EnemyUnitId != -1)
            {
                List<DiceCardXmlInfo> deck = unitBattleData.unitData.GetDeck();
                if (deck != null)
                {
                    model.allyCardDetail.Init(deck);
                }
            }
            else
            {
                List<DiceCardXmlInfo> deckForBattle = unitBattleData.unitData.GetDeckForBattle(0);
                if (deckForBattle != null)
                {
                    model.allyCardDetail.Init(deckForBattle);
                }
            }
            model.personalEgoDetail.Init();
            model.passiveDetail.Init();
        }
    }
}
