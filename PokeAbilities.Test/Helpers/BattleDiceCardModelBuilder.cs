using System.Collections.Generic;
using LOR_DiceSystem;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// バトル ページのインスタンスを構築します。
    /// Unity のリソース参照を避けるために、ユニット テスト上で使用します。
    /// </summary>
    public class BattleDiceCardModelBuilder
    {
        private readonly List<DiceBehaviour> diceBehaviourList = new List<DiceBehaviour>();

        /// <summary>
        /// バトル ページの所有キャラクターを取得または設定します。
        /// </summary>
        public BattleUnitModel Owner { get; set; }

        /// <summary>
        /// バトル ページが保有するバトル ページ状態のコレクションを取得または設定します。
        /// nullの場合、バトル ページ状態を保有しません。
        /// </summary>
        public IEnumerable<BattleDiceCardBuf> Bufs { get; set; } = null;

        /// <summary>
        /// <see cref="BattleDiceCardModelBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleDiceCardModelBuilder() { }

        /// <summary>
        /// バトル ページのダイス情報を追加します。
        /// </summary>
        public void AddDiceBehaviour()
        {
            var behaviour = new DiceBehaviour();
            
            diceBehaviourList.Add(behaviour);
        }

        /// <summary>
        /// 現在設定さている情報から、バトル ページのインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BattleDiceCardModel ToBattleDiceCardModel()
        {
            var cardInfo = new DiceCardXmlInfo()
            {
                DiceBehaviourList = diceBehaviourList,
            };
            var model = new BattleDiceCardModel()
            {
                owner = Owner,
            };
            PrivateAccess.SetField(model, "_xmlData", cardInfo.Copy(false));
            PrivateAccess.SetField(model, "_curCost", cardInfo.Spec.Cost);
            if (Bufs != null)
            {
                foreach (var buf in Bufs)
                {
                    model.AddBuf(buf);
                }
            }
            return model;
        }
    }
}
