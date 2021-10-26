using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LookingForGrazingSpot : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureSight creatureSight;
    private readonly CreatureCharacteristics creatureCharacteristics;
    private readonly Collider2D creatureCollider;
    private bool hasGrazingSpot;

    public LookingForGrazingSpot(GameObject gameObject, StateMachine stateMachine)
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
        creatureBehaviour.currentState = "Looking For Grazing Spot";

        SetNewGrazingSpot();
        CreatureMovement.reachedTargetPosition += StartGrazing;
    }

    private void StartGrazing(CreatureMovement creatureMovement)
    {
        if (creatureMovement.gameObject == gameObject && hasGrazingSpot)
            stateMachine.ChangeState(new Grazing(gameObject, stateMachine));
    }

    public void Update()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }
    
    private void SetNewGrazingSpot()
    {
        if (hasGrazingSpot) return;
        hasGrazingSpot = true;
        List<GameObject> grazingZones = new List<GameObject>();

        var other = Physics2D.OverlapPointAll(gameObject.transform.position, 1 << 3);
        foreach (var collider in other)
        {
            if (!collider.CompareTag("GrazingZone")) continue;
            
            grazingZones.Add(collider.gameObject);
        }
        var grazingZone = grazingZones[Random.Range(0, grazingZones.Count)];

        if (grazingZone == null)
        {
            Debug.LogError("NoGrazingZoneFound");
            return;
        }
        
        var newSpotOffset = new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0).normalized;
        newSpotOffset *= Random.Range(0f, grazingZone.transform.localScale.x / 2);
        creatureMovement.SetTargetPosition(grazingZone.transform.position + newSpotOffset);
    }

    public void Exit()
    {
        CreatureMovement.reachedTargetPosition -= StartGrazing;
    }
}
