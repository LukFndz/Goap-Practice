using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapNode
{
    //los goapnodes son los que usa el planner. 
    //tienen asociadas un worldstate y una accion.

    public WorldState currentState;
    public GoapAction goapAction;
    public List<GoapNode> neighbors = new List<GoapNode>(); // se llenan en el goap planner

    public GoapNode(WorldState state, GoapAction action)
    {
        currentState = state;
        goapAction = action;
    }

    public List<GoapNode> GetNeighbors(Dictionary<GoapNode, GoapNode> visited) //INVENTO a los vecinos (o sea, los proximos pasos posibles) usando el GetAvailableActionsForState y aplicando sus efectos
    {
        //Debug.Log("GetNeighbors: ya te los paso, dame un segundito");
        neighbors = new List<GoapNode>();
        List<GoapAction> availableActions = GameManager.instance.GetAvailableActionsForState(currentState);

        foreach (GoapAction action in availableActions)
        {
            //Debug.Log("GetNeighbors: agrego la accion " + action.name + " como vecino de este node");
            WorldState newState = action.ApplyEffects(currentState);
            GoapNode newNode = new GoapNode(newState, action);
            if (!visited.ContainsKey(newNode))
            {
                neighbors.Add(newNode);
            }
        }

        return neighbors;
    }

    public int CalculateHeuristic(WorldState currentState, WorldState goalState)
    {
        int heuristicCost = 0;

        if (currentState.brokenHouse != goalState.brokenHouse) //si la casa esta sin reparar
        {
            heuristicCost++; //1 punto
        }

        if (currentState.gold < goalState.gold) //si me falta oro para llegar al goal
        {
            heuristicCost += goalState.gold - currentState.gold; //1 punto por cada gold faltante
        }

        return heuristicCost;
    }
}
