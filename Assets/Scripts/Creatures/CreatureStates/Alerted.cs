using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alerted : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;
    private readonly IState previousState;

    private float alertedDuration = 10f;

    public Alerted(GameObject gameObject, StateMachine stateMachine, IState previousState)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        this.previousState = previousState;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureSight = creatureBehaviour.creatureSight;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
    }
    
    public void Enter()
    {
        //Set trigger for Alerted animation
        creatureBehaviour.currentState = "Alerted";
        alertedDuration += Time.time;
    }

    public void Update()
    {
        ScanSurroundings();
        CheckForThreats();
        ResumePreviousState();
    }

    private void ScanSurroundings()
    {
        var positionToLookTowards = gameObject.transform.position;

        foreach (var sound in creatureHearing.heardTargets)
            positionToLookTowards = sound.position;

        if (creatureHearing.heardTargets.Count <= 0) return;
        
        alertedDuration += Time.deltaTime;
        creatureMovement.LookTowardsPosition(positionToLookTowards);
    }

    private void CheckForThreats()
    {
        if (creatureSight.visibleTargets.Count <= 0) return;

        foreach (var creature in creatureSight.visibleTargets)
        {
            var seenCreatureCharacteristics = creature.GetComponent<CreatureCharacteristics>();
            if (seenCreatureCharacteristics == null) return;
            if (seenCreatureCharacteristics.creatureTypeName == creatureCharacteristics.creatureTypeName) return;

            if (seenCreatureCharacteristics.baseThreatLevel > creatureCharacteristics.baseThreatLevel)
                /*stateMachine.ChangeState(new Fleeing(gameObject, stateMachine))*/ ;
        }
    }

    private void ResumePreviousState()
    {
        if (alertedDuration > Time.time) return;
        
        stateMachine.ChangeState(previousState);
    }

    public void PhysicsUpdate()
    {
        
    }

    public void Exit()
    {
        
    }
}
