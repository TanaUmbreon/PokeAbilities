//using System;
//using System.Collections.Generic;
//using LOR_DiceSystem;

//namespace PokeAbilities.Test.Helpers
//{
//    /// <summary>
//    /// バトル ページのインスタンスを構築します。
//    /// Unity のリソース参照を避けるために、ユニット テスト上で使用します。
//    /// </summary>
//    public class BattleDiceCardModelBuilder
//    {
//        /// <summary>
//        /// バトル ページの情報を取得または設定します。
//        /// </summary>
//        public DiceCardXmlInfo CardInfo { get; set; }

//        /// <summary>
//        /// バトル ページ効果を取得します。
//        /// null の場合、バトル ページ効果を保有しません。
//        /// </summary>
//        public DiceCardSelfAbilityBase Script { get; set; } = null;

//        /// <summary>
//        /// バトル ページの所有キャラクターを取得または設定します。
//        /// </summary>
//        public BattleUnitModel Owner { get; set; }

//        /// <summary>
//        /// バトル ページが保有するバトル ページ状態のコレクションを取得または設定します。
//        /// null の場合、バトル ページ状態を保有しません。
//        /// </summary>
//        public IEnumerable<BattleDiceCardBuf> Bufs { get; set; } = null;

//        /// <summary>
//        /// <see cref="BattleDiceCardModelBuilder"/> の新しいインスタンスを生成します。
//        /// </summary>
//        public BattleDiceCardModelBuilder() { }

//        /// <summary>
//        /// 現在設定さている情報から、バトル ページのインスタンスを構築して返します。
//        /// </summary>
//        /// <returns></returns>
//        public BattleDiceCardModel ToBattleDiceCardModel()
//        {
//            Thrower.ThrowIfPropertyIsNull(() => CardInfo);
//            Thrower.ThrowIfPropertyIsNull(() => Owner);

//            BattleDiceCardModel card = ImitateCreatePlayingCard();
//            card.owner = Owner;

//            if (Bufs != null)
//            {
//                foreach (var buf in Bufs)
//                {
//                    card.AddBuf(buf);
//                }
//            }

//            return card;
//        }

//        /// <summary>
//        /// <see cref="BattleDiceCardModel.CreatePlayingCard(DiceCardXmlInfo)"/> メソッドを疑似的に再現し、
//        /// <see cref="BattleDiceCardModel"/> のインスタンスを生成します。
//        /// </summary>
//        /// <returns></returns>
//        private BattleDiceCardModel ImitateCreatePlayingCard()
//        {
//            var model = new BattleDiceCardModel();
//            PrivateAccess.SetField(model, "_xmlData", CardInfo.Copy(false));
//            PrivateAccess.SetField(model, "_curCost", CardInfo.Spec.Cost);
//            PrivateAccess.SetField(model, "_script", Script);
//            return model;
//        }
//    }
//}
