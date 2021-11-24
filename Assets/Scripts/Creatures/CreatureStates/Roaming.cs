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
    private List<Node> pathList = new List<Node>();
    
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

        DecideNeed();
    }

    private void DecideNeed()
    {
        currentNeed = creatureCharacteristics.GetLowestStat();
        
        switch (currentNeed)
        {
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
                            if (creatureCharacteristics.previousWaypointType == Waypoint.waypointType.GrazingSpot)
                            {
                                DecideNeed();
                                break;
                            }
                            GoTo(Waypoint.waypointType.GrazingSpot);
                            break;
                        case 1:
                            if (creatureCharacteristics.previousWaypointType == Waypoint.waypointType.DrinkingSpot)
                            {
                                DecideNeed();
                                break;
                            }
                            GoTo(Waypoint.waypointType.DrinkingSpot);
                            break;
                    }
                }
                break;
            
            case CreatureStats.water:
                GoTo(Waypoint.waypointType.DrinkingSpot);
                break;
            
            case CreatureStats.energy:
                if (creatureCharacteristics.herbivore)
                    GoTo(Waypoint.waypointType.NestingSpot);

                if (creatureCharacteristics.carnivore)
                    GoTo(Waypoint.waypointType.CarnivoreNestingSpot);
                break;
            
            case CreatureStats.health:
                if (creatureCharacteristics.herbivore)
                    GoTo(Waypoint.waypointType.NestingSpot);

                if (creatureCharacteristics.carnivore)
                    GoTo(Waypoint.waypointType.CarnivoreNestingSpot);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GoTo(GameObject waypoint)
    {
        pathList = graph.AStar(currentNode, waypoint);
        currentWP = 0;
    }

    private void GoTo(Waypoint.waypointType type)
    {
        pathList = graph.AStar(currentNode, type);
        creatureCharacteristics.previousWaypointType = type;
        creatureMovement.targetPosition = graph.getPathPoint(currentWP, pathList).transform.position;
        currentWP = 0;
    }

    public void Update()
    {
        IncrementWP();
        UpdateStats();
        CheckForSounds();
        CheckForPrey();
    }

    private void IncrementWP()
    {
        if (!creatureMovement.CheckProximity()) return;

        if (graph.getPathLength(pathList) == 0 || currentWP == graph.getPathLength(pathList))
        {
            ChangeState();
            return;
        }

        currentNode = graph.getPathPoint(currentWP, pathList);
        
        currentWP++;

        if (currentWP < graph.getPathLength(pathList))
        {
            creatureMovement.targetPosition = graph.getPathPoint(currentWP, pathList).transform.position;
        }
    }

    private void ChangeState()
    {
        switch (currentNeed)
        {
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
    
    private void CheckForSounds()
    {
        if (creatureHearing.checkInterval > Time.time) return;
        creatureHearing.checkInterval = Time.time + creatureHearing.checkCooldown;

        if (creatureHearing.heardHostileTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
    }
    
    private void CheckForPrey()
    {
        if (!creatureCharacteristics.carnivore) return;
        if (creatureSight.checkInterval > Time.time) return;
        creatureSight.checkInterval = Time.time + creatureSight.checkCooldown;
        if (creatureSight.visibleTargets.Count < 0) return;

        foreach (var target in creatureSight.visibleTargets)
        {
            var targetCharacteristics = target.GetComponent<CreatureCharacteristics>();
            if (targetCharacteristics.creatureTypeName == creatureCharacteristics.creatureTypeName) return;
            if (targetCharacteristics.baseThreatLevel >= creatureCharacteristics.baseThreatLevel) return;
            
            stateMachine.ChangeState(new Hunting(target, gameObject, stateMachine));
        }
    }

    public void PhysicsUpdate()
    {
        
    }

    public void LateUpdate()
    {
        
    }

    public void Exit()
    {
        
    }
}
