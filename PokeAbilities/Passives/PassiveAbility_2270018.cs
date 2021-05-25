using LOR_DiceSystem;

namespace PokeAbilities.Passives
{
    /// <summary>
    /// パッシブ「するどいめ」
    /// マッチ相手の回避ダイスは威力の影響を受けない。
    /// </summary>
    public class PassiveAbility_2270018 : PassiveAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            BattlePlayingCardDataInUnitModel targetCard = behavior.card.target?.currentDiceAction;
            BattleDiceBehavior targetDice = targetCard?.currentBehavior;
            if (targetDice == null || targetDice.Detail != BehaviourDetail.Evasion) { return; }

            owner.battleCardResultLog?.SetPassiveAbility(this);
            targetCard.ignorePower = true;
        }
    }
}
