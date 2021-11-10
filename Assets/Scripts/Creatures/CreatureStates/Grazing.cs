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
    private readonly float grazingYield;

    private float eatingCooldown = 0;
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
        eatingDuration = Time.time + 2f/*Random.Range(10f, 30f)*/;
    }

    public void Update()
    {
        CheckForSounds();
        Eat();
        LookForNewSpot();
    }

    private void CheckForSounds()
    {
        //TODO: Add scan timer so it doesnt scan every frame
        
        if (creatureHearing.heardTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
    }

    private void Eat()
    {
        if (eatingCooldown > Time.time) return;
        eatingCooldown = Time.time + 1/creatureCharacteristics.eatingSpeed;
        
        creatureCharacteristics.AddFood(grazingYield);
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