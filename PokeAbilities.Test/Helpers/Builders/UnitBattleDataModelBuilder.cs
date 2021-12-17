using System.Collections.Generic;

namespace PokeAbilities.Test.Helpers.Builders
{
    /// <summary>
    /// キャラクターの戦闘データのインスタンスを構築します。
    /// </summary>
    public class UnitBattleDataModelBuilder
    {
        /// <summary>
        /// 所属する派閥を取得または設定します。
        /// 既定値は味方です。
        /// </summary>
        public Faction Faction { get; set; } = Faction.Player;

        /// <summary>
        /// 所属階層を取得または設定します。
        /// 既定値は <see cref="SephirahType.None"/> です。
        /// また、 <see cref="Faction.Enemy"/> の場合は設定された値が無視されて <see cref="SephirahType.None"/> 固定となります。
        /// </summary>
        public SephirahType SephirahType { get; set; } = SephirahType.None;

        /// <summary>
        /// 指定司書であることを示す値を取得または設定します。
        /// 既定値は司書 (指定司書でない) です。
        /// また、 <see cref="Faction.Enemy"/> の場合は使用されません。
        /// </summary>
        public bool IsSephirahChar { get; set; } = false;

        /// <summary>
        /// 装着するコア ページ情報の構築オブジェクトを取得または設定します。
        /// null の場合、規定のインスタンスを使用します。
        /// </summary>
        public BookXmlInfoBuilder EquipBook { get; set; } = null;

        /// <summary>
        /// 現在設定さている情報から <see cref="UnitBattleDataModel"/> のインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public UnitBattleDataModel Build()
        {
            // UnitDataModelのコンストラクタでは、コアページIDからBookXmlListに登録されたコアページXML情報を取得している
            // BookXmlListにコアページの登録がない場合は例外が発生するので都度初期化している
            // (UnitDataModelのコンストラクタを呼び出す時にのみBookXmlListが必要となるので、それ以降は初期化して問題がない)
            BookXmlInfo equipBook = (EquipBook ?? new BookXmlInfoBuilder()).Build();
            Singleton<BookXmlList>.Instance.Init(new List<BookXmlInfo>() { equipBook });

            var data = (Faction == Faction.Player) ?
                new UnitDataModel(equipBook.id, SephirahType, IsSephirahChar) :
                new UnitDataModel(equipBook.id, SephirahType.None, false);

            if (Faction == Faction.Enemy)
            {
                var classInfo = new EnemyUnitClassInfo()
                {
                    bookId = new List<int>() { equipBook._id },
                };
                data.SetByEnemyUnitClassInfo(classInfo);
            }

            var stage = new StageModel();
            return new UnitBattleDataModel(stage, data);
        }
    }
}
