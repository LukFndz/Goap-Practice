using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    //metodos viejos que traje de otro tp. no tocar.

    public List<Node> AStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();
                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Add(startingNode); 
                path.Reverse();

                return path;
            }

            foreach (var next in current.GetNeighbors())
            {
                if (next.isBlocked) continue;
                int newCost = costSoFar[current] + next.cost;
                float priority = newCost + Vector3.Distance(next.transform.position, goalNode.transform.position);
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
        return new List<Node>();
    } //el que se usa para moverse.
    
    public Node GetClosestNodeToPosition(Vector3 pos)
    {
        var currentDistance = Mathf.Infinity;
        Node closestNode = default;
        
        foreach (var node in GameManager.instance.allNodes)
        {
            float nodeDistance = Vector3.Distance(pos, node.transform.position);

            if (!(nodeDistance < currentDistance)) 
                continue;
            
            currentDistance = nodeDistance;
            closestNode = node;
        }

        return closestNode;
    }
}
