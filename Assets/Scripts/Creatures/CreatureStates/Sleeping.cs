using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleeping : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;

    public Sleeping(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureSight = creatureBehaviour.creatureSight;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
    }
    
    public void Enter()
    {
        //Set trigger for Sleeping animation
        creatureBehaviour.currentState = "Sleeping";
        Propagate();
    }

    public void Update()
    {
        CheckForSounds();
        UpdateStats();
    }

    private void Propagate()
    {
        if (creatureCharacteristics.food < creatureCharacteristics.maxFood * 0.8f) return;
        if (creatureCharacteristics.energy < creatureCharacteristics.maxEnergy * 0.8f) return;
        
        var zone = Physics2D.OverlapPoint(gameObject.transform.position, 1 << 3);
        var creaturesInZone = Physics2D.OverlapCircleAll(zone.transform.position, zone.transform.localScale.x, 1 << 9);

        foreach (var creature in creaturesInZone)
        {
            if (creature.gameObject == gameObject) continue;
            var characteristics = creature.gameObject.GetComponent<CreatureCharacteristics>();
            if (characteristics.creatureTypeName != creatureCharacteristics.creatureTypeName) continue;
            if (characteristics.gender == creatureCharacteristics.gender) continue;

            var baby = ObjectPooler.Instance.GetPooledObject("Creature");
            baby.transform.position = gameObject.transform.position;
            baby.SetActive(true);

            creatureCharacteristics.food -= creatureCharacteristics.maxFood / 2;
            creatureCharacteristics.energy -= creatureCharacteristics.maxEnergy / 2;
        }
    }

    private void CheckForSounds()
    {
        if (creatureHearing.checkInterval > Time.time) return;
        creatureHearing.checkInterval = Time.time + creatureHearing.checkCooldown;

        if (creatureHearing.heardTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
    }

    private void UpdateStats()
    {
        if (creatureCharacteristics.statUpdateInterval > Time.time) return;
        creatureCharacteristics.statUpdateInterval = Time.time + creatureCharacteristics.statUpdateCooldown;
        
        creatureCharacteristics.RemoveFood(creatureCharacteristics.hungerRate);
        creatureCharacteristics.RemoveWater(creatureCharacteristics.thirstRate);
        creatureCharacteristics.AddEnergy(creatureCharacteristics.restingSpeed);
        creatureCharacteristics.AddHealth(creatureCharacteristics.restingSpeed);

        if (creatureCharacteristics.energy >= creatureCharacteristics.maxEnergy &&
            creatureCharacteristics.health >= creatureCharacteristics.maxHealth)
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
