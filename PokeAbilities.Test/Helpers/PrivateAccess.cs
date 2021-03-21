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
  
        /// <summary>
        /// 指定したオブジェクト型の指定した静的プライベート フィールドに指定した値を設定します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="setValue"></param>
        public static void SetField<T>(string fieldName, object setValue)
            => typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, setValue);

        /// <summary>
        /// 指定したオブジェクトの指定したプライベート フィールドの値を取得します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="fieldName"></param>
        public static T GetField<T>(object target, string fieldName)
            => (T)target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
    }
}
