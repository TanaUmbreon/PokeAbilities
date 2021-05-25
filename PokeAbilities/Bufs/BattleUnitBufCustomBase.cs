using BaseMod;
using HarmonyLib;
using UnityEngine;

namespace PokeAbilities.Bufs
{
    /// <summary>
    /// カスタムしたバフの基底クラスです。
    /// </summary>
    public class BattleUnitBufCustomBase : BattleUnitBuf
    {
        /// <summary>
        /// バフの最大スタック数を取得します。
        /// </summary>
        protected virtual int MaxStack { get; } = int.MaxValue;

        public override void OnAddBuf(int addedStack)
            => stack = (stack > MaxStack) ? MaxStack : stack;

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
                Log.Instance.ErrorWithCaller($"ArtWorks not found. (artWorksName: {artWorksName})");
                return;
            }

            typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, sprite);
            typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
        }
    }
}
