using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using LOR_DiceSystem;
using NUnit.Framework;
using PokeAbilities.Test.Helpers;
using PokeAbilities.Test.Helpers.StaticInfo;
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
        /// <summary>バトル ページ効果のキー名と、それに対応するバトル ページ効果の型情報のマップ</summary>
        private static readonly Dictionary<string, DiceCardSelfAbilityBase> keyDiceCardSelfAbilityMap = new Dictionary<string, DiceCardSelfAbilityBase>();
        /// <summary>バトル ダイス効果のキー名と、それに対応するバトル ダイス効果の型情報のマップ</summary>
        private static readonly Dictionary<string, DiceCardAbilityBase> keyDiceCardAbilityMap = new Dictionary<string, DiceCardAbilityBase>();

        /// <summary>ゲーム本体 (Assembly-CSharp.dll) のアセンブリ参照</summary>
        private static Assembly ruinaAssembly = null;
        /// <summary>MOD 本体 (PokeAbilities.dll) のアセンブリ参照</summary>
        private static Assembly modAssembly = null;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            InitializeStaticFields();
            new Harmony("PokeAbilities.Test").PatchAll();

            // カスタムバフを生成する時に必須
            BaseMod.Harmony_Patch.ArtWorks = new Dictionary<string, Sprite>();

            // BattleUnitModelBuilder.EquipBook を指定している状態でBattleUnitModelを生成する時に必須
            Singleton<DeckXmlList>.Instance.Init(new List<DeckXmlInfo>());

            // BattleUnitModel の生成の内部処理でnull参照例外を回避する為に必須
            // (BattleUnitModel.SetUnitData → UnitDataModel.SetUnitData → ItemXmlDataList.GetCardItem で例外発生)
            ItemXmlDataList.instance.InitCardInfo(new List<DiceCardXmlInfo>());
            // (BattleUnitEmotionDetail.Reset → BattleUnitModel.OnCreated で例外発生)
            var stage = new StageModel();
            PrivateAccess.SetField(stage, "_classInfo", new StageClassInfo());
            PrivateAccess.SetField(Singleton<StageController>.Instance, "_stageModel", stage);

            // UnitBattleDataModel の生成の内部処理でnull参照例外を回避する為に必須(敵キャラクター生成時のみ)
            // 使用する敵キャラクターIDに一致するEnemyUnitClassInfoをリストに登録しておく必要がある
            // (UnitBattleDataModel.CreateUnitBattleDataByEnemyUnitId で例外発生)
            Singleton<EnemyUnitClassInfoList>.Instance.Init(EnemyUnitInfo.GetEnemies());

            //LibraryModel.Instance.Init();

            //// SecurityException の回避 (Harmonyによる割り込みでも同様の例外が発生する場合にこの方法で回避)
            OverwriteMethod<UnityEngine.Random>(nameof(Range), typeof(int), typeof(int));
            
            //OverwriteMethod<StageController>(nameof(IsLogState)); // 呼出元: BattleUnitModel.RecoverHP(int)
            //OverwriteMethod<BattleUnitModel>(nameof(CheckGiftOnTakeDamage), typeof(int), typeof(DamageType), typeof(BattleUnitModel), typeof(KeywordBuf)); // 呼び出し元: BattleUnitModel.TakeDamage(int)

            //// 実績解除を行おうとするメソッドの呼び出しを回避
            //OverwriteMethod<BattleUnitBufListDetail>(nameof(CheckAchievements));
        }

        private void InitializeStaticFields()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                switch (assembly.GetName().Name)
                {
                    case "Assembly-CSharp":
                        ruinaAssembly = assembly;
                        break;
                    case "PokeAbilities":
                        modAssembly = assembly;
                        break;
                }
            }
            if (ruinaAssembly == null) { throw new InvalidOperationException("Assembly-CSharp.dll がロードされていません。"); }
            if (modAssembly == null) { throw new InvalidOperationException("PokeAbilities.dll がロードされていません。"); }
        }

        #region Harmony による割込 (インスタンスの動的生成)

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

        #endregion

        #region Harmony による割込 (SecurityException の回避)

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

        //[HarmonyPatch(typeof(LibraryModel), "Init")]
        //[HarmonyPrefix]
        //private static bool LibraryModel_Init_Prefix(LibraryModel __instance)
        //{
        //    __instance._floorList = new List<LibraryFloorModel>();
        //    __instance._openedSephirah = new HashSet<SephirahType>();
        //    __instance._clearInfo = new StageClearInfoListModel();
        //    __instance._playHistory = new PlayHistoryModel();
        //    __instance.AddFloor(SephirahType.Malkuth);
        //    __instance.AddFloor(SephirahType.Yesod);
        //    __instance.AddFloor(SephirahType.Hod);
        //    __instance.AddFloor(SephirahType.Netzach);
        //    __instance.AddFloor(SephirahType.Tiphereth);
        //    __instance.AddFloor(SephirahType.Gebura);
        //    __instance.AddFloor(SephirahType.Chesed);
        //    __instance.AddFloor(SephirahType.Binah);
        //    __instance.AddFloor(SephirahType.Hokma);
        //    __instance.AddFloor(SephirahType.Keter);
        //    Singleton<CustomCoreBookInventoryModel>.Instance.Init();
        //    Singleton<LibraryQuestManager>.Instance.Init();
        //    __instance.OpenSephirah(SephirahType.Keter);
        //    Singleton<InventoryModel>.Instance.Init();
        //    Singleton<BookInventoryModel>.Instance.Init();
        //    Singleton<DropBookInventoryModel>.Instance.Init();
        //    __instance._clearInfo.Init();
        //    __instance._currentChapter = 1;
        //    Singleton<DropBoxListModel>.Instance.Init();
        //    Singleton<DeckListModel>.Instance.Init();
        //    __instance._storySeeInfo.Clear();
        //    return false;
        //}

        #endregion

        #region メソッドの呼び出し先を上書き

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

        #endregion
    }
}
