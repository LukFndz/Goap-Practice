using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPickaxeProAction : GoapAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        Debug.Log("Picking up a Pro Pickaxe!");
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) =>
        state.currentPickaxe == Pickaxe.Basic &&
        state.stamina >= staminaCost;

        effects = (state) =>
        {
            WorldState newState = new WorldState(state);
            newState.currentPickaxe = Pickaxe.Pro;
            newState.stamina -= staminaCost;
            newState.stamina *= staminaMultiplier;

            return newState;
        };
    }
}
