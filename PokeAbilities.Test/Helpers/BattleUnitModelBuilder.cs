using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// バトル キャラクターのインスタンスを構築します。
    /// </summary>
    public class BattleUnitModelBuilder
    {
        /// <summary>
        /// キャラクター ID を取得または設定します。
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// キャラクターが装着しているコア ページを取得または設定します。
        /// null の場合、既定のコア ページを構築します。
        /// </summary>
        public BookXmlInfo EquipBook { get; set; } = null;

        /// <summary>
        /// キャラクターが保有するパッシブのコレクションを取得または設定します。
        /// nullの場合、パッシブを保有しません。
        /// </summary>
        public IEnumerable<PassiveAbilityBase> Passives { get; set; } = null;

        /// <summary>
        /// キャラクターの派閥を取得または設定します。
        /// </summary>
        public Faction Faction { get; set; } = Faction.Enemy;

        /// <summary>
        /// キャラクターが死亡している事を表す値を取得または設定します。
        /// </summary>
        public bool IsDie { get; set; } = false;

        /// <summary>
        /// 現在使用しているバトル ページのデータを取得または設定します。
        /// </summary>
        public BattlePlayingCardDataInUnitModel CurrentDiceAction { get; set; } = null;

        /// <summary>
        /// デッキの 1 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard1 { get; set; } = null;

        /// <summary>
        /// デッキの 2 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard2 { get; set; } = null;

        /// <summary>
        /// デッキの 3 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard3 { get; set; } = null;

        /// <summary>
        /// デッキの 4 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard4 { get; set; } = null;

        /// <summary>
        /// デッキの 5 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard5 { get; set; } = null;

        /// <summary>
        /// デッキの 6 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard6 { get; set; } = null;

        /// <summary>
        /// デッキの 7 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard7 { get; set; } = null;

        /// <summary>
        /// デッキの 8 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard8 { get; set; } = null;

        /// <summary>
        /// デッキの 9 枚目のバトル ページを取得または設定します。
        /// nullの場合、既定のバトル ページを構築します。
        /// </summary>
        public BattleDiceCardModel DeckCard9 { get; set; } = null;

        /// <summary>
        /// <see cref="BattleUnitModelBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitModelBuilder() { }


        /// <summary>
        /// 現在設定さている情報から、バトル キャラクターのインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BattleUnitModel ToBattleUnitModel()
        {
            var model = new BattleUnitModel(Id)
            {
                faction = Faction,
                currentDiceAction = CurrentDiceAction,
            };

            InitEquipBook(model);
            model.allyCardDetail = ToBattleAllyCardDetail(model);

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
            var dictionary = new Dictionary<int, BookXmlInfo>();
            dictionary.Add(EquipBook.id, EquipBook);
            PrivateAccess.SetField(bookInfo, "_dictionary", dictionary);

            var stage = new StageModel();
            var data = new UnitDataModel(EquipBook.id);
            var unitData = new UnitBattleDataModel(stage, data);
            PrivateAccess.SetField(model, "_unitData", unitData);
            model.equipment.SetUnitData(data);

            // BattleUnitModel.OnDispose() で行う処理の一部
            model.SetHp((int)model.UnitData.hp);
            model.ResetBreakGauge();
            model.RecoverBreakLife(model.MaxBreakLife, true);
        }

        /// <summary>
        /// 現在設定さている情報から、デッキおよび手札の詳細のインスタンスを構築して返します。
        /// </summary>
        /// <param name="owner">生成するデッキおよび手札の詳細の所有キャラクター。</param>
        /// <returns></returns>
        private BattleAllyCardDetail ToBattleAllyCardDetail(BattleUnitModel owner)
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
            var deck = new[]
            {
                DeckCard1,
                DeckCard2,
                DeckCard3,
                DeckCard4,
                DeckCard5,
                DeckCard6,
                DeckCard7,
                DeckCard8,
                DeckCard9,
            };

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
            };
            card.AddDiceBehaviour();
            return card.ToBattleDiceCardModel();
        }
    }
}
