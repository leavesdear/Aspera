using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(5);
        player.canBeHurt = false;
        stateTimer = player.deadBuffingTime;
    }

    public override void Exit()
    {
        base.Exit();
        player.canBeHurt = true;
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        if (player.playerStats.canReviveCounter <= 0 && stateTimer <= 0)
        {
            player.teleport.DieBack();

            player.stateMachine.ChangeState(player.idleState);
            player.playerStats.RevivePlayer();
        }
    }
}
