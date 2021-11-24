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
    private IState roaming;

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
        roaming = new Roaming(gameObject, stateMachine);
        
        stateMachine.Initialize(roaming);
        
        if (creatureCharacteristics.carnivore)
            UIController.Instance.ChangeNumberOfCarnivores(1);
        else if (creatureCharacteristics.herbivore)
            UIController.Instance.ChangeNumberOfHerbivores(1);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }
}