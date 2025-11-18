using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GoapAction : MonoBehaviour
{
    public Func<WorldState, bool> preconditions;
    public Func<WorldState, WorldState> effects;
    public bool requiresMovement = false;
    public int cost = 1; // Goap cost
    public float staminaCost = 10; // Default player stamina cost
    public float staminaMultiplier = 0.75f; // percentage of action cost that will be subtracted from stamina

    public GameObject target; //some actions target


    private void Start()
    {
        SetUpPreconditionsAndEffects();
    }

    public bool CheckPreconditions(WorldState state)
    {
        return preconditions(state);
    }

    public WorldState ApplyEffects(WorldState state)
    {
        return effects(state);
    }

    protected abstract void SetUpPreconditionsAndEffects(); //cada accion tendra sus propias preco y efectos

    public virtual void ExecuteAction() //esto se dispara cuando el player va y hace la accion posta. no durante el planning. cada accion agrega sus cositas
    {
        GameManager.instance.currentState = ApplyEffects(GameManager.instance.currentState);
    }
}
