//using System.Collections.Generic;
//using LOR_DiceSystem;
//using PokeAbilities.Test.Helpers.Imitators;
//using PokeAbilities.Test.Helpers.StaticInfo;

//namespace PokeAbilities.Test.Helpers.Builders
//{
//    /// <summary>
//    /// 敵キャラクターのインスタンスを構築します。
//    /// </summary>
//    public class EnemyBattleUnitModelBuilder
//    {
//        /// <summary>
//        /// 敵キャラクター ID を取得または設定します。
//        /// <see cref="EnemyUnitInfo"/> に定義された敵キャラクター数値 ID である必要があります。
//        /// 既定値は <see cref="EnemyUnitInfo.DefaultEnemyId"/> です。
//        /// </summary>
//        public int Id { get; set; } = EnemyUnitInfo.DefaultEnemyId;

//        /// <summary>
//        /// 所属する派閥に配置されているキャラクターの順番を取得または設定します。
//        /// 既定値は 0 です。
//        /// </summary>
//        public int Index { get; set; } = 0;

//        /// <summary>
//        /// 装着するコア ページ含まれるパッシブ以外に追加で保有するパッシブのコレクションを取得または設定します。
//        /// null の場合、パッシブを追加で保有しません。
//        /// </summary>
//        public IEnumerable<PassiveAbilityBase> AdditionalPassives { get; set; } = null;

//        /// <summary>
//        /// キャラクターが死亡している事を表す値を取得または設定します。
//        /// 既定値は false です。
//        /// </summary>
//        public bool IsDie { get; set; } = false;

//        /// <summary>
//        /// デッキのバトル ページを構築するオブジェクトのコレクションを取得または設定します。
//        /// nullの場合、既定のバトル ページを構築します。
//        /// </summary>
//        public IEnumerable<DiceCardXmlInfo> Deck { get; set; } = new DiceCardXmlInfo[] { };

//        /// <summary>
//        /// <see cref="EnemyBattleUnitModelBuilder"/> の新しいインスタンスを生成します。
//        /// </summary>
//        public EnemyBattleUnitModelBuilder() { }

//        /// <summary>
//        /// 現在設定さている情報から、敵キャラクターのインスタンスを構築して返します。
//        /// </summary>
//        /// <returns></returns>
//        public BattleUnitModel Build()
//        {
//            UnitBattleDataModel battleData = CreateUnitBattleDataModel();
//            BattleUnitModel unit = StageControllerImitator.ImitateCreateEnemyUnit(battleData, Index, Id);

//            if (IsDie)
//            {
//                unit.DieFake();
//            }
//            if (AdditionalPassives != null)
//            {
//                foreach (var passive in AdditionalPassives)
//                {
//                    passive.Init(unit);
//                    unit.passiveDetail.AddPassive(passive);
//                    unit.passiveDetail.OnCreated();
//                }
//            }

//            return unit;
//        }

//        private UnitBattleDataModel CreateUnitBattleDataModel()
//        {
//            var stage = new StageModel();
//            var id = new LorId(Id);
//            Thrower.ThrowIfEnemyUnitNotFound(id);
//            return UnitBattleDataModel.CreateUnitBattleDataByEnemyUnitId(stage, id);
//        }
//    }
//}
