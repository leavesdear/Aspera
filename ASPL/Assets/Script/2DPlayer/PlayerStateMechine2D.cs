using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine2D : MonoBehaviour
{
    public PlayerState2D currentState { get; private set; }

    public void Initialize(PlayerState2D _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState2D _newState)
    {
        currentState.Exit();
        currentState = _newState;
        _newState.Enter();
    }
}
