namespace PokeAbilities.Passives
{
    /// <summary>
    /// <see cref="BattleCardTotalResult"/> で使用する表示専用パッシブの基底クラスです。
    /// 「タイプ一致」や「効果はバツグンだ！」などのメッセージ表示に使用します。
    /// </summary>
    public abstract class PassiveAbilityResultOnly : PassiveAbilityBase
    {
        public PassiveAbilityResultOnly()
            => OnCreated(); // パッシブ名、パッシブ説明のテキストを設定するために必要
    }

    /// <summary>
    /// 表示専用パッシブ「タイプ一致」
    /// ダメージ・混乱ダメージ量+1
    /// </summary>
    public class PassiveAbility_22710000 : PassiveAbilityResultOnly
    {
        /// <summary>
        /// 規定のインスタンスを取得します。
        /// </summary>
        public static PassiveAbilityBase Instance { get; } = new PassiveAbility_22710000();
    }

    /// <summary>
    /// 表示専用パッシブ「効果はバツグンだ！」
    /// ダメージ・混乱ダメージ量+1
    /// </summary>
    public class PassiveAbility_22710001 : PassiveAbilityResultOnly
    {
        /// <summary>
        /// 規定のインスタンスを取得します。
        /// </summary>
        public static PassiveAbilityBase Instance { get; } = new PassiveAbility_22710001();
    }

    /// <summary>
    /// 表示専用パッシブ「効果は今ひとつのようだ……」
    /// ダメージ・混乱ダメージ量-1
    /// </summary>
    public class PassiveAbility_22710002 : PassiveAbilityResultOnly
    {
        /// <summary>
        /// 規定のインスタンスを取得します。
        /// </summary>
        public static PassiveAbilityBase Instance { get; } = new PassiveAbility_22710002();
    }

    /// <summary>
    /// 表示専用パッシブ「効果がないようだ…」
    /// ダメージ・混乱ダメージ量-2
    /// </summary>
    public class PassiveAbility_22710003 : PassiveAbilityResultOnly
    {
        /// <summary>
        /// 規定のインスタンスを取得します。
        /// </summary>
        public static PassiveAbilityBase Instance { get; } = new PassiveAbility_22710003();
    }
}
