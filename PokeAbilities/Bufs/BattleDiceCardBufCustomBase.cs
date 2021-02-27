using BaseMod;
using HarmonyLib;
using UnityEngine;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// カスタムしたバトル ページ バフの基底クラスです。
    /// </summary>
    public class BattleDiceCardBufCustomBase : BattleDiceCardBuf
    {
        /// <summary>
        /// <see cref="keywordIconId"/> プロパティの値を使用して、このバフのアイコンを読み込みます。
        /// </summary>
        protected void LoadIcon()
            => LoadIcon(keywordIconId);

        /// <summary>
        /// 指定したアート ワーク名でこのバフのアイコンを読み込みます。
        /// </summary>
        /// <param name="artWorksName">アイコンとして読み込むアート ワーク名。</param>
        protected void LoadIcon(string artWorksName)
        {
            Harmony_Patch.ArtWorks.TryGetValue(artWorksName, out Sprite sprite);
            if (sprite == null)
            {
                Log.Instance.AppendLine("[BattleDiceCardBufCustomBase.LoadIcon] ArtWorks not found. (artWorksName: " + artWorksName + ")");
                return;
            }

            typeof(BattleDiceCardBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, sprite);
            typeof(BattleDiceCardBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }
    }
}
