using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private float xStop;
    private float yStop;

    public PlayerMoveState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
        if (xInput == 0 && yInput == 0)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
        else
        {
            xStop = xInput;
            yStop = yInput;
        }
        player.faceDir = new Vector2(xStop, yStop).normalized;

        player.anim.SetFloat("XInput", xStop);
        player.anim.SetFloat("YInput", yStop);

        Vector2 input = (xInput * player.transform.right + player.transform.up * yInput).normalized;
        rb.velocity = input * player.moveSpeed;



    }
}
