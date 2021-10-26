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

    private float drinkingCooldown = 0;
    
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
        CheckForThreats();
        Drink();
    }

    private void CheckForThreats()
    {
        if (creatureHearing.heardTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine));
    }
    
    private void Drink()
    {
        if (drinkingCooldown > Time.time) return;
        drinkingCooldown = Time.time + 1/creatureCharacteristics.eatingSpeed;
        
        creatureCharacteristics.AddWater(5);
    }

    public void PhysicsUpdate()
    {
        
    }

    public void Exit()
    {
        
    }
}
