using System.Collections.Generic;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「ノーマルタイプ」
    /// このキャラクターはノーマルタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「ノーマルタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22700100 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Normal, };
    }

    /// <summary>
    /// パッシブ「ほのおタイプ」
    /// このキャラクターはほのおタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「ほのおタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22700200 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Fire, };
    }

    /// <summary>
    /// パッシブ「みずタイプ」
    /// このキャラクターはみずタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「みずタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22700300 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Water, };
    }

    /// <summary>
    /// パッシブ「みず/ひこうタイプ」
    /// このキャラクターはみずタイプとひこうタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「みずタイプ」と「ひこうタイプ」をそれぞれランダムに付与。
    /// </summary>
    public class PassiveAbility_22700310 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Water, PokeType.Flying };
    }

    /// <summary>
    /// パッシブ「でんきタイプ」
    /// このキャラクターはでんきタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「でんきタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22700400 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Electric, };
    }

    /// <summary>
    /// パッシブ「くさタイプ」
    /// このキャラクターはくさタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「くさタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22700500 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Grass, };
    }

    /// <summary>
    /// パッシブ「こおりタイプ」
    /// このキャラクターはこおりタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「こおりタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22700600 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Ice, };
    }

    /// <summary>
    /// パッシブ「エスパータイプ」
    /// このキャラクターはエスパータイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「エスパータイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22701100 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Psychic, };
    }

    /// <summary>
    /// パッシブ「あくタイプ」
    /// このキャラクターはあくタイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「あくタイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22701600 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Dark, };
    }

    /// <summary>
    /// パッシブ「フェアリータイプ」
    /// このキャラクターはフェアリータイプを持ち、特定のパッシブやバトルページの影響を受ける。
    /// 幕の開始時、手元のバトルページ2枚に「フェアリータイプ」をランダムに付与。
    /// </summary>
    public class PassiveAbility_22701800 : PassiveAbilityTypeBase
    {
        public override IEnumerable<PokeType> Types => new[] { PokeType.Fairy, };
    }
}
