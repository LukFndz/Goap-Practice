using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Node> allNodes = new List<Node>(); //estos son los nodos de movimiento nomas. se cargan solos. nada q ver con goap.

    public List<GoapAction> allActions = new List<GoapAction>(); //las cargo en el inspector una x una
    public WorldState currentState; //el estado inicial, y que se va a ir actualizando a medida que ejecuto tareas exitosamente
    [HideInInspector] public WorldState goalState;

    private SpatialGrid _spatialGrid;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        goalState = new WorldState(0, true, false, Pickaxe.None, 10); //HARDCODEADO EL GOALSTATE. esto es porque el chequeo solo chequea por brokenhouses y gold, por ahora
        
        _spatialGrid = FindObjectOfType<SpatialGrid>();
    }

    public void AddNode(Node node)
    {
        allNodes.Add(node);
        _spatialGrid.AddEntityToGrid(node);
        //print("agregue el nodo " + node.gameObject.name + " a la gran lista de nodos");
    }

    public List<GoapAction> GetAvailableActionsForState(WorldState state) //le pasas un estado y te devuelve que acciones podrias tomar
    {
        //Debug.Log("GetAvailableActionsForState: tom�");

        return allActions
            .Where(x => x.CheckPreconditions(state))
            .ToList(); ;
    }

    public bool IsGoalState(WorldState current, WorldState goal) //compara si el estado actual es igual al goal
    {
        if (current.brokenHouse == goal.brokenHouse && current.gold == goal.gold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public SpatialGrid GetSpatialGrid()
    {
        return _spatialGrid;
    }
}
