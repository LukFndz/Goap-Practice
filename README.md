# GOAP Practice (Unity)

This project is a Unity sample that demonstrates **Goal-Oriented Action Planning (GOAP)** with a simple world state (stamina, tools, house repair, and gold). The planner builds an action sequence that satisfies a goal state using A* search, and actions update the world state as they execute.【F:Assets/Scripts/GOAP/GoapPlanner.cs†L1-L71】【F:Assets/Scripts/GOAP/Actions/GoapAction.cs†L1-L38】【F:Assets/Scripts/GOAP/WorldState.cs†L1-L35】

## What this project does

At runtime, the planner evaluates the current world state and a goal state, then generates the best list of actions (by cost + heuristic) to reach the goal. The goal currently focuses on repairing the house and earning gold, while actions such as mining, resting, or picking up tools modify the world state and stamina.【F:Assets/Scripts/GOAP/GoapPlanner.cs†L7-L71】【F:Assets/Scripts/GameManager.cs†L20-L63】【F:Assets/Scripts/GOAP/Actions/PickupHammerAction.cs†L5-L31】

## Requirements

- **Unity 2021.3.12f1** (matching the project version).【F:ProjectSettings/ProjectVersion.txt†L1-L2】

## How to run the project

1. Open the repo folder in **Unity Hub**.
2. Select Unity **2021.3.12f1** when prompted.
3. Open the scene: `Assets/Scenes/SampleScene.unity`.
4. Press **Play** to run the simulation.

> The `GameManager` holds the current world state and an inspector-populated list of `GoapAction` instances. The planner uses these actions to compute a plan at runtime.【F:Assets/Scripts/GameManager.cs†L10-L49】【F:Assets/Scripts/GOAP/Actions/GoapAction.cs†L6-L30】

## GOAP API overview

This section documents the core classes that act as the project’s “API” for planning and world-state changes.

### `WorldState`

Located in `Assets/Scripts/GOAP/WorldState.cs`.

- Holds stamina, hammer status, house status, current pickaxe, and gold.
- Can be copied from another `WorldState` to apply effects safely.
【F:Assets/Scripts/GOAP/WorldState.cs†L1-L35】

### `GoapAction` (base class)

Located in `Assets/Scripts/GOAP/Actions/GoapAction.cs`.

Key fields and methods:

- `preconditions` (`Func<WorldState, bool>`) and `effects` (`Func<WorldState, WorldState>`) define when an action is valid and how it changes state.
- `cost`, `staminaCost`, and `staminaMultiplier` allow the planner and action to consider tradeoffs.
- `ExecuteAction()` applies effects to the `GameManager`’s current state (override in derived actions to add extra behavior).
【F:Assets/Scripts/GOAP/Actions/GoapAction.cs†L6-L38】

### `GoapPlanner`

Located in `Assets/Scripts/GOAP/GoapPlanner.cs`.

Key methods:

- `Plan(currentState, goalState)` returns a list of `GoapAction` steps to reach the goal.
- Internally calls `AStarGoap(...)` to search for the best sequence using cost + heuristic.
【F:Assets/Scripts/GOAP/GoapPlanner.cs†L7-L71】

### `GameManager`

Located in `Assets/Scripts/GameManager.cs`.

Key responsibilities:

- Stores `currentState` and a hard-coded `goalState` (broken house repaired and gold target).
- Tracks all available `GoapAction` instances assigned in the Unity inspector.
- Provides `GetAvailableActionsForState(state)` so the planner can filter usable actions.
【F:Assets/Scripts/GameManager.cs†L10-L60】

## How actions work

Each concrete action derives from `GoapAction` and defines:

1. **Preconditions** (e.g., enough stamina, has a tool).
2. **Effects** (e.g., change stamina, add gold, set hammer/pickaxe flags).

Example: `PickupHammerAction` requires no hammer and enough stamina, then sets `hasHammer = true` and reduces stamina on execution.【F:Assets/Scripts/GOAP/Actions/PickupHammerAction.cs†L5-L31】

## Extending the system

To add a new action:

1. Create a new class in `Assets/Scripts/GOAP/Actions/`.
2. Inherit from `GoapAction`.
3. Implement `SetUpPreconditionsAndEffects()`.
4. (Optional) Override `ExecuteAction()` for custom runtime behavior.
5. Add the action component to a GameObject in your scene and include it in the `GameManager`’s `allActions` list in the inspector.
【F:Assets/Scripts/GOAP/Actions/GoapAction.cs†L6-L38】【F:Assets/Scripts/GameManager.cs†L13-L49】

## Notes

- The goal state is currently hard-coded in `GameManager.Awake()`, so change it there if you want different objectives.【F:Assets/Scripts/GameManager.cs†L20-L34】
- Stamina costs are controlled per action and can be tuned to change the optimal plan.【F:Assets/Scripts/GOAP/Actions/GoapAction.cs†L10-L14】
