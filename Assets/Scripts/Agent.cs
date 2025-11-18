using System;
using System.Collections.Generic;
using UnityEngine;

public class Agent : GridEntity
{
    public float moveSpeed = 10;
    
    [HideInInspector] public List<Node> _pathToFollow = new List<Node>(); //estos 2 son para el movimiento nomas
    [HideInInspector] public Pathfinding _pf = new Pathfinding();

    [HideInInspector] public GoapPlanner _planner = new GoapPlanner();
    [HideInInspector] public Queue<GoapAction> actionQueue = new Queue<GoapAction>(); //la lista que me da el planner
    [HideInInspector] public GoapAction currentAction; //la que me toca ejecutar ahora
    [HideInInspector] public GameObject target; //algunas action requieren que me mueva hasta una posicion

    public enum AgentState { Plan, Move, ExecuteAction}

    private EventFSM<AgentState> _fsm;
    private Node targetNode;
    private Node startingNode;

    private void Start()
    {
        var plan = new State<AgentState>("PlanState");
        var move = new State<AgentState>("MoveState");
        var executeAction = new State<AgentState>("ExecuteActionState");

        StateConfigurer.Create(plan)
           .SetTransition(AgentState.Move, move)
           .SetTransition(AgentState.ExecuteAction, executeAction)
           .Done(); //aplico y asigno

        StateConfigurer.Create(move)
           .SetTransition(AgentState.ExecuteAction, executeAction)
           .Done(); //aplico y asigno

        StateConfigurer.Create(executeAction)
           .SetTransition(AgentState.Plan, plan)
           .Done(); //aplico y asigno


        plan.OnEnter += x =>
        {
            if (actionQueue.Count == 0) //si no tengo plan
            {
                SetPlan();
            }
        };

        plan.OnUpdate += () =>
        {
            FollowPlan();
        };

        move.OnEnter += x =>
        {
            startingNode = _pf.GetClosestNodeToPosition(transform.position);
            targetNode = _pf.GetClosestNodeToPosition(target.transform.position);
            _pathToFollow = _pf.AStar(startingNode, targetNode);
        };

        move.OnUpdate += () =>
        {
            if (FollowPath())
            {
                //Debug.Log("follow path dio true, debo cambiar a execute action");
                _fsm.SendInput(AgentState.ExecuteAction);
            }
        };

        executeAction.OnEnter += x =>
        {
            ExecuteAction();
        };

        _fsm = new EventFSM<AgentState>(plan);
    }

    private void Update()
    {
        _fsm.Update();
    }

    #region PLAN STATE

    private void SetPlan()
    {
        actionQueue = new Queue<GoapAction>(_planner.Plan(GameManager.instance.currentState, GameManager.instance.goalState)); //pido el plan al planner
    }

    private void FollowPlan()
    {
        if (actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();

            if (currentAction.requiresMovement)
            {
                target = currentAction.target; //establezco mi target

                //cambiar a move
                _fsm.SendInput(AgentState.Move);

            }
            else
            {
                //ejecuto de una
                _fsm.SendInput(AgentState.ExecuteAction);
            }
        }
    }

    #endregion

    #region MOVE STATE
    private bool FollowPath()
    {
        Vector3 nextPos = _pathToFollow[0].transform.position;
        Vector3 dir = nextPos - transform.position;
        transform.forward = dir;
        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        if (dir.magnitude < 0.1f)
        {
            _pathToFollow.RemoveAt(0);
        }

        if (_pathToFollow.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region EXECUTE STATE
    private void ExecuteAction()
    {
        currentAction.ExecuteAction();
        _fsm.SendInput(AgentState.Plan);
    }
    #endregion
}
