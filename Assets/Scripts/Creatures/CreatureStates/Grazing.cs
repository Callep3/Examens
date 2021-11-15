using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Grazing : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureCharacteristics creatureCharacteristics;

    private float eatingDuration;
    
    public Grazing(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
        //Assign grazingYield from constructor
    }
    
    public void Enter()
    {
        //Set trigger for grazing animation
        creatureBehaviour.currentState = "Grazing";
        eatingDuration = Time.time + Random.Range(10f, 30f);
    }

    public void Update()
    {
        CheckForSounds();
        UpdateStats();
        LookForNewSpot();
    }
    
    private void UpdateStats()
    {
        if (creatureCharacteristics.statUpdateInterval > Time.time) return;
        creatureCharacteristics.statUpdateInterval = Time.time + creatureCharacteristics.statUpdateCooldown;
        
        creatureCharacteristics.AddFood(creatureCharacteristics.eatingSpeed);
        creatureCharacteristics.RemoveWater(creatureCharacteristics.thirstRate);

        if (creatureCharacteristics.food <= 0 || creatureCharacteristics.water <= 0)
            creatureCharacteristics.RemoveHealth(1f);
    }

    private void CheckForSounds()
    {
        //TODO: Add scan timer so it doesnt scan every frame
        
        if (creatureHearing.heardTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
    }

    private void LookForNewSpot()
    {
        if (eatingDuration > Time.time) return;
        stateMachine.ChangeState(new LookingForGrazingSpot(gameObject, stateMachine));
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