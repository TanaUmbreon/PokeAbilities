using System;
using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers.Builders
{
    public class BattleDiceBehaviorBuilder
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
		/// バトル ダイス効果を取得または設定します。
		/// null の場合、バトルダイス効果を持ちません。
		/// </summary>
		public DiceCardAbilityBase Script { get; set; } = null;

		/// <summary>
		/// 威力のボーナス値が適用される前のダイス値を取得または設定します。
		/// 初期値は 1 です。
		/// </summary>
		public int DiceVanillaValue { get; set; } = 1;

		/// <summary>
		/// 威力のボーナス値を取得または設定します。
		/// 初期値は 0 です。
		/// </summary>
		public int PowerBonus { get; set; } = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public BattleDiceBehavior Build(int index)
		{
			if (Min > Dice)
			{
				throw new InvalidOperationException($"バトル ダイスの出目は「最小値≦最大値」にする必要があります。現在は最小値 {Min}、最大値 {Dice} となっています。");
			}
			if (DiceVanillaValue < Min || DiceVanillaValue > Dice)
			{
				throw new InvalidOperationException($"威力のボーナス値が適用される前のダイス値は「最小値 {Min} ～最大値 {Dice}」の範囲内にする必要があります。現在の値は {DiceVanillaValue} となっています。");
			}

			var behavior = new BattleDiceBehavior()
			{
				behaviourInCard = CreateDiceBehaviour(),
			};
			behavior.SetIndex(index);

			if (Script != null)
			{
				behavior.AddAbility(Script);
			}

			PrivateAccess.SetField(behavior, "_diceResultValue", DiceVanillaValue);

			if (PowerBonus != 0)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = PowerBonus });
			}

			return behavior;
		}

		/// <summary>
		/// 現在設定さている情報からバトル ダイスの XML 情報を生成します。
		/// </summary>
		/// <returns></returns>
		public DiceBehaviour CreateDiceBehaviour()
			=> new DiceBehaviour()
			{
				Min = Min,
				Dice = Dice,
				Type = Type,
				Detail = Detail,
			};
	}
}
