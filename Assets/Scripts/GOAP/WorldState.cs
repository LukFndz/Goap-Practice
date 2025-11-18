using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pickaxe
{
    None,
    Basic,
    Pro
}

[System.Serializable]
public class WorldState
{
    public float stamina;

    public bool hasHammer;
    public bool brokenHouse;

    public Pickaxe currentPickaxe;
    public int gold;

    public WorldState(WorldState state)
    {
        stamina = state.stamina;
        hasHammer = state.hasHammer;
        brokenHouse = state.brokenHouse;
        currentPickaxe = state.currentPickaxe;
        gold = state.gold;
    }

    public WorldState(float stamina, bool hasHammer, bool brokenHouses, Pickaxe currentPickaxe, int gold)
    {
        this.stamina = stamina;
        this.hasHammer = hasHammer;
        this.brokenHouse = brokenHouses;
        this.currentPickaxe = currentPickaxe;
        this.gold = gold;
    }
}

