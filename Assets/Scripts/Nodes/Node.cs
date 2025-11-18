using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Node : GridEntity
{
	[Space(10)]
	[Header("Node Parameters")]
    [SerializeField]
    protected List<Node> _neighbors = new List<Node>();

    [SerializeField] protected int maxNeighbors = 4;
    [SerializeField] protected float maxNeighborDistance = 15;

    public bool isBlocked;
    public int cost = 1;

    void Start()
	{
		GameManager.instance.AddNode(this);
	}
    
    public abstract List<Node> GetNeighbors();

}
