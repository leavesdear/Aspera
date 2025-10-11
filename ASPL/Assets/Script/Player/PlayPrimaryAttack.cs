using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPrimaryAttack : PlayerState
{
    private int comboCounter;

    private float lastTimeAttack;
    private float comboWindow = 1f;
    public PlayPrimaryAttack(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.canAttack = false;

        //player.StartCoroutine("BusyFor", .5f);

        //if (comboCounter > 2 || lastTimeAttack + comboWindow <= Time.time)
        //    comboCounter = 0;

        //player.anim.SetInteger("comboCounter", comboCounter);

        player.attackDir = player.faceDir;
        if (xInput != 0 || yInput != 0)
        {
            player.attackDir = new Vector2(xInput, yInput);
        }
        else
        {
            player.attackDir = player.faceDir;
        }

        player.SetZeroVelocity();



        //player.SetVelocity(player.attackMovement[comboCounter].x * player.attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        //lastTimeAttack = Time.time;
        //comboCounter++;
        player.canAttack = true;
    }

    public override void Update()
    {
        //if (stateTimer < 0)
        //{
        //    player.SetZeroVelocity();
        //}
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
