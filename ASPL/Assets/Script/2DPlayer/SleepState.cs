using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : PlayerState2D
{
    public SleepState(PlayerStateMachine2D _stateMachine, Player2D _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
