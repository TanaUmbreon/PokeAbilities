using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers.Builders
{
    /// <summary>
    /// バトル ページのインスタンスを構築します。
    /// </summary>
    public class BattleDiceCardModelBuilder
    {
		/// <summary>
		/// バトル ページ効果の名前を取得または設定します。
		/// null の場合、バトル ページ効果を持ちません。
		/// </summary>
		public string Script { get; set; } = null;

		/// <summary>
		/// ダイスの振る舞い構築オブジェクトのコレクションを取得または設定します。
		/// null の場合、ダイスなしとなります。
		/// </summary>
		public IEnumerable<DiceBehaviourBuilder> DiceBehaviourList { get; set; } = null;

		/// <summary>
		/// バトル ページが保有するバトル ページ状態のコレクションを取得または設定します。
		/// null の場合、バトル ページ状態を保有しません。
		/// </summary>
		public IEnumerable<BattleDiceCardBuf> Bufs { get; set; } = null;

		/// <summary>
		/// コストを取得または設定します。
		/// 既定値は 0 です。
		/// </summary>
		public int Cost { get; set; } = 0;

		/// <summary>
		/// 攻撃範囲を取得または設定します。
		/// 既定値は近接攻撃です。
		/// </summary>
		public CardRange Ranged { get; set; } = CardRange.Near;

		/// <summary>
		/// 使用対象を取得または設定します。
		/// 既定値は一人です。
		/// </summary>
		public CardAffection Affection { get; set; } = CardAffection.One;

		/// <summary>
		/// バトルページのオプションのコレクションを取得または設定します。
		/// null の場合、何もオプションを指定しません。
		/// </summary>
		public IEnumerable<CardOption> OptionList = null;

		/// <summary>
		/// 現在設定さている情報から、バトル ページのインスタンスを構築して返します。
		/// </summary>
		/// <returns></returns>
		public BattleDiceCardModel Build(BattleUnitModel owner)
        {
			if (owner == null) { throw new ArgumentNullException(nameof(owner)); }

			var cardInfo = CreateDiceCardXmlInfo();
			var card = BattleDiceCardModel.CreatePlayingCard(cardInfo);
			card.owner = owner;

			if (Bufs != null)
            {
				foreach (BattleDiceCardBuf buf in Bufs)
                {
					card.AddBuf(buf);
                }
            }

			return card;
		}

		private DiceCardXmlInfo CreateDiceCardXmlInfo()
        {
			var cardInfo = new DiceCardXmlInfo()
			{
				Script = Script,
				Spec = new DiceCardSpec()
				{
					Cost = Cost,
					Ranged = Ranged,
					affection = Affection,
				},
			};

			if (DiceBehaviourList != null)
			{
				cardInfo.DiceBehaviourList.AddRange(DiceBehaviourList.Select(b => b.Build()));
			}

			if (OptionList != null)
			{
				cardInfo.optionList.AddRange(OptionList);
			}

			return cardInfo;
		}
	}
}
