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
    private readonly CreatureCharacteristics creatureCharacteristics;

    public Alerted(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = this.gameObject.GetComponent<CreatureMovement>();
        creatureHearing = this.gameObject.GetComponent<CreatureHearing>();
        creatureCharacteristics = this.gameObject.GetComponent<CreatureCharacteristics>();
    }
    
    public void Enter()
    {
        //Set trigger for Alerted animation
        creatureBehaviour.currentState = "Alerted";
    }

    public void Update()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        
    }

    public void Exit()
    {
        
    }
}
