using System.Collections.Generic;
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
        }
    }
}
