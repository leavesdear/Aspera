using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{

    private Vector2 hitDir;
    public PlayerHitState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.canBeHurt = false;
        stateTimer = player.invincibilityTime;


        Collider2D[] collider = Physics2D.OverlapCircleAll(player.transform.position, 20, player.enemyLayer);
        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                enemy = hit.GetComponent<Transform>();
                hitDir = (player.transform.position - enemy.position).normalized;
                if (hitDir.x < 0)
                {
                    player.Flip();
                }
            }
        }

        //AudioManager.instance.PlaySFX(0);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(hitDir.x * player.hitForce, hitDir.y * player.hitForce);
        if (stateTimer < 0)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetZeroVelocity();
        player.canBeHurt = true;
        if (player.EntityFliped)
        {
            player.Flip();
        }
    }


}
