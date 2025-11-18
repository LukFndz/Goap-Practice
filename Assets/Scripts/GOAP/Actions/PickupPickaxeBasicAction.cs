using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPickaxeBasicAction : GoapAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        Debug.Log("Picking up a Basic Pickaxe!");
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) =>
        state.currentPickaxe == Pickaxe.None &&
        state.stamina >= staminaCost;

        effects = (state) =>
        {
            WorldState newState = new WorldState(state);
            newState.currentPickaxe = Pickaxe.Basic;
            newState.stamina -= staminaCost;
            newState.stamina *= staminaMultiplier;
            return newState;
        };
    }
}
