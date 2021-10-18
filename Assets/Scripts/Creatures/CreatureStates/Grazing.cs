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
    private readonly CreatureCharacteristics creatureCharacteristics;
    private readonly float grazingYield;

    private float eatingCooldown = 0;
    private float eatingDuration;
    
    public Grazing(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureCharacteristics = this.gameObject.GetComponent<CreatureCharacteristics>();
        //Assign grazingYield from constructor
    }
    
    public void Enter()
    {
        //Set trigger for grazing animation
        creatureBehaviour.currentState = "Grazing";
        //Subscribe to alert event
        eatingDuration = Time.time + Random.Range(10f, 30f);
    }

    public void Update()
    {
        CheckForThreats();
        Eat();
        LookForNewSpot();
    }

    private void CheckForThreats()
    {
        
    }

    private void Eat()
    {
        if (eatingCooldown > Time.time) return;
        eatingCooldown = Time.time + 1/creatureCharacteristics.eatingSpeed;
        
        creatureCharacteristics.AddFood(grazingYield);
    }
    
    private void LookForNewSpot()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }

    public void Exit()
    {
        //Unsubscribe to alert event
    }
}