using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LookingForGrazingSpot : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;
    private readonly CreatureMovement creatureMovement;
    private readonly CreatureHearing creatureHearing;
    private readonly CreatureCharacteristics creatureCharacteristics;
    private readonly Collider2D creatureCollider;
    private bool hasGrazingSpot;

    public LookingForGrazingSpot(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
        creatureMovement = this.gameObject.GetComponent<CreatureMovement>();
        creatureHearing = this.gameObject.GetComponent<CreatureHearing>();
        creatureCharacteristics = this.gameObject.GetComponent<CreatureCharacteristics>();
        creatureCollider = this.gameObject.GetComponent<Collider2D>();
    }
    
    public void Enter()
    {
        //Set trigger for Walking animation
        creatureBehaviour.currentState = "Looking For Grazing Spot";
    }

    public void Update()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        SetNewGrazingSpot(other);
    }
    
    private void SetNewGrazingSpot(Collider2D other)
    {
        if (hasGrazingSpot) return;
        hasGrazingSpot = true;

        if (!other.CompareTag("GrazingZone")) return;
        var newSpotOffset = new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0).normalized;
        newSpotOffset *= Random.Range(0f, other.transform.localScale.x);
        creatureMovement.targetPosition = other.transform.position + newSpotOffset;

        CheckPathObstruction();
    }

    private void CheckPathObstruction()
    {
        //TODO: Fix bug where distance == 0
        var distance = Vector2.Distance(gameObject.transform.position, creatureMovement.targetPosition);
        var targetsInTheWay = Physics2D.Raycast(gameObject.transform.position,
            creatureMovement.targetPosition.normalized,
            distance, LayerMask.NameToLayer("Obstacles"));

        if (targetsInTheWay.collider == null) return;
        var distanceToCollision = targetsInTheWay.distance;

        if (distanceToCollision < (creatureMovement.targetPosition - gameObject.transform.position).magnitude)
        {
            creatureMovement.targetPosition = creatureMovement.targetPosition.normalized * distanceToCollision;
        }
    }

    public void Exit()
    {
        
    }
}
