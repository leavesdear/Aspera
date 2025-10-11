using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlosHitState : EnemyState
{
    private Enemy_Flos enemy;
    private Vector2 hitDir;
    public FlosHitState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Flos _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.canBeHurt = false;
        stateTimer = enemy.hitTime;


        //Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.transform.position, 20);
        //foreach (var hit in collider)
        //{
        //    if (hit.GetComponent<Player>() != null)
        //    {
        //player = hit.GetComponent<Transform>();

        hitDir = (player.transform.position - enemy.transform.position).normalized;
        if (hitDir.x < 0)
        {
            enemy.Flip();
        }

        //    }
        //}

        //AudioManager.instance.PlaySFX(0);
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(hitDir.x * enemy.hitForce, hitDir.y * enemy.hitForce);
        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetZeroVelocity();
        enemy.canBeHurt = true;
        if (enemy.EntityFliped)
        {
            enemy.Flip();
        }
    }
}
