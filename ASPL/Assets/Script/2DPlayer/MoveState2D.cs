using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState2D : PlayerState2D
{
    public MoveState2D(PlayerStateMachine2D _stateMachine, Player2D _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.GetComponent<AdvancedSquashStretch>().isMoving = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.GetComponent<AdvancedSquashStretch>().isMoving = false;
    }

    public override void Update()
    {
        base.Update();
        if (xInput == 0 && player.IsGroundDetected())
        {
            player.stateMachine.ChangeState(player.idleState);
        }

        Vector2 input = new Vector2(xInput, 0).normalized;
        rb.velocity = input * player.moveSpeed;
    }
}
