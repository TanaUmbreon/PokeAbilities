using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LOR_DiceSystem;
using PokeAbilities.Test.Helpers.Imitators;

namespace PokeAbilities.Test.Helpers.Builders
{
    /// <summary>
    /// バトル ページのインスタンスを構築します。
    /// </summary>
    public class BattleDiceCardModelBuilder
    {
		/// <summary>
		/// バトル ページ効果を取得または設定します。
		/// null の場合、バトル ページ効果を持ちません。
		/// </summary>
		public DiceCardSelfAbilityBase Script { get; set; } = null;

		/// <summary>
		/// バトル ダイスの振る舞いの構築オブジェクトのコレクションを取得または設定します。
		/// null の場合、規定のインスタンスを一つ使用します。
		/// </summary>
		public IEnumerable<BattleDiceBehaviorBuilder> DiceBehaviourList { get; set; } = null;

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
		/// 現在設定さている情報から、バトル ページ XML 情報のインスタンスを構築して返します。
		/// </summary>
		/// <returns></returns>
		public BattleDiceCardModel Build(BattleUnitModel owner)
        {
			if (owner == null) { throw new ArgumentNullException(nameof(owner)); }

			string scriptName = null;
			if (Script != null)
            {
				Match m = Regex.Match(Script.GetType().Name, "^[^_]+?_([^_]+)$");
				if (!m.Success) { throw new typeexce}
            }
			var cardInfo = new DiceCardXmlInfo()
			{
				Script = scriptName,
				Spec = new DiceCardSpec()
				{
					Cost = Cost,
					Ranged = Ranged,
					affection = Affection,
				},
			};

			if (OptionList != null)
            {
				cardInfo.optionList.AddRange(OptionList);
            }

			var diceBuilders = DiceBehaviourList ?? new[] { new BattleDiceBehaviorBuilder() };
			cardInfo.DiceBehaviourList.AddRange(diceBuilders.Select(b => b.CreateDiceBehaviour()));

			var card = BattleDiceCardModelImitator.ImitateCreatePlayingCard(cardInfo);
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
    }
}
