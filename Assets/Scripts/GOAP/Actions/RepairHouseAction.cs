using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairHouseAction : GoapAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) => 
        state.hasHammer && 
        state.stamina >= staminaCost &&
        state.brokenHouse;

        effects = (state) =>
        {
            WorldState newState = new WorldState(state);
            newState.brokenHouse = false;
            newState.stamina -= staminaCost;
            newState.stamina *= staminaMultiplier;
            return newState;
        };
    }
}
