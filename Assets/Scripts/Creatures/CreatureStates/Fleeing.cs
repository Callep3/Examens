using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fleeing : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;
    private readonly Collider2D creatureCollider;
    
    private List<CreatureCharacteristics> threats = new List<CreatureCharacteristics>();
    private int numberOfLineChecks = 10;
    private float baseObstacleCheckDistance = 2;
    private float checkCooldown;
    private float checkInterval = 0.2f;

    public Fleeing(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureSight = creatureBehaviour.creatureSight;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
        creatureCollider = creatureBehaviour.creatureCollider;
    }

    public void Enter()
    {
        //Set trigger for Running animation
        creatureBehaviour.currentState = "Fleeing";
    }

    public void Update()
    {
        CheckForBiggestThreat();
    }

    private void CheckForBiggestThreat()
    {
        if (checkCooldown > Time.time) return;
        checkCooldown = Time.time + checkInterval;
        
        CheckForDoubles();
        
        CheckForClosestThreat();
    }

    private void CheckForDoubles()
    {
        Collider2D[] nearbyThreats = Physics2D.OverlapCircleAll(
            gameObject.transform.position, 
            creatureSight.viewRadius, 
            creatureSight.targetMask
        );

        if (nearbyThreats.Length <= 1)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, new Roaming(gameObject, stateMachine)));

        for (int i = 0; i < nearbyThreats.Length; i++)
        {
            if (nearbyThreats[i] == creatureCollider) continue;
            CreatureCharacteristics threatCharacteristics = nearbyThreats[i].GetComponent<CreatureCharacteristics>();

            if (threatCharacteristics == null) continue;
            if (threatCharacteristics.baseThreatLevel < creatureCharacteristics.baseThreatLevel) continue;
            if (threats.Contains(threatCharacteristics)) continue;

            threats.Add(threatCharacteristics);
        }
    }

    private void CheckForClosestThreat()
    {
        if (threats.Count <= 0) return;

        var distanceToNearestThreat = (threats[0].transform.position - gameObject.transform.position).sqrMagnitude;
        var nearestThreat = threats[0];
        for (int i = 1; i < threats.Count; i++)
        {
            var distanceToThreat = (threats[i].transform.position - gameObject.transform.position).sqrMagnitude;

            if (distanceToThreat >= distanceToNearestThreat) continue;

            nearestThreat = threats[i];
            distanceToNearestThreat = distanceToThreat;
        }

        FleeFromThreat(nearestThreat);
    }

    private void FleeFromThreat(CreatureCharacteristics nearestThreat)
    {
        var angleOffset = 180f / numberOfLineChecks;
        var directionAwayFromThreat = (gameObject.transform.position - nearestThreat.transform.position).normalized;
        int pathToChoose = 0;
        for (int i = 0; i < numberOfLineChecks; i++)
        {
            if (i % 2 == 0)
                angleOffset *= -1f;

            if (Physics2D.CapsuleCast(
                gameObject.transform.position,
                gameObject.transform.localScale,
                CapsuleDirection2D.Vertical,
                Vector2.Angle(gameObject.transform.position, directionAwayFromThreat) + angleOffset * ((i + 1) / 2),
                directionAwayFromThreat,
                baseObstacleCheckDistance / ((i + 1) / 2),
                creatureSight.obstacleMask))
            {
                pathToChoose = i + 1;
            }
        }

        Debug.Log(pathToChoose);
        if (pathToChoose % 2 == 0)
            angleOffset *= -1f;

        directionAwayFromThreat = Quaternion.Euler(0, 0, angleOffset * pathToChoose) * directionAwayFromThreat;
        
        creatureMovement.SetTargetPosition(directionAwayFromThreat + gameObject.transform.position);
    }

    public void PhysicsUpdate()
    {
        
    }

    public void Exit()
    {
        
    }
}
