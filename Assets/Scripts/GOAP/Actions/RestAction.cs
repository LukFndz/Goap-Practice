using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : GoapAction
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
    }

    protected override void SetUpPreconditionsAndEffects()
    {
        preconditions = (state) => 
        state.stamina < 10 &&
        !state.brokenHouse;

        effects = (state) =>
        {
            WorldState newState = new WorldState(state);
            newState.stamina = 100;
            return newState;
        };
    }
}
