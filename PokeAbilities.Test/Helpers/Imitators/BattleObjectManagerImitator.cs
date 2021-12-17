namespace PokeAbilities.Test.Helpers.Imitators
{
    /// <summary>
    /// <see cref="BattleObjectManager"/> クラスのメソッドを模倣し、疑似的に再現します。
    /// </summary>
    internal static class BattleObjectManagerImitator
    {
        /// <summary>
        /// <see cref="BattleObjectManager.CreateDefaultUnit(Faction)"/> メソッドを疑似的に再現し、
        /// 規定のキャラクターを生成します。
        /// </summary>
        /// <param name="faction">生成するキャラクターが所属する派閥。</param>
        /// <param name="id">生成するキャラクターに割り当てる ID。</param>
        public static BattleUnitModel ImitateCreateDefaultUnit(Faction faction, int id) 
            => new BattleUnitModel(id) { faction = faction };
    }
}
