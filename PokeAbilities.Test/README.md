# PokeAbilities.Test

このプロジェクト `PokeAbilities.Test` はユニットテスト(NUnit)専用のテストプロジェクトです。このプロジェクトで生成されるDLLはMODとして使用しません。

MOD本体のプロジェクトは `PokeAbilities` になります。

## テスト実行時の注意事項

Visual Studioのテストエクスプローラーを使用してこのテストを実行する場合、通常の `実行` ではなく `デバッグ` から行ってください。アンセーフなコード (`CommonSetUp.cs` を参照) を使用しているため、通常の実行ではテストが実行されません。
