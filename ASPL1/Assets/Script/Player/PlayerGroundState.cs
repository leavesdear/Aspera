using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
        //if (Input.GetMouseButtonDown(1))
        //{
        //    player.stateMachine.ChangeState(player.attackState);
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.stateMachine.ChangeState(player.dashState);
        //}
    }
}
