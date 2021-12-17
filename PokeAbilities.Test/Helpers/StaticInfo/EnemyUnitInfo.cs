using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers.StaticInfo
{
    /// <summary>
    /// StaticInfo/EnemyUnitInfo に定義された敵キャラクターの XML 情報です。
    /// </summary>
    public static class EnemyUnitInfo
    {
        /// <summary>規定の敵キャラクター (敵キャラクター数値 ID: 0, 装着コア ページ ID: 0)</summary>
        public static readonly EnemyUnitClassInfo DefaultEnemy = new EnemyUnitClassInfo()
        {
            _id = 0,
            bookId = new List<int>() { EquipPage.DefaultEnemyBookId },
        };

        /// <summary>
        /// 規定の敵キャラクター数値 ID を取得します。
        /// </summary>
        public static int DefaultEnemyId => DefaultEnemy._id;

        /// <summary>
        /// 敵キャラクターの XML 情報リストを取得します。
        /// </summary>
        /// <returns></returns>
        public static List<EnemyUnitClassInfo> GetEnemies()
            => new List<EnemyUnitClassInfo>()
            {
                DefaultEnemy,
            };
    }
}
