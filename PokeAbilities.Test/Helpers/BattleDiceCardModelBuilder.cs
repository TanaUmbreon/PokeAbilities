﻿using System.Collections.Generic;
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
            return BattleDiceCardModel.CreatePlayingCard(cardInfo);
        }
    }
}