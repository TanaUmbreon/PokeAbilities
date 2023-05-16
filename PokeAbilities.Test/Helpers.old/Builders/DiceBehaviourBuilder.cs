using System;
using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers.Builders
{
	/// <summary>
	/// ダイスの振る舞いのインスタンスを構築します。
	/// </summary>
    public class DiceBehaviourBuilder
	{
		/// <summary>
		/// バトル ダイスの最小値を取得または設定します。
		/// 既定値は 1 です。
		/// </summary>
		public int Min { get; set; } = 1;

		/// <summary>
		/// バトル ダイスの最大値を取得または設定します。
		/// 既定値は 4 です。
		/// </summary>
		public int Dice { get; set; } = 4;

		/// <summary>
		/// 振る舞いの種類を取得または設定します。
		/// 既定値は攻撃ダイスです。
		/// </summary>
		public BehaviourType Type { get; set; } = BehaviourType.Atk;

		/// <summary>
		/// 振る舞いの詳細を取得または設定します。
		/// 既定値は斬撃ダイスです。
		/// </summary>
		public BehaviourDetail Detail { get; set; } = BehaviourDetail.Slash;

		/// <summary>
		/// バトル ダイス効果の名前を取得または設定します。
		/// null の場合、バトル ダイス効果を持ちません。
		/// </summary>
		public string Script { get; set; } = null;

		/// <summary>
		/// 現在設定されている情報から、ダイスの振る舞いのインスタンスを構築して返します。
		/// </summary>
		/// <returns></returns>
		public DiceBehaviour Build()
		{
			if (Min > Dice)
			{
				throw new InvalidOperationException($"ダイスの出目は「最小値≦最大値」にする必要があります。現在は最小値 {Min}、最大値 {Dice} となっています。");
			}

			return new DiceBehaviour()
            {
				Min = Min,
				Dice = Dice,
				Type = Type,
				Detail = Detail,
				Script = Script,
            };
		}
	}
}
