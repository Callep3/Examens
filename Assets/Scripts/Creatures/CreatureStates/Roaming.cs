using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private CreatureStats currentNeed;
    
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
            ChangeState();
            return;
        }

        currentNode = graph.getPathPoint(currentWP);
        
        currentWP++;

        if (currentWP < graph.getPathLength())
        {
            creatureMovement.targetPosition = graph.getPathPoint(currentWP).transform.position;
        }
    }

    private void ChangeState()
    {
        switch (currentNeed)
        {
            case CreatureStats.none:
                stateMachine.ChangeState(new Nesting(gameObject, stateMachine));
                break;
            
            case CreatureStats.food:
                if (creatureCharacteristics.herbivore)
                {
                    stateMachine.ChangeState(new LookingForGrazingSpot(gameObject, stateMachine));
                }

                if (creatureCharacteristics.carnivore)
                {
                    stateMachine.ChangeState(new Roaming(gameObject, stateMachine));
                }
                break;
            
            case CreatureStats.water:
                stateMachine.ChangeState(new LookingForDrinkingSpot(gameObject, stateMachine));
                break;
            
            case CreatureStats.energy:
                stateMachine.ChangeState(new Nesting(gameObject, stateMachine));
                break;
            
            case CreatureStats.health:
                stateMachine.ChangeState(new Nesting(gameObject, stateMachine));
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DecideNeed()
    {
        currentNeed = creatureCharacteristics.GetLowestStat();
        
        switch (currentNeed)
        {
            case CreatureStats.none:
                GoTo(Waypoint.waypointType.NestingSpot);
                break;
            
            case CreatureStats.food:
                if (creatureCharacteristics.herbivore)
                {
                    GoTo(Waypoint.waypointType.GrazingSpot);
                }

                if (creatureCharacteristics.carnivore)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            GoTo(Waypoint.waypointType.GrazingSpot);
                            break;
                        case 1:
                            GoTo(Waypoint.waypointType.DrinkingSpot);
                            break;
                    }
                }
                break;
            
            case CreatureStats.water:
                GoTo(Waypoint.waypointType.DrinkingSpot);
                break;
            
            case CreatureStats.energy:
                GoTo(Waypoint.waypointType.NestingSpot);
                break;
            
            case CreatureStats.health:
                GoTo(Waypoint.waypointType.NestingSpot);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
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
        UpdateStats();
    }
    
    private void UpdateStats()
    {
        if (creatureCharacteristics.statUpdateInterval > Time.time) return;
        creatureCharacteristics.statUpdateInterval = Time.time + creatureCharacteristics.statUpdateCooldown;
        
        creatureCharacteristics.RemoveFood(creatureCharacteristics.hungerRate);
        creatureCharacteristics.RemoveWater(creatureCharacteristics.thirstRate);
        creatureCharacteristics.RemoveEnergy(creatureCharacteristics.drainRate);

        if (creatureCharacteristics.food <= 0 || creatureCharacteristics.water <= 0)
            creatureCharacteristics.RemoveHealth(1f);
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
