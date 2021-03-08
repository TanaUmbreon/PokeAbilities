#pragma warning disable CA1031 // Do not catch general exception types

using System;
using System.Linq;
using PokeAbilities.Bufs;
using System.Reflection;

namespace PokeAbilities
{
    /// <summary>
    /// <see cref="BattleUnitBufListDetail"/> の拡張メソッドを提供します。
    /// </summary>
    public static class BattleUnitBufListDetailExtension
    {
        /// <summary>
        /// 指定したポジティブ タイプが、この幕に適用されているバフに有効な状態で存在している事を判定します。
        /// </summary>
        /// <param name="target">判定する対象キャラクターのバフ リスト。</param>
        /// <param name="type">判定するポジティブ タイプ。</param>
        /// <returns>指定したポジティブ タイプが有効な状態で存在している場合は true、存在していない場合は false を返します。</returns>
        public static bool HasBuf(this BattleUnitBufListDetail target, BufPositiveType type)
            => target.GetActivatedBufList().Any(b => b.positiveType == type && !b.IsDestroyed());

        /// <summary>
        /// 指定したバフの種類が、この幕に適用されているバフに有効な状態で存在している事を判定します。
        /// </summary>
        /// <param name="target">判定する対象キャラクターのバフ リスト。</param>
        /// <param name="type">判定するバフの種類。</param>
        /// <returns>指定したバフの種類が有効な状態で存在している場合は true、存在していない場合は false を返します。</returns>
        public static bool HasBuf(this BattleUnitBufListDetail target, KeywordBuf buf)
            => target.GetActivatedBufList().Any(b => b.bufType == buf && !b.IsDestroyed());

        /// <summary>
        /// 指定した型のバフをこの幕から付与します。
        /// </summary>
        /// <typeparam name="T">付与するバフの型。</typeparam>
        /// <param name="target">バフを付与する対象キャラクターのバフ リスト。</param>
        /// <param name="stack">付与するバフのスタック数。</param>
        public static void AddBuf<T>(this BattleUnitBufListDetail target, int stack = 0) where T : BattleUnitBuf, new()
        {
            try
            {
                if (stack <= 0) { return; }

                bool mustAddBuf = false;
                BattleUnitBuf buf = target.GetActivatedBufList().FirstOrDefault(b => b is T);
                if (buf == null)
                {
                    buf = new T() { stack = 0 };
                    mustAddBuf = true;
                }
                if (!target.CanAddBuf(buf)) { return; }

                int modifiedStack = target.ModifyStack(buf, stack);
                buf.stack += modifiedStack;
                if (mustAddBuf)
                {
                    target.AddBuf(buf);
                }
                buf.OnAddBuf();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 指定した型のバフを次の幕から付与します。
        /// <see cref="BattleUnitBuf_burn"/> が指定された場合はこの幕に付与します。
        /// </summary>
        /// <typeparam name="T">付与するバフの型。</typeparam>
        /// <param name="target">バフを付与する対象キャラクターのバフ リスト。</param>
        /// <param name="stack">付与するバフのスタック数。</param>
        public static void AddReadyBuf<T>(this BattleUnitBufListDetail target, int stack) where T : BattleUnitBuf, new()
        {
            try
            {
                if (stack <= 0) { return; }

                if (typeof(T) == typeof(BattleUnitBuf_burn))
                {
                    AddBuf<T>(target, stack);
                    return;
                }

                bool mustAddBuf = false;
                BattleUnitBuf buf = target.GetReadyBufList().FirstOrDefault(b => b is T);
                if (buf == null)
                {
                    buf = new T() { stack = 0 };
                    mustAddBuf = true;
                }
                if (!target.CanAddBuf(buf)) { return; }

                int modifiedStack = target.ModifyStack(buf, stack);
                buf.stack += modifiedStack;
                if (mustAddBuf)
                {
                    target.AddReadyBuf(buf);
                }
                buf.OnAddBuf();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 付与されている天気状態を全て削除します。
        /// </summary>
        /// <param name="target">バフを付与する対象キャラクターのバフ リスト。</param>
        public static void RemoveAllWeather(this BattleUnitBufListDetail target)
        {
            target.RemoveBufAll(typeof(BattleUnitBuf_Rain));
            target.RemoveBufAll(typeof(BattleUnitBuf_SunnyDay));
        }

        #region private メンバ メソッド呼び出し

        /// <summary><see cref="BattleUnitBufListDetail"/>.ModifyStack(BattleUnitBuf, int) メソッドのキャッシュ</summary>
        private static MethodInfo modifyStackMethod = null;

        /// <summary>
        /// 指定したバフの付与数を修正します。
        /// </summary>
        /// <param name="target">付与数を修正する対象キャラクターのバフ リスト。</param>
        /// <param name="buf">修正対象のバフ。</param>
        /// <param name="stack">修正対象の付与数。</param>
        /// <returns>修正されたバフの付与数。</returns>
        private static int ModifyStack(this BattleUnitBufListDetail target, BattleUnitBuf buf, int stack)
        {
            try
            {
                if (modifyStackMethod == null)
                {
                    modifyStackMethod = typeof(BattleUnitBufListDetail).GetMethod(
                        "ModifyStack",
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        Type.DefaultBinder,
                        new[] { typeof(BattleUnitBuf), typeof(int) },
                        null);
                }

                return (int)modifyStackMethod.Invoke(target, new object[] { buf, stack });
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorWithCaller("Exception thrown.");
                Log.Instance.Error(ex);
                return stack;
            }
        }

        #endregion
    }
}
