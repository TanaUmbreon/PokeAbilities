using System;
using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers.Imitators
{
    /// <summary>
    /// <see cref="BattleDiceCardModel"/> クラスのメソッドを模倣し、疑似的に再現します。
    /// </summary>
    internal static class BattleDiceCardModelImitator
    {
        /// <summary>
        /// <see cref="BattleDiceCardModel.CreatePlayingCard(DiceCardXmlInfo)"/> メソッドを疑似的に再現し、
        /// 指定したバトル ページの XML 情報からバトル ページのインスタンスを生成します。
        /// </summary>
        /// <param name="cardInfo"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static BattleDiceCardModel ImitateCreatePlayingCard(DiceCardXmlInfo cardInfo)
        {
            if (cardInfo == null) { throw new ArgumentNullException(nameof(cardInfo)); }

            BattleDiceCardModel card = new BattleDiceCardModel();
            PrivateAccess.SetField(card, "_xmlData", cardInfo); // card._xmlData = cardInfo.Copy(false);
            PrivateAccess.SetField(card, "_curCost", cardInfo.Spec.Cost); // card._curCost = cardInfo.Spec.Cost;
            //PrivateAccess.SetField(card, "_script", script); // card._script = card.CreateDiceCardSelfAbilityScript();
            PrivateAccess.SetField(card, "_script", card.CreateDiceCardSelfAbilityScript()); // card._script = card.CreateDiceCardSelfAbilityScript();
            return card;
        }
    }
}
