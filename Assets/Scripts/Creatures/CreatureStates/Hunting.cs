using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunting : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;

    private Transform target;

    public Hunting(Transform newTarget, GameObject gameObject, StateMachine stateMachine)
    {
        target = newTarget;
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
        creatureBehaviour.currentState = "Hunting";
        
        creatureMovement.SetTarget(target);
    }

    public void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        if (creatureCharacteristics.statUpdateInterval > Time.time) return;
        creatureCharacteristics.statUpdateInterval = Time.time + creatureCharacteristics.statUpdateCooldown;
        
        creatureCharacteristics.RemoveFood(creatureCharacteristics.hungerRate);
        creatureCharacteristics.RemoveWater(creatureCharacteristics.thirstRate);
        creatureCharacteristics.RemoveEnergy(creatureCharacteristics.drainRate * 1.5f);

        if (creatureCharacteristics.food <= 0 || creatureCharacteristics.water <= 0)
            creatureCharacteristics.RemoveHealth(1f);
    }

    public void PhysicsUpdate()
    {
        
    }

    public void LateUpdate()
    {
        Catch();
    }
    
    private void Catch()
    {
        if (creatureMovement.target == null)
            stateMachine.ChangeState(new Roaming(gameObject, stateMachine));
        if (creatureMovement.CheckDistanceToTarget() > target.localScale.x) return;

        creatureCharacteristics.AddFood( 100f);
        target.gameObject.SetActive(false);
        UIController.Instance.ChangeNumberOfHerbivores(-1);
        stateMachine.ChangeState(new Roaming(gameObject, stateMachine));
    }

    public void Exit()
    {
        creatureMovement.SetTarget(null);
    }
}
