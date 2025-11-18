using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCoinsProAction : GoapAction
{
    public int goldReward = 5;
    public override void ExecuteAction()
    {
        base.ExecuteAction();
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) =>
        state.currentPickaxe == Pickaxe.Pro &&
        state.stamina >= staminaCost;

        effects = (state) =>
        {
            WorldState newState = new WorldState(state);
            newState.gold += goldReward;
            newState.stamina -= staminaCost;
            newState.stamina *= staminaMultiplier;
            return newState;
        };
    }
}
