using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace PokeAbilities.Test
{
    /// <summary>
    /// アセンブリ単位のユニット テストで呼び出されるセット アップ処理です。
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

            // SecurityException (ECall メソッドをシステム モジュールにパッケージ化しなければなりません) の回避
            OverwriteMethod<UnityEngine.Random>(nameof(Range), typeof(int), typeof(int));
            OverwriteMethod<Debug>(nameof(LogError), typeof(object));
            OverwriteMethod<StageController>(nameof(IsLogState)); // 呼出元: BattleUnitModel.RecoverHP(int)
            OverwriteMethod<BattleUnitModel>(nameof(CheckGiftOnTakeDamage), typeof(int), typeof(DamageType), typeof(BattleUnitModel), typeof(KeywordBuf)); // 呼び出し元: BattleUnitModel.TakeDamage(int)

            // 実績解除を行おうとするメソッドの呼び出しを回避
            OverwriteMethod<BattleUnitBufListDetail>(nameof(CheckAchievements));
        }

        /// <summary>
        /// 指定した型の指定した名前のメソッドの呼び出し先を、
        /// このクラスに定義された同名のメソッドに上書きします。
        /// </summary>
        /// <typeparam name="TTarget">上書き対象の型。</typeparam>
        /// <param name="methodName"><typeparamref name="TTarget"/> に含まれる上書き対象のメソッド名、およびこのクラスに含まれる上書き元のメソッド名。</param>
        /// <param name="methodArgumentTypes">上書き対象のメソッドの引数の型リスト。</param>
        private void OverwriteMethod<TTarget>(string methodName, params Type[] methodArgumentTypes)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            OverwriteMethod(
                target: typeof(TTarget).GetMethod(methodName, flags, null, methodArgumentTypes, null),
                replacement: GetType().GetMethod(methodName, flags, null, methodArgumentTypes, null));
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

            *((int*)targetPointer + 2) = *((int*)replacementPointer + 2);
        }

        public static int Range(int min, int max)
            => new System.Random().Next(min, max);

        public static void LogError(object message)
            => Console.WriteLine(message);

        public bool IsLogState()
            => true;

        public void CheckGiftOnTakeDamage(int dmg, DamageType type, BattleUnitModel attacker, KeywordBuf keyword)
            => Console.WriteLine(nameof(CheckGiftOnTakeDamage));

        private void CheckAchievements() { }
    }
}
