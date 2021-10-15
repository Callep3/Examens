using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    private StateMachine stateMachine;
    private CreatureCharacteristics creatureCharacteristics;
    private IState grazing;

    [Header("Debug")] 
    [Attribute_ReadOnly] public string currentState;

    void Start()
    {
        stateMachine = new StateMachine();
        grazing = new Grazing(gameObject, stateMachine);
        
        stateMachine.Initialize(grazing);
        //TODO: Initialize State dependent on characteristics
    }
    
    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
