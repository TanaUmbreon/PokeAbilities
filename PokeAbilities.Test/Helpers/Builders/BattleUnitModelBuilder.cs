using System;
using System.Collections.Generic;
using System.Linq;
using PokeAbilities.Test.Helpers.Imitators;

namespace PokeAbilities.Test.Helpers.Builders
{
    /// <summary>
    /// キャラクターのインスタンスを構築します。
    /// </summary>
    public class BattleUnitModelBuilder
    {
        /// <summary>
        /// 所属する派閥を取得または設定します。
        /// 既定値は <see cref="Faction.Player"/> (味方) です。
        /// </summary>
        public Faction Faction { get; set; } = Faction.Player;

        /// <summary>
        /// 装着するコア ページ情報の構築オブジェクトを取得または設定します。
        /// null の場合、規定のインスタンスを使用します。
        /// </summary>
        public BookXmlInfoBuilder EquipBook { get; set; } = null;

        /// <summary>
        /// 保有するパッシブのコレクションを取得または設定します。
        /// null の場合、パッシブを保有しません。
        /// </summary>
        public IEnumerable<PassiveAbilityBase> Passives { get; set; } = null;

        /// <summary>
        /// デッキを構成するバトル ページの構築オブジェクトのコレクションを取得または設定します。
        /// nullの場合、デッキ容量を満たすまで規定のインスタンスを使用します。
        /// </summary>
        public IEnumerable<BattleDiceCardModelBuilder> Deck { get; set; } = null;

        /// <summary>
        /// キャラクターが死亡している事を表す値を取得または設定します。
        /// 既定値は false (生存) です。
        /// </summary>
        public bool IsDie { get; set; } = false;

        /// <summary>
        /// 所属階層を取得または設定します。
        /// 既定値は <see cref="SephirahType.None"/> (指定階層なし) です。
        /// また、 <see cref="Faction"/> プロパティの値が <see cref="Faction.Enemy"/> (敵) の場合は使用されず
        /// <see cref="SephirahType.None"/> (指定階層なし) 固定となります。
        /// </summary>
        public SephirahType SephirahType { get; set; } = SephirahType.None;

        /// <summary>
        /// 指定司書であることを示す値を取得または設定します。
        /// 既定値は false (一般司書) です。
        /// また、 <see cref="Faction.Enemy"/> (敵) の場合は使用されません。
        /// </summary>
        public bool IsSephirahChar { get; set; } = false;

        /// <summary>
        /// キャラクター ID を取得または設定します。
        /// 既定値は 0 です。
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 所属する派閥に配置されているキャラクターの順番を取得または設定します。
        /// 既定値は 0 です。
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// <see cref="BattleUnitModelBuilder"/> の新しいインスタンスを生成します。
        /// </summary>
        public BattleUnitModelBuilder() { }

        /// <summary>
        /// 現在設定さている情報から、キャラクターのインスタンスを構築して返します。
        /// </summary>
        /// <returns></returns>
        public BattleUnitModel Build()
        {
            UnitBattleDataModel battleData = new UnitBattleDataModelBuilder()
            {
                Faction = Faction,
                SephirahType = SephirahType,
                IsSephirahChar = IsSephirahChar,
                EquipBook = EquipBook,
            }.Build();

            Singleton<StageController>.Instance.SetCurrentSephirah(SephirahType);
            BattleUnitModel unit = (Faction == Faction.Player) ?
                StageControllerImitator.ImitateCreateLibrarianUnit(SephirahType, battleData, Index, Id) :
                StageControllerImitator.ImitateCreateEnemyUnit(battleData, Index, Id);

            IEnumerable<BattleDiceCardModelBuilder> fixedDeck = CreateFixedDeck(battleData.unitData.GetDeckSize());
            unit.allyCardDetail.ImitateInit(fixedDeck, unit);

            if (IsDie)
            {
                unit.DieFake();
            }

            if (Passives != null)
            {
                foreach (var passive in Passives)
                {
                    passive.Init(unit);
                    unit.passiveDetail.AddPassive(passive);
                    unit.passiveDetail.OnCreated();
                }
            }

            return unit;
        }

        /// <summary>
        /// 指定したデッキ容量を満たすデッキを生成します。
        /// </summary>
        /// <param name="deckSize">デッキ容量。</param>
        /// <returns>
        /// <see cref="Deck"/> プロパティに設定されたバトル ページ XML 情報のコレクションを返します。
        /// コレクションの要素数が <paramref name="deckSize"/> を超える場合はコレクションの先頭から <paramref name="deckSize"/> 個までの要素を返し、
        /// <paramref name="deckSize"/> に満たない場合は要素数が <paramref name="deckSize"/> になるまでコレクションの末尾に規定のインスタンスで満たしたものを返します。
        /// </returns>
        private IEnumerable<BattleDiceCardModelBuilder> CreateFixedDeck(int deckSize)
        {
            if (deckSize < 0) { throw new ArgumentOutOfRangeException(nameof(deckSize), "デッキ容量を 0 以下にすることはできません。"); }

            List<BattleDiceCardModelBuilder> deck = new List<BattleDiceCardModelBuilder>();

            if (Deck != null)
            {
                deck.AddRange(Deck.Take(deckSize));
            }

            while (deck.Count < deckSize)
            {
                deck.Add(new BattleDiceCardModelBuilder());
            }

            return deck;
        }
    }
}
