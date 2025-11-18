using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NavigationNode : Node
{
    public override List<Node> GetNeighbors()
    {
        var myPosition = transform.position;
        
        var nodes = _query.Query()
            .OfType<Node>()
            .Where(x => (transform.position - x.transform.position).sqrMagnitude <= _query.radius * _query.radius)
            .ToList();
        
        var neighbors = nodes
            .Where(node => node != this && !node.isBlocked)
            .OrderBy(node => Vector3.Distance(myPosition, node.transform.position))
            .TakeWhile(node => Vector3.Distance(myPosition, node.transform.position) <= maxNeighborDistance)
            .Take(maxNeighbors)
            .ToList();
        
        return neighbors;
    }
}
