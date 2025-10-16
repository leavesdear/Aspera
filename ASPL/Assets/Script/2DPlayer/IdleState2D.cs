using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState2D : PlayerState2D
{
    public IdleState2D(PlayerStateMachine2D _stateMachine, Player2D _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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

        if (player.IsGroundDetected() && Input.GetKeyDown(KeyCode.W))
        {
            player.stateMachine.ChangeState(player.jumpState);
        }

        if (xInput != 0)
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }
}
