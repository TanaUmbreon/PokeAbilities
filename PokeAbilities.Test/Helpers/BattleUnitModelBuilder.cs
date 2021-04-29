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
        /// キャラクターの派閥を取得または設定します。
        /// </summary>
        public Faction Faction { get; set; } = Faction.Enemy;

        /// <summary>
        /// キャラクターが死亡している事を表す値を取得または設定します。
        /// </summary>
        public bool IsDie { get; set; } = false;

        /// <summary>
        /// キャラクターの現在体力を取得または設定します。
        /// </summary>
        public int Hp { get; set; } = 20;

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
            model.allyCardDetail = CreateBattleAllyCardDetail(model);
            model.equipment.book = CreateBookModel();
            model.SetHp(Hp);
            if (IsDie)
            {
                model.DieFake();
            }

            return model;
        }

        private BattleAllyCardDetail CreateBattleAllyCardDetail(BattleUnitModel target)
        {
            var c = new BattleAllyCardDetail(target);
            PrivateAccess.SetField(c, "_cardInDeck", CreateDeck());
            PrivateAccess.SetField(c, "_cardInHand", new List<BattleDiceCardModel>());
            PrivateAccess.SetField(c, "_cardInUse", new List<BattleDiceCardModel>());
            PrivateAccess.SetField(c, "_cardInDiscarded", new List<BattleDiceCardModel>());
            PrivateAccess.SetField(c, "_cardInReserved", new List<BattleDiceCardModel>());
            return c;
        }

        private List<BattleDiceCardModel> CreateDeck()
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

            var defaultCard = new BattleDiceCardModelBuilder();
            defaultCard.AddDiceBehaviour();

            var list = new List<BattleDiceCardModel>();
            foreach (BattleDiceCardModel card in deck)
            {
                list.Add(card ?? defaultCard.ToBattleDiceCardModel());
            }
            return list;
        }

        private BookModel CreateBookModel()
        {
            var bookXml = new BookXmlInfo();
            var book = new BookModel();
            PrivateAccess.SetField(book, "_classInfo", bookXml);
            return book;
        }
    }
}
