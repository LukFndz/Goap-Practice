using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCoinsBasicAction : GoapAction
{
    public int goldReward = 2;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        Debug.Log("Mining coins with Basic Pickaxe!");
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) =>
        state.currentPickaxe == Pickaxe.Basic &&
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
