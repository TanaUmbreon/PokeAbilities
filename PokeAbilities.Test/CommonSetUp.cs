using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using HarmonyLib;
using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Test.Helpers;
using PokeAbilities.Test.Helpers.Battles;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace PokeAbilities.Test
{
    /// <summary>
    /// アセンブリ単位のユニット テストで呼び出されるセット アップ処理です。
    /// </summary>
    [SetUpFixture]
    [HarmonyPatch]
    public class CommonSetUp
    {
        ///// <summary>バトル ページ効果のキー名と、それに対応するバトル ページ効果の型情報のマップ</summary>
        //private static readonly Dictionary<string, DiceCardSelfAbilityBase> keyDiceCardSelfAbilityMap = new Dictionary<string, DiceCardSelfAbilityBase>();
        ///// <summary>バトル ダイス効果のキー名と、それに対応するバトル ダイス効果の型情報のマップ</summary>
        //private static readonly Dictionary<string, DiceCardAbilityBase> keyDiceCardAbilityMap = new Dictionary<string, DiceCardAbilityBase>();

        ///// <summary>ゲーム本体 (Assembly-CSharp.dll) のアセンブリ参照</summary>
        //private static Assembly ruinaAssembly = null;
        ///// <summary>MOD 本体 (PokeAbilities.dll) のアセンブリ参照</summary>
        //private static Assembly modAssembly = null;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            new Harmony("PokeAbilities.Test").PatchAll();
            InitializeFields();
            InitializeSingleton();
            OverwriteMethods();
        }

        #region HarmonyPatch による割込

        // Memo:
        //   ゲーム非起動時に Unity ライブラリのメソッドを呼び出すと
        //   SecurityException (ECall メソッドをシステム モジュールにパッケージ化しなければなりません) が発生する。
        //   これを回避する為、実装を置き換えて例外を回避している。

        /// <remarks>
        /// 呼び出し箇所:
        /// <see cref="BattleEmulator.StartBattleRoutine"/> →
        /// <see cref="BattleObjectManager.Clear"/>
        /// </remarks>
        [HarmonyPatch(typeof(BattleObjectLayer), "Clear")]
        [HarmonyPrefix]
        public static bool BattleObjectLayer_Clear_Prefix(BattleObjectLayer __instance)
        {
            // 処理全てを呼び出さないようにする
            return false;
        }

        /// <remarks>
        /// 呼び出し箇所:
        /// <see cref="BattleEmulator.StartBattleRoutine"/> →
        /// <see cref="StageController.InitStageByInvitation(StageClassInfo, List{LorId})"/> →
        /// <see cref="StageController.Clear"/>
        /// </remarks>
        [HarmonyPatch(typeof(StageController), "InitCommon")]
        [HarmonyPrefix]
        public static bool StageController_InitCommon_Prefix(StageController __instance, StageClassInfo stage, bool isRebattle)
        {
            __instance.battleState = StageController.BattleState.Setting;
            PrivateAccess.SetField(__instance, "_isRebattle", isRebattle);
            PrivateAccess.SetField(__instance, "_stageModel", new StageModel());
            PrivateAccess.GetField<StageModel>(__instance, "_stageModel").Init(stage, LibraryModel.Instance, isRebattle);
            PrivateAccess.SetField(__instance, "_alreadyStory", false);
            PrivateAccess.SetField(__instance, "_roundendedforcely", false);
            PrivateAccess.SetField(__instance, "_rewardInfo", new BattleRewardInfo());
            __instance._droppedbookdatas = new List<DropBookDataForAddedReward>();
            PrivateAccess.SetField(__instance, "_usedFloorList", new List<SephirahType>());
            PrivateAccess.GetField<List<string>>(__instance, "_addedEgoMap").Clear();
            PrivateAccess.GetField<List<LorId>>(__instance, "_usedBooks").Clear();
            __instance.SetCurrentWave(1);

            // BattleManagerUIクラスを操作する処理は呼び出さないようにする

            Singleton<LibraryQuestManager>.Instance.OnStageStart();

            return false;
        }

        /*
        [HarmonyPatch(typeof(Log), "AddLog")]
        [HarmonyPrefix]
        public static bool Log_AddLog_Prefix(Log.LogLevel level, string message)
        {
            // ログはファイルではなく標準出力に出力する
            Console.WriteLine($"[{level.Text,-5}] {message}");
            return false;
        }

        [HarmonyPatch(typeof(BattleUnitBufListDetail), "CheckGift")]
        [HarmonyPrefix]
        public static bool BattleUnitBufListDetail_CheckGift_Prefix()
        {
            // 戦闘表象入手の為の行動履歴の記録は行わない
            return false;
        }

        [HarmonyPatch(typeof(BattleUnitBufListDetail), "CheckAchievements")]
        [HarmonyPrefix]
        private static bool BattleUnitBufListDetail_CheckAchievements_Prefix()
        {
            // 実績解除の処理は行わない
            return false;
        }
        */

        #region Harmony による割込 (インスタンスの動的生成)

        /*
        // Memo:
        //   Assembly-CSharp.dll から全ての型情報を取得 (Assembly.GetTypes()) しようとすると、多数の Unity ライブラリの参照が必要となり手間。
        //   型の完全名を指定してピンポイントで取得 (Assembly.GetType(string)) する場合はそれら参照が不要になるため、実装を置き換えている。

        /// <summary>
        /// <see cref="BattleDiceCardModel.CreateDiceCardSelfAbilityScript"/> メソッドが呼び出される直前に割り込まれます。
        /// バトル ページ効果オブジェクトの生成を NUnit 上で実行できるように処理を差し替えます。
        /// このメソッドでは、ゲーム本体とこの MOD 本体に定義されたバトル ページ効果のみ、キー名から生成します。
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="__result"></param>
        /// <returns>false 固定で本来の呼び出すメソッドを呼び出さない。</returns>
        [HarmonyPatch(typeof(BattleDiceCardModel), "CreateDiceCardSelfAbilityScript")]
        [HarmonyPrefix]
        private static bool BattleDiceCardModel_CreateDiceCardSelfAbilityScript_Prefix(BattleDiceCardModel __instance, ref DiceCardSelfAbilityBase __result)
        {
            __result = null;

            string key = __instance.XmlData.Script;
            if (string.IsNullOrEmpty(key)) { return false; }

            if (keyDiceCardSelfAbilityMap.TryGetValue(key, out __result)) { return false; }

            // ゲーム本体のアセンブリから型情報を検索
            if (TryCreateInstance(ruinaAssembly, $"DiceCardSelfAbility_{key}", out __result))
            {
                keyDiceCardSelfAbilityMap[key] = __result;
                return false;
            }

            // MOD本体のアセンブリから型情報を検索
            if (TryCreateInstance(modAssembly, $"PokeAbilities.CardAbilities.DiceCardSelfAbility_{key}", out __result))
            {
                keyDiceCardSelfAbilityMap[key] = __result;
                return false;
            }

            throw new InvalidOperationException($"設定されているバトル ページ効果のキー名 '{key}' に対応するバトル ページ効果は定義されていません。");
        }

        // ToDo: バトルダイス効果オブジェクトの生成も同様に対応する。

        /// <summary>
        /// 指定したアセンブリに含まれる指定した型の名前に一致するオブジェクトのインスタンスを生成し、out パラメータにて返します。
        /// 戻り値は生成が成功したかどうかを示します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="name">名前空間も含めた完全な型の名前。</param>
        /// <param name="ability">インスタンスの生成に成功した場合は <typeparamref name="T"/> のインスタンス。失敗した場合は null。</param>
        /// <returns></returns>
        private static bool TryCreateInstance<T>(Assembly assembly, string name, out T ability) where T : class
        {
            ability = null;

            Type t = assembly.GetType(name, throwOnError: false, ignoreCase: true);
            if (t == null) { return false; }

            ability = Activator.CreateInstance(t) as T;
            return ability != null;
        }
        */
        #endregion

        #region Harmony による割込 (SecurityException の回避)

        /*
        // Memo:
        //   ゲーム非起動時に Unity ライブラリのメソッドを呼び出すと
        //   SecurityException (ECall メソッドをシステム モジュールにパッケージ化しなければなりません) が発生する。
        //   これを回避する為、実装を置き換えて例外を回避している。

        [HarmonyPatch(typeof(Debug), "LogError", new[] { typeof(object) })]
        [HarmonyPrefix]
        private static bool Debug_LogError_Prefix(object message)
        {
            Console.WriteLine($"[LogError] {message}");
            Console.WriteLine(new StackTrace(2).ToString()); // 2は、このメソッドとHarmonyによる割込みメソッドを省略している
            return false;
        }

        [HarmonyPatch(typeof(Util), "PreLoadPrefab")]
        [HarmonyPrefix]
        private static bool Util_PreLoadPrefab_Prefix(string path)
        {
            Console.WriteLine($"[PreLoadPrefab] path: '{path}'");
            return false;
        }

        #endregion

        #region Harmony による割込 (null 参照の回避)

        [HarmonyPatch(typeof(LibraryModel), "GetFloor")]
        [HarmonyPrefix]
        private static bool LibraryModel_GetFloor_Prefix(ref LibraryFloorModel __result, SephirahType sephirah)
        {
            __result = new LibraryFloorModel();
            __result.SetTemporaryLevel(__result.Maxlevel);
            __result.SetLevel(__result.Maxlevel);

            return false;
        }

        [HarmonyPatch(typeof(BattleUnitModel), "RollSpeedDice")]
        [HarmonyPrefix]
        private static bool BattleUnitModel_RollSpeedDice_Prefix(BattleUnitModel __instance, ref List<SpeedDice> __result)
        {
            __instance.currentSpeedDiceIdx = 0;
            __instance.speedDiceResult = __instance.Book.GetSpeedDiceRule(__instance).Roll(__instance);
            foreach (SpeedDice speedDice in __instance.speedDiceResult)
            {
                if (__instance.IsBreakLifeZero() || __instance.IsKnockout() || speedDice.breaked)
                {
                    speedDice.value = 0;
                }
                else
                {
                    int num = __instance.bufListDetail.GetSpeedDiceAdder(speedDice.value);
                    num += __instance.emotionDetail.GetSpeedDiceAdder(speedDice.value);
                    num += __instance.passiveDetail.GetSpeedDiceAdder(speedDice.value);
                    speedDice.value = Mathf.Clamp(speedDice.value + num, 1, 999);
                }
            }
            __instance.speedDiceResult.Sort(delegate (SpeedDice d1, SpeedDice d2)
            {
                if (d1.breaked && d2.breaked)
                {
                    if (d1.value > d2.value)
                    {
                        return -1;
                    }
                    if (d1.value < d2.value)
                    {
                        return 1;
                    }
                    return 0;
                }
                else
                {
                    if (d1.breaked && !d2.breaked)
                    {
                        return -1;
                    }
                    if (!d1.breaked && d2.breaked)
                    {
                        return 1;
                    }
                    if (d1.value > d2.value)
                    {
                        return -1;
                    }
                    if (d1.value < d2.value)
                    {
                        return 1;
                    }
                    return 0;
                }
            });
            __instance.passiveDetail.OnRollSpeedDice();
            __instance.bufListDetail.OnRollSpeedDice();

            // null参照を回避
            //__instance.view.speedDiceSetterUI.SetSpeedDicesAfterRoll(th__instanceis.speedDiceResult);

            __result = __instance.speedDiceResult;
            return false;
        }
        */

        #endregion

        #endregion

        /// <summary>
        /// <see cref="CommonSetUp"/> の静的フィールドを初期化します。
        /// </summary>
        private void InitializeFields()
        {
            //foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    switch (assembly.GetName().Name)
            //    {
            //        case "Assembly-CSharp":
            //            ruinaAssembly = assembly;
            //            break;
            //        case "PokeAbilities":
            //            modAssembly = assembly;
            //            break;
            //    }
            //}
            //if (ruinaAssembly == null) { throw new InvalidOperationException("Assembly-CSharp.dll がロードされていません。"); }
            //if (modAssembly == null) { throw new InvalidOperationException("PokeAbilities.dll がロードされていません。"); }
        }

        #region InitializeSingleton

        /// <summary>
        /// シングルトン オブジェクトを初期化します。
        /// </summary>
        private void InitializeSingleton()
        {
            // 接待処理を初期化する時に必須
            // BattleObjectManager.Clear()メソッドでBattleObjectLayerを参照している
            // BattleObjectLayerはシングルトンだがMonoBehaviour(Unityによる依存性注入)のため手動でインスタンス生成する
            if (BattleObjectLayer.instance == null)
            {
                var instance = new BattleObjectLayer();
                PrivateAccess.SetField<BattleObjectLayer>("_instance", instance);
                PrivateAccess.SetField(instance, "_unitList", new List<BattleUnitView>());
                instance.unitViewportList = new List<BattleObjectLayer.UnitViewportPosInfo>();
            }

            //// カスタムバフを生成する時に必須
            //BaseMod.Harmony_Patch.ArtWorks = new Dictionary<string, Sprite>();

            //// BattleUnitModelBuilder.EquipBook を指定している状態でBattleUnitModelを生成する時に必須
            //Singleton<DeckXmlList>.Instance.Init(new List<DeckXmlInfo>());

            //// BattleUnitModel の生成の内部処理でnull参照例外を回避する為に必須
            //// (BattleUnitModel.SetUnitData → UnitDataModel.SetUnitData → ItemXmlDataList.GetCardItem で例外発生)
            //ItemXmlDataList.instance.InitCardInfo(new List<DiceCardXmlInfo>());

            //// (BattleUnitEmotionDetail.Reset → BattleUnitModel.OnCreated で例外発生)
            //var stage = new StageModel();
            //PrivateAccess.SetField(stage, "_classInfo", new StageClassInfo());
            //PrivateAccess.SetField(Singleton<StageController>.Instance, "_stageModel", stage);

            //// UnitBattleDataModel の生成の内部処理でnull参照例外を回避する為に必須(敵キャラクター生成時のみ)
            //// 使用する敵キャラクターIDに一致するEnemyUnitClassInfoをリストに登録しておく必要がある
            //// (UnitBattleDataModel.CreateUnitBattleDataByEnemyUnitId で例外発生)
            //Singleton<EnemyUnitClassInfoList>.Instance.Init(EnemyUnitInfo.GetEnemies());

            //LibraryModel.Instance.Init();
        }

        #endregion

        #region OverwriteMethods

        /// <summary>
        /// UnityEngine の <see cref="SecurityException"/> 例外を回避する為にメソッドを上書きします。
        /// <see cref="HarmonyPatch"/> による割り込みでも例外を回避できない場合に使用します。
        /// </summary>
        private void OverwriteMethods()
        {
            //OverwriteMethod<UnityEngine.Random>(nameof(Range), typeof(int), typeof(int));
            //OverwriteMethod<StageController>(nameof(IsLogState)); // 呼出元: BattleUnitModel.RecoverHP(int)
            //OverwriteMethod<BattleUnitModel>(nameof(CheckGiftOnTakeDamage), typeof(int), typeof(DamageType), typeof(BattleUnitModel), typeof(KeywordBuf)); // 呼び出し元: BattleUnitModel.TakeDamage(int)
        }

        /*
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
        */

        #endregion
    }
}
