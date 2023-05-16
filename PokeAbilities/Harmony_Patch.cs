using System;
using System.Collections.Generic;
using HarmonyLib;
using UI;
using UnityEngine;

namespace PokeAbilities
{
    [Harmony]
    public class Harmony_Patch
    {
        /// <summary>
        /// <see cref="Harmony_Patch"/> の新しいインスタンスを生成します。
        /// </summary>
        public Harmony_Patch()
        {
            try
            {
                new Harmony("PokeAbilities").PatchAll();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        [HarmonyPatch(typeof(UISpriteDataManager), "GetStoryIcon")]
        [HarmonyPrefix]
        public static bool UISpriteDataManager_GetStoryIcon(ref UIIconManager.IconSet __result, string story)
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
