using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers.StaticInfo
{
    /// <summary>
    /// StaticInfo/EquipPage に定義されたコア ページの XML 情報です。
    /// </summary>
    public static class EquipPage
    {
        /// <summary>規定の敵キャラクターのコア ページ (コア ページ数値 ID: 0)</summary>
        public static readonly BookXmlInfo DefaultEnemyBook = new BookXmlInfo()
        {
            _id = 0,
            EquipEffect = new BookEquipEffect()
            {
                Hp = 50,
                Break = 25,
                SpeedMin = 2,
                Speed = 5,
            }
        };

        /// <summary>規定の味方キャラクターのコア ページ (コア ページ数値 ID: 31)</summary>
        public static readonly BookXmlInfo DefaultPlayerBook = new BookXmlInfo()
        {
            _id = 31,
            EquipEffect = new BookEquipEffect()
            {
                Hp = 30,
                Break = 20,
                SpeedMin = 1,
                Speed = 4,
            }
        };

        /// <summary>
        /// 敵キャラクターの規定のコア ページ数値 ID を取得します。
        /// </summary>
        public static int DefaultEnemyBookId => DefaultEnemyBook._id;

        /// <summary>
        /// 味方キャラクターの規定のコア ページ数値 ID を取得します。
        /// </summary>
        public static int DefaultPlayerBookId => DefaultPlayerBook._id;

        /// <summary>
        /// コア ページの XML 情報リストを取得します。
        /// </summary>
        /// <returns></returns>
        public static List<BookXmlInfo> GetBooks()
            => new List<BookXmlInfo>()
            {
                DefaultEnemyBook,
                DefaultPlayerBook,
            };
    }
}
