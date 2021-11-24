using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drinking : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;

    public Drinking(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureSight = creatureBehaviour.creatureSight;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
    }

    public void Enter()
    {
        //Set trigger for drinking animation
        creatureBehaviour.currentState = "Drinking";
        
        creatureMovement.SetTargetPosition(gameObject.transform.position);
    }

    public void Update()
    {
        CheckForSounds();
        UpdateStats();
    }

    private void CheckForSounds()
    {
        if (creatureHearing.checkInterval > Time.time) return;
        creatureHearing.checkInterval = Time.time + creatureHearing.checkCooldown;

        if (creatureHearing.heardHostileTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
    }

    private void UpdateStats()
    {
        if (creatureCharacteristics.statUpdateInterval > Time.time) return;
        creatureCharacteristics.statUpdateInterval = Time.time + creatureCharacteristics.statUpdateCooldown;
        
        creatureCharacteristics.RemoveFood(creatureCharacteristics.hungerRate);
        creatureCharacteristics.AddWater(creatureCharacteristics.drinkingSpeed);

        if (creatureCharacteristics.food <= 0 || creatureCharacteristics.water <= 0)
            creatureCharacteristics.RemoveHealth(1f);

        if (creatureCharacteristics.water >= creatureCharacteristics.maxWater)
            stateMachine.ChangeState(new Roaming(gameObject, stateMachine));
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
