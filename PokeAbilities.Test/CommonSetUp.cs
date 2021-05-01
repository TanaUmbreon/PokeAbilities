using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace PokeAbilities.Test
{
    /// <summary>
    /// 全てのユニット テストで呼び出されるセット アップ処理です。
    /// </summary>
    [SetUpFixture]
    public class CommonSetUp
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // カスタムバフを生成する時に必須
            BaseMod.Harmony_Patch.ArtWorks = new Dictionary<string, Sprite>();

            // BattleUnitModelBuilder.EquipBook を指定している状態でBattleUnitModelを生成する時に必須
            Singleton<DeckXmlList>.Instance.Init(new List<DeckXmlInfo>());

            // BattleObjectManagerの操作や参照を行う為に必須
            BattleObjectManager.instance.Init_only();

            const BindingFlags PublicStaticBinding = BindingFlags.Public | BindingFlags.Static;
            const BindingFlags PublicBinding = BindingFlags.Public | BindingFlags.Instance;

            // SecurityException (ECall メソッドをシステム モジュールにパッケージ化しなければなりません) の回避
            {
                Type type = typeof(UnityEngine.Random);
                Type[] argumentTypes = new[] { typeof(int), typeof(int) };
                OverwriteMethod(
                    target: type.GetMethod("Range", PublicStaticBinding, null, argumentTypes, null),
                    replacement: GetType().GetMethod(nameof(Range), PublicStaticBinding, null, argumentTypes, null));
            }
            {
                Type type = typeof(Debug);
                Type[] argumentTypes = new[] { typeof(object) };
                OverwriteMethod(
                    target: type.GetMethod("LogError", PublicStaticBinding, null, argumentTypes, null),
                    replacement: GetType().GetMethod(nameof(LogError), PublicStaticBinding, null, argumentTypes, null));
            }
            // BattleUnitModel.RecoverHP(int) メソッドの内部で呼び出される
            {
                Type type = typeof(StageController);
                OverwriteMethod(
                    target: type.GetMethod("IsLogState", PublicBinding),
                    replacement: GetType().GetMethod(nameof(IsLogState), PublicBinding));
            }
        }

        /// <summary>
        /// 指定したメソッドの呼び出し先を指定したメソッドに上書きします。
        /// 上書き対象のメソッドと呼び出し先のメソッドのアクセス装飾子は同じにする必要があります。
        /// </summary>
        /// <param name="target">上書き対象のメソッド。</param>
        /// <param name="replacement"><paramref name="target"> のメソッドが呼び出された時に呼び出すメソッド。</param>
        unsafe private void OverwriteMethod(MethodInfo target, MethodInfo replacement)
        {
            void* targetPointer = target.MethodHandle.Value.ToPointer();
            void* replacementPointer = replacement.MethodHandle.Value.ToPointer();

            *((long*)targetPointer + 1) = *((long*)replacementPointer + 1);
        }

        public static int Range(int min, int max)
            => new System.Random().Next(min, max);

        public static void LogError(object message)
            => Console.WriteLine(message);

        public bool IsLogState()
            => true;
    }
}
