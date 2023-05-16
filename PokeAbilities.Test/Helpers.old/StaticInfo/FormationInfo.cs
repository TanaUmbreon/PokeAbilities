using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAbilities.Test.Helpers.StaticInfo
{
    /// <summary>
    /// StaticInfo/FormationInfo に定義されたフォーメーションの XML 情報です。
    /// </summary>
    public static class FormationInfo
    {
        /// <summary>規定のフォーメーション位置</summary>
        public static readonly FormationPositionXmlData DefaultPosition = new FormationPositionXmlData()
        {
            name = "",
            vector = new XmlVector2() { x = 0, y = 0 }
        };
    }
}
