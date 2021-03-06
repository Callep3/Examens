using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForDrinkingSpot : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;
    private readonly Collider2D creatureCollider;
    private bool hasDrinkingSpot;

    public LookingForDrinkingSpot(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = creatureBehaviour.creatureMovement;
        creatureSight = creatureBehaviour.creatureSight;
        creatureHearing = creatureBehaviour.creatureHearing;
        creatureCharacteristics = creatureBehaviour.creatureCharacteristics;
        creatureCollider = creatureBehaviour.creatureCollider;
    }
    
    public void Enter()
    {
        //Set trigger for Walking animation
        creatureBehaviour.currentState = "Looking For Drinking Spot";

        SetNewDrinkingSpot();
    }

    public void Update()
    {
        UpdateStats();
        StartDrinking();
        CheckForSounds();
    }

    private void StartDrinking()
    {
        if (!creatureMovement.CheckProximity()) return;
        if (creatureMovement.gameObject == gameObject && hasDrinkingSpot)
            stateMachine.ChangeState(new Drinking(gameObject, stateMachine));
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
    
    private void CheckForSounds()
    {
        if (creatureHearing.checkInterval > Time.time) return;
        creatureHearing.checkInterval = Time.time + creatureHearing.checkCooldown;

        if (creatureHearing.heardHostileTargets.Count > 0)
            stateMachine.ChangeState(new Alerted(gameObject, stateMachine, this));
    }

    public void PhysicsUpdate()
    {
        
    }

    public void LateUpdate()
    {
        
    }

    private void SetNewDrinkingSpot()
    {
        if (hasDrinkingSpot) return;
        hasDrinkingSpot = true;
        List<GameObject> drinkingZones = new List<GameObject>();

        var other = Physics2D.OverlapPointAll(gameObject.transform.position, 1 << 3);
        foreach (var collider in other)
        {
            if (!collider.CompareTag("DrinkingZone")) continue;
            
            drinkingZones.Add(collider.gameObject);
        }
        var drinkingZone = drinkingZones[Random.Range(0, drinkingZones.Count)];

        if (drinkingZone == null)
        {
            Debug.LogError("NoDrinkingZoneFound");
            return;
        }
        
        var newSpotOffset = new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0).normalized;
        //Drink from the edge of the zone
        newSpotOffset *= drinkingZone.transform.localScale.x / 2;
        creatureMovement.SetTargetPosition(drinkingZone.transform.position + newSpotOffset);
    }

    public void Exit()
    {
        
    }
}
