using LOR_DiceSystem;
using System.Collections.Generic;
using System;
using PokeAbilities.Test.Helpers.Imitators;
using UnityEngine;
using PokeAbilities.Test.Helpers.Builders;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// キャラクターのインスタンスを構築します。
    /// </summary>
    public class BattleUnitModelBuilder
    {
        /// <summary>
        /// キャラクター ID を取得または設定します。
        /// 既定値は 0 です。
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 所属する派閥に配置されているバトル キャラクターの順番を取得または設定します。
        /// 既定値は 0 です。
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// キャラクターが装着しているコア ページを取得または設定します。
        /// null の場合、既定のコア ページを構築します。
        /// </summary>
        public BookXmlInfo EquipBook { get; set; } = null;

        /// <summary>
        /// キャラクターが保有するパッシブのコレクションを取得または設定します。
        /// null の場合、パッシブを保有しません。
        /// </summary>
        public IEnumerable<PassiveAbilityBase> Passives { get; set; } = null;

        /// <summary>
        /// キャラクターの派閥を取得または設定します。
        /// 既定値は <see cref="Faction.Player"/> です。
        /// </summary>
        public Faction Faction { get; set; } = Faction.Player;

        /// <summary>
        /// キャラクターが死亡している事を表す値を取得または設定します。
        /// 既定値は false です。
        /// </summary>
        public bool IsDie { get; set; } = false;

        /// <summary>
        /// デッキのバトル ページを構築するオブジェクトのコレクションを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public IEnumerable<BattleDiceCardModelBuilder> DeckCards { get; set; } = null;

        /// <summary>
        /// <see cref="BattleUnitModelBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitModelBuilder() { }

        /// <summary>
        /// 現在設定さている情報から、キャラクターのインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BattleUnitModel Build()
        {
            UnitBattleDataModel battleData = CreateUnitBattleDataModel();

            // 以下のメソッドを模倣する
            // - StageController.CreateEnemyUnit(UnitBattleDataModel, int) : BattleUnitModel
            // - StageController.CreateLibrarianUnit(SephirahType, UnitBattleDataModel, int) : BattleUnitModel

            BattleUnitModel unit = BattleObjectManagerImitator.ImitateCreateDefaultUnit(Faction, Id);
            unit.index = Index;
            unit.formation = CreateFormationPosition(); // model.formation = floor.GetFormationPosition(battleUnitModel.index);

            // 敵キャラクターで死亡している場合はここでmodelを早期returnするが、
            // 現状だとテストに不要なのでそこまで厳密に再現しない

            unit.grade = UnitGradeType.Normal; // model.grade = unitData.grade;
            unit.SetUnitData(battleData);
            unit.OnCreated();
            unit.passiveDetail.OnUnitCreated();

            return unit;
            /*
            var model = BattleUnitModelExtension.ImitateCreateDefaultUnit(Id, Faction);

            InitEquipBook(model);
            model.allyCardDetail = ImitateInit(model);

            if (IsDie)
            {
                model.DieFake();
            }
            if (Passives != null)
            {
                foreach (var passive in Passives)
                {
                    passive.Init(model);
                    model.passiveDetail.AddPassive(passive);
                    model.passiveDetail.OnCreated();
                }
            }

            return model;
            */
        }

        private UnitBattleDataModel CreateUnitBattleDataModel()
        {
            var stage = new StageModel();

            if (Faction == Faction.Enemy)
            {
                var id = new LorId(Id);
                if (Singleton<EnemyUnitClassInfoList>.Instance.GetData(id) == null)
                {
                    throw new InvalidOperationException($"EnemyUnitClassInfoList に ID '{id.packageId}{id.id}' の敵キャラクター情報が登録されていません。");
                }
                return UnitBattleDataModel.CreateUnitBattleDataByEnemyUnitId(stage, id);
            }

            if (EquipBook == null)
            {
                EquipBook = new BookXmlInfoBuilder().ToBookXmlInfo();
            }
            var data = new UnitDataModel(EquipBook.id);
            return new UnitBattleDataModel(stage, data);
        }

        private FormationPosition CreateFormationPosition()
            => new FormationPosition(new FormationPositionXmlData()
                {
                    name = "",
                    vector = new XmlVector2(),
                });

        /*
        /// <summary>
        /// <see cref="BattleUnitModel.SetUnitData(UnitBattleDataModel)"/> メソッドを疑似的に再現し、
        /// 指定したキャラクターに指定したバトル データを設定します。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="unitBattleData"></param>
        private static void ImitiateSetUnitData(BattleUnitModel model, UnitBattleDataModel unitBattleData)
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
                    this.allyCardDetail.Init(deck);
                }
            }
            else
            {
                List<DiceCardXmlInfo> deckForBattle = unitBattleData.unitData.GetDeckForBattle(0);
                if (deckForBattle != null)
                {
                    this.allyCardDetail.Init(deckForBattle);
                }
            }
            this.personalEgoDetail.Init();
            this.passiveDetail.Init();
        }

        private void InitEquipBook(BattleUnitModel model)
        {
            if (EquipBook == null)
            {
                EquipBook = new BookXmlInfoBuilder().ToBookXmlInfo();
            }

            // ロードされたコアページの一覧を初期化して追加する
            // (UnitDataModelのコンストラクタで参照してそこからコアページの設定をしている為。
            //  都度の初期化はテストケース毎に同じIDで異なる性能のコアページを使用できるようにする為)
            BookXmlList bookInfo = Singleton<BookXmlList>.Instance;
            var dictionary = new Dictionary<LorId, BookXmlInfo>();
            dictionary.Add(EquipBook.id, EquipBook);
            PrivateAccess.SetField(bookInfo, "_dictionary", dictionary);

            var stage = new StageModel();
            var data = new UnitDataModel(EquipBook.id);
            var unitData = new UnitBattleDataModel(stage, data);
            //-/PrivateAccess.SetField(model, "_unitData", unitData);
            //-/model.equipment.SetUnitData(data);

            // BattleUnitModel.OnDispose() で行う処理の一部
            model.SetHp((int)model.UnitData.hp);
            model.ResetBreakGauge();
            model.RecoverBreakLife(model.MaxBreakLife, true);
        }

        /// <summary>
        /// <see cref="BattleAllyCardDetail.Init(List{DiceCardXmlInfo})"/> メソッドを疑似的に再現し、
        /// <see cref="BattleAllyCardDetail"/> のインスタンスを生成します。
        /// </summary>
        /// <param name="owner">生成するデッキおよび手札の詳細の所有キャラクター。</param>
        /// <returns></returns>
        private BattleAllyCardDetail ImitateInit(BattleUnitModel owner)
        {
            var c = new BattleAllyCardDetail(owner);
            PrivateAccess.SetField(c, "_cardInDeck", ToCardInDeck(owner));
            PrivateAccess.SetField(c, "_cardInHand", new List<BattleDiceCardModel>());
            PrivateAccess.SetField(c, "_cardInUse", new List<BattleDiceCardModel>());
            PrivateAccess.SetField(c, "_cardInDiscarded", new List<BattleDiceCardModel>());
            PrivateAccess.SetField(c, "_cardInReserved", new List<BattleDiceCardModel>());
            return c;
        }

        /// <summary>
        /// 現在設定さている情報から、デッキ用バトル ページのインスタンスを構築して返します。
        /// </summary>
        /// <param name="owner">生成するデッキ用バトル ページの所有キャラクター。</param>
        /// <returns></returns>
        private List<BattleDiceCardModel> ToCardInDeck(BattleUnitModel owner)
        {
            var result = new List<BattleDiceCardModel>();
            foreach (BattleDiceCardModel card in deck)
            {
                if (card == null)
                {
                    result.Add(CreateDefaultBattleDiceCardModel(owner));
                    continue;
                }

                card.owner = owner;
                result.Add(card);
            }
            return result;
        }

        /// <summary>
        /// 規定のバトル ページを生成します。
        /// </summary>
        /// <param name="owner">生成するバトル ページの所有キャラクター。</param>
        /// <returns></returns>
        private BattleDiceCardModel CreateDefaultBattleDiceCardModel(BattleUnitModel owner)
        {
            var card = new BattleDiceCardModelBuilder()
            {
                Owner = owner,
                CardInfo = new DiceCardXmlInfo()
                {
                    DiceBehaviourList = new List<DiceBehaviour>()
                    {
                        new DiceBehaviour()
                        {
                            Min = 1,
                            Dice = 1,
                        }
                    }
                }
            };
            return card.ToBattleDiceCardModel();
        }
        */
    }
}
