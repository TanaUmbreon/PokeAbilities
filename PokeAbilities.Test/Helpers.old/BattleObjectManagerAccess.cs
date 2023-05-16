using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// ユニット テスト上で動作可能な <see cref="BattleObjectManager"/> の操作を提供するユーティリティ クラスです。
    /// </summary>
    public static class BattleObjectManagerAccess
    {
        /// <summary>
        /// ユニット テストで動作するように <see cref="BattleObjectManager.RegisterUnit(BattleUnitModel)"/> を呼び出します。
        /// </summary>
        /// <param name="unit"></param>
        public static void RegisterUnit(BattleUnitModel unit)
        {
            unit.Book.SetOriginalResists();
            var unitList = PrivateAccess.GetField<List<BattleUnitModel>>(BattleObjectManager.instance, "_unitList");
            unitList.Add(unit);
        }
    }
}
