using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UI;
using UnityEngine;

namespace PokeAbilities
{
    public class Harmony_Patch
    {
        /// <summary>
        /// <see cref="Harmony_Patch"/> の新しいインスタンスを生成します。
        /// </summary>
        public Harmony_Patch()
        {
            var harmony = new Harmony("PokeAbilities");
            MethodInfo method = typeof(Harmony_Patch).GetMethod(nameof(UISpriteDataManager_GetStoryIcon));
            harmony.Patch(typeof(UISpriteDataManager).GetMethod("GetStoryIcon", AccessTools.all), new HarmonyMethod(method), null, null, null);
        }

		public static bool UISpriteDataManager_GetStoryIcon(UISpriteDataManager __instance, ref UIIconManager.IconSet __result, string story)
		{
            if (story != "PokeAbilities") { return true; }

            ArtWorks.TryGetValue("Pokemon", out var sprite);
            if (sprite == null) { return true; }

            __result = new UIIconManager.IconSet
            {
                icon = sprite,
                iconGlow = sprite
            };
            return false;
        }

        /// <summary>
        /// 各 MOD に配置された ArtWorks フォルダー内のスプライトを取得します。
        /// </summary>
        public static Dictionary<string, Sprite> ArtWorks
            => BaseMod.Harmony_Patch.ArtWorks;
	}
}
