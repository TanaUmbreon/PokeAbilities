using System;
using System.IO;
using UnityEngine;

namespace PokeAbilities
{
    /// <summary>
    /// ログ出力機能を提供します。
    /// </summary>
    public class Log
    {
        #region Singleton 実装

        /// <summary>
        /// このクラスのインスタンスを取得します。
        /// 初回呼び出し時にログ ファイルが新規作成されます。既に存在する場合は上書きされます。
        /// </summary>
        public static Log Instance { get; } = new Log();

        #endregion

        /// <summary>ログ ファイルの出力先パス</summary>
        private readonly string FilePath = Path.Combine(Application.dataPath, "BaseMods/PokeAbilitiesLog.txt");

        /// <summary>
        /// <see cref="Log"/> の新しいインスタンスを生成します。
        /// </summary>
        private Log() => File.WriteAllText(FilePath, "");

        /// <summary>
        /// 指定した文字列を、続いて改行文字を書き込みます。
        /// </summary>
        /// <param name="value">書き込む文字列。</param>
        public void AppendLine(string value) => File.AppendAllText(FilePath, value + "\n");

        /// <summary>
        /// 指定した例外の説明メッセージとスタック トレースを、続いて改行文字を書き込みます。
        /// </summary>
        /// <param name="ex">書き込む例外。</param>
        public void AppendLine(Exception ex) => AppendLine($"{ex.Message}\n<{ex.GetType().FullName}>\n{ex.StackTrace}");

        /// <summary>
        /// 指定した値を、続いて改行文字を書き込みます。
        /// </summary>
        /// <param name="value">書き込む値。</param>
        public void AppendLine<T>(T value) where T : struct => AppendLine(value.ToString());
    }
}
