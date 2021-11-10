using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roaming : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;

    private Waypoint[] waypoints;
    private GameObject currentNode;
    private int currentWP = 0;
    private Graph graph;
    
    public Roaming(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureSight = creatureBehaviour.creatureSight;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
    }

    public void Enter()
    {
        //Set trigger for Walking animation
        creatureBehaviour.currentState = "Roaming";
        waypoints = WaypointsController.Instance.waypoints;
        graph = WaypointsController.Instance.graph;
        currentNode = graph.GetClosestWaypoint(gameObject.transform.position);

        CreatureMovement.reachedTargetPosition += IncrementWP; 
        
        DecideNeed();
    }

    private void IncrementWP(CreatureMovement obj)
    {
        if (graph.getPathLength() == 0 || currentWP == graph.getPathLength())
        {
            stateMachine.ChangeState(new LookingForGrazingSpot(gameObject, stateMachine));
            return;
        }

        currentNode = graph.getPathPoint(currentWP);
        
        currentWP++;

        if (currentWP < graph.getPathLength())
        {
            creatureMovement.targetPosition = graph.getPathPoint(currentWP).transform.position;
        }
    }

    private void DecideNeed()
    {
        GoTo(Waypoint.waypointType.GrazingSpot);
    }

    private void GoTo(GameObject waypoint)
    {
        graph.AStar(currentNode, waypoint);
        currentWP = 0;
    }

    private void GoTo(Waypoint.waypointType type)
    {
        graph.AStar(currentNode, type);
        currentWP = 0;
    }

    public void Update()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }

    public void LateUpdate()
    {
        
    }

    public void Exit()
    {
        CreatureMovement.reachedTargetPosition -= IncrementWP;
    }
}
