using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nesting : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;

    public Nesting(GameObject gameObject, StateMachine stateMachine)
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
        //Set trigger for Walking animation
        creatureBehaviour.currentState = "Nesting";

        PickASpot();
    }

    public void Update()
    {
        JitterFix();
        CheckForSounds();
        CheckToRest();
        UpdateStats();
    }

    private void PickASpot()
    {
        List<GameObject> nestingZones = new List<GameObject>();

        var other = Physics2D.OverlapPointAll(gameObject.transform.position, 1 << 3);
        foreach (var collider in other)
        {
            if (!collider.CompareTag("NestingZone")) continue;
            
            nestingZones.Add(collider.gameObject);
        }
        var nestingZone = nestingZones[Random.Range(0, nestingZones.Count)];

        if (nestingZone == null)
        {
            Debug.LogError("NoNestingZoneFound");
            return;
        }
        
        var newSpotOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        newSpotOffset *= Random.Range(0f, nestingZone.transform.localScale.x / 2);
        creatureMovement.SetTargetPosition(nestingZone.transform.position + newSpotOffset);
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

    private void CheckToRest()
    {
        if (creatureCharacteristics.health < creatureCharacteristics.maxHealth ||
            creatureCharacteristics.GetLowestStat() == CreatureStats.energy)
            stateMachine.ChangeState(new Sleeping(gameObject, stateMachine));
        else
            stateMachine.ChangeState(new Roaming(gameObject, stateMachine));
    }

    private void UpdateStats()
    {
        if (creatureCharacteristics.statUpdateInterval > Time.time) return;
        creatureCharacteristics.statUpdateInterval = Time.time + creatureCharacteristics.statUpdateCooldown;
        
        creatureCharacteristics.RemoveFood(creatureCharacteristics.hungerRate);
        creatureCharacteristics.RemoveWater(creatureCharacteristics.thirstRate);
        creatureCharacteristics.RemoveEnergy(creatureCharacteristics.drainRate);

        if (creatureCharacteristics.food <= 0 || creatureCharacteristics.water <= 0)
            creatureCharacteristics.RemoveHealth(1f);
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
