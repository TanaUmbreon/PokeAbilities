using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// タイプ系パッシブの基底クラスです。このクラスを継承することでタイプ系パッシブとして扱われます。
    /// </summary>
    public abstract class PassiveAbilityTypeBase : PassiveAbilityBase
    {
        public abstract IEnumerable<PokeType> Types { get; }

        // ToDo: OnRoundStartAfterメソッドをオーバーライドして、手元のバトルページ2枚にタイプを付与するコードを実装する。
    }
}
