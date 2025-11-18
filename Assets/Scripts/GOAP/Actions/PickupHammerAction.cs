using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHammerAction : GoapAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) =>
        !state.hasHammer &&
        state.stamina >= staminaCost;

        effects = (state) =>
        {
            WorldState newState = new WorldState(state);
            newState.hasHammer = true;
            newState.stamina -= staminaCost;
            newState.stamina *= staminaMultiplier;

            return newState;
        };
    }
}
