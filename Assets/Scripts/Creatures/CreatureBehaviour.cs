using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    private StateMachine stateMachine;
    public CreatureMovement creatureMovement;
    public CreatureCharacteristics creatureCharacteristics;
    public CreatureSight creatureSight;
    public CreatureHearing creatureHearing;
    public Collider2D creatureCollider;
    private IState grazing;

    [Header("Debug")] 
    [Attribute_ReadOnly] public string currentState;

    private void Start()
    {
        stateMachine = new StateMachine();
        creatureMovement = gameObject.GetComponent<CreatureMovement>();
        creatureSight = gameObject.GetComponent<CreatureSight>();
        creatureHearing = gameObject.GetComponent<CreatureHearing>();
        creatureCharacteristics = gameObject.GetComponent<CreatureCharacteristics>();
        creatureCollider = gameObject.GetComponent<Collider2D>();
        grazing = new LookingForDrinkingSpot(gameObject, stateMachine);
        
        stateMachine.Initialize(grazing);
        //TODO: Initialize State dependent on characteristics
    }
    
    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}