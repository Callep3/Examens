using Unity.Collections;
using UnityEngine;

public interface IState
{
    public void Enter();
    public void Update();
    public void PhysicsUpdate();
    public void LateUpdate();
    public void Exit();
}

#region StateMachine

public class StateMachine
{
    private IState currentState;

    public void Initialize(IState startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }
    
    public void ChangeState(IState newState)
    {
        currentState?.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    public void LateUpdate()
    {
        currentState?.LateUpdate();
    }
}

#endregion