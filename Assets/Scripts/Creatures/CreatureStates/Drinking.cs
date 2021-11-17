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
    private readonly CreatureCharacteristics creatureCharacteristics;

    public Drinking(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
    }

    public void Enter()
    {
        //Set trigger for drinking animation
        creatureBehaviour.currentState = "Drinking";
    }

    public void Update()
    {
        CheckForSounds();
        UpdateStats();
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

    private void CheckForSounds()
    {
        if (creatureHearing.heardTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
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
