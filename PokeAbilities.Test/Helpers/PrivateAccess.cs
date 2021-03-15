using System.Reflection;

namespace PokeAbilities.Test.Helpers
{
    /// <summary>
    /// プライベート アクセス修飾子で宣言されたオブジェクトのメンバーを操作するユーティリティ クラスです。
    /// </summary>
    public static class PrivateAccess
    {
        /// <summary>
        /// 指定したオブジェクトの指定したプライベート フィールドに指定した値を設定します。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fieldName"></param>
        /// <param name="setValue"></param>
        public static void SetField(object target, string fieldName, object setValue)
            => target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, setValue);
    }
}
