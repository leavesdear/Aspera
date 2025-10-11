using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        player.canBeHurt = false;

        AudioManager.instance.PlaySFX(6);
    }

    public override void Update()
    {
        base.Update();

        player.rb.velocity = player.dashSpeed * player.faceDir;
        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetZeroVelocity();
        player.canBeHurt = true;
    }
}
