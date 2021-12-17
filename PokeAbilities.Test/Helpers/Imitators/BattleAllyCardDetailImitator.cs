using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using PokeAbilities.Test.Helpers.Builders;

namespace PokeAbilities.Test.Helpers.Imitators
{
    /// <summary>
    /// <see cref="BattleAllyCardDetail"/> クラスのメソッドを模倣し、疑似的に再現します。
    /// </summary>
    internal static class BattleAllyCardDetailImitator
    {
		/// <summary>
		/// <see cref="BattleAllyCardDetail.Init(List{DiceCardXmlInfo})"/> メソッドを疑似的に再現し、
		/// 指定したバトル ページの構築オブジェクトのコレクションで構成されるデッキに初期化します。
		/// </summary>
		/// <param name="cardDetail"></param>
		/// <param name="deck">デッキを構成するバトル ページの構築オブジェクトのコレクション。</param>
		/// <param name="owner">バトル ページを所有するキャラクター。</param>
		public static void ImitateInit(this BattleAllyCardDetail cardDetail, IEnumerable<BattleDiceCardModelBuilder> deck, BattleUnitModel owner)
		{
			if (cardDetail == null) { throw new ArgumentNullException(nameof(cardDetail)); }
			if (deck == null) { throw new ArgumentException(nameof(deck)); }
			if (owner == null) { throw new ArgumentException(nameof(owner)); }

			// deckに含まれる要素数はここでは考慮しない。デッキ枚数は0枚にすることも9枚を超えることも可能
			var _cardInDeck = new List<BattleDiceCardModel>(deck.Select(b => b.Build(owner)));

			PrivateAccess.SetField(cardDetail, "_cardInDeck", _cardInDeck); // cardDetail._cardInDeck = new List<BattleDiceCardModel>();
			PrivateAccess.SetField(cardDetail, "_cardInHand", new List<BattleDiceCardModel>()); // cardDetail._cardInHand = new List<BattleDiceCardModel>();
			PrivateAccess.SetField(cardDetail, "_cardInDiscarded", new List<BattleDiceCardModel>()); // cardDetail._cardInDiscarded = new List<BattleDiceCardModel>();
			PrivateAccess.SetField(cardDetail, "_cardInUse", new List<BattleDiceCardModel>()); // cardDetail._cardInUse = new List<BattleDiceCardModel>();
			PrivateAccess.SetField(cardDetail, "_cardInReserved", new List<BattleDiceCardModel>()); // cardDetail._cardInReserved = new List<BattleDiceCardModel>();

			// E.G.Oスキンと背景のアセット読み込み、デッキのシャッフルは行わない
			//for (int i = 0; i < deck.Count; i++)
			//{
			//	BattleDiceCardModel battleDiceCardModel = BattleDiceCardModel.CreatePlayingCard(deck[i]);
			//	Util.PreLoadPrefab("EGOs/[Prefab]" + battleDiceCardModel.GetSkin());
			//	Util.PreLoadPrefab("CreatureMaps/CreatureMap_" + battleDiceCardModel.GetMap());
			//	battleDiceCardModel.owner = cardDetail._self;
			//	cardDetail._cardInDeck.Add(battleDiceCardModel);
			//}
			//cardDetail.Shuffle();
		}
    }
}
