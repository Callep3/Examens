using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sleeping : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;

    private float spawn;

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
        JitterFix();
        CheckForSounds();
        UpdateStats();
    }

    private void Propagate()
    {
        if (creatureCharacteristics.gender != Gender.female) return;
        if (creatureCharacteristics.food < creatureCharacteristics.maxFood * 0.8f) return;

        List<GameObject> nestingZones = new List<GameObject>();
        var zones = Physics2D.OverlapPointAll(gameObject.transform.position, 1 << 3);
        foreach (var zone in zones)
        {
            if (!zone.CompareTag("NestingZone")) continue;
            
            nestingZones.Add(zone.gameObject);
        }
        var nestingZone = nestingZones[Random.Range(0, nestingZones.Count)];
        var creaturesInZone = Physics2D.OverlapCircleAll(nestingZone.transform.position, nestingZone.transform.localScale.x, 1 << 9);

        foreach (var creature in creaturesInZone)
        {
            if (creature.gameObject == gameObject) continue;
            var characteristics = creature.gameObject.GetComponent<CreatureCharacteristics>();
            if (characteristics.creatureTypeName != creatureCharacteristics.creatureTypeName) continue;
            if (characteristics.gender == creatureCharacteristics.gender) continue;

            var baby = ObjectPooler.Instance.GetPooledObject(creatureCharacteristics.carnivore ? "Carnivore" : "Herbivore");
            baby.transform.position = gameObject.transform.position;
            baby.GetComponent<CreatureCharacteristics>().SetVariables(creatureCharacteristics);
            if (creatureCharacteristics.carnivore)
                UIController.Instance.ChangeNumberOfCarnivores(1);
            else if (creatureCharacteristics.herbivore)
                UIController.Instance.ChangeNumberOfHerbivores(1);
            baby.SetActive(true);
            
            creatureCharacteristics.RemoveFood(creatureCharacteristics.maxFood / 2);
            break;
        }
    }
    
    private void JitterFix()
    {
        if (creatureMovement.CheckProximity())
            creatureMovement.SetTargetPosition(gameObject.transform.position);
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
