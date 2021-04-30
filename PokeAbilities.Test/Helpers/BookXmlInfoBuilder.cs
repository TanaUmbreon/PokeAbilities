using System;
using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// <see cref="BookXmlInfo"/> のインスタンスを構築します。
    /// </summary>
    public class BookXmlInfoBuilder
    {
        /// <summary>
        /// コア ページの ID を取得または設定します。
        /// 既定値は 31 (UnitDataModel のコンストラクタ引数の既定値) です。
        /// </summary>
        public int Id { get; set; } = 31;

        /// <summary>
        /// コア ページの体力を取得または設定します。
        /// 既定値は 30 です。
        /// </summary>
        public int Hp { get; set; } = 30;

        /// <summary>
        /// コア ページの混乱耐性を取得または設定します。
        /// 既定値は 15 です。
        /// </summary>
        public int Break { get; set; } = 15;

        /// <summary>
        /// <see cref="BookXmlInfoBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BookXmlInfoBuilder() { }

        /// <summary>
        /// 現在設定さている情報から、 <see cref="BookXmlInfo"/> のインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BookXmlInfo ToBookXmlInfo()
        {
            var equipEffect = new BookEquipEffect()
            {
                Hp = Hp,
                Break = Break,
            };

            return new BookXmlInfo()
            {
                id = Id,
                EquipEffect = equipEffect,
                CharacterSkin = new List<string>() { "KetherLibrarian", "KetherLibrarian" },
            };
        }
    }
}