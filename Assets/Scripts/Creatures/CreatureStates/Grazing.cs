using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Grazing : IState
{
    private readonly StateMachine stateMachine;
    private readonly GameObject gameObject;
    private readonly CreatureBehaviour creatureBehaviour;

    public Grazing(GameObject gameObject, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
        creatureBehaviour = this.gameObject.GetComponent<CreatureBehaviour>();
    }
    
    public void Enter()
    {
        creatureBehaviour.currentState = "Grazing";
    }

    public void Update()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }

    public void Exit()
    {
        
    }
}
