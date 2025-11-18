using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoapPlanner
{
    public List<GoapAction> Plan(WorldState currentState, WorldState goalState)
    {
        Debug.Log("GoapPlanner: Planning...");
        List<GoapAction> elMejorPlan = new List<GoapAction>();

        if (GameManager.instance.IsGoalState(currentState, goalState))
        {
            Debug.Log("GoapPlanner: We Win");
        }
        else
        {
            List<GoapNode> path = AStarGoap(new GoapNode(currentState, null), new GoapNode(goalState, null));

            elMejorPlan = path
                .Where(x => x.goapAction != null)
                .Select(x => x.goapAction)
                .ToList();
        }

        return elMejorPlan;
        
    }

    public List<GoapNode> AStarGoap(GoapNode startingNode, GoapNode goalNode)
    {
        Debug.Log("AStar: Start");

        if (startingNode == null || goalNode == null) return new List<GoapNode>();

        PriorityQueue<GoapNode> frontier = new PriorityQueue<GoapNode>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<GoapNode, GoapNode> cameFrom = new Dictionary<GoapNode, GoapNode>();
        cameFrom.Add(startingNode, null);

        Dictionary<GoapNode, int> costSoFar = new Dictionary<GoapNode, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            GoapNode current = frontier.Dequeue();

            if (GameManager.instance.IsGoalState(current.currentState, goalNode.currentState))
            {
                List<GoapNode> path = new List<GoapNode>();
                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Add(startingNode);
                path.Reverse();

                return path;
            }

            foreach (GoapNode next in current.GetNeighbors(cameFrom))
            {
                int newCost = costSoFar[current] + next.goapAction.cost;
                float heuristicCost = next.CalculateHeuristic(current.currentState, goalNode.currentState);
                float priority = newCost + heuristicCost;

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }
        return new List<GoapNode>();
    }
}


