namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「せいしんりょく」。
    /// 行動不能・虚弱状態に対して免疫。
    /// </summary>
    public class PassiveAbility_2270002 : PassiveAbilityBase
    {
        public override bool CanAddBuf(BattleUnitBuf buf)
        {
            if (buf.bufType == KeywordBuf.Stun || buf.bufType == KeywordBuf.Weak) {
                owner.battleCardResultLog?.SetPassiveAbility(this);
                return false;
            }

            return base.CanAddBuf(buf);
        }
    }
}
