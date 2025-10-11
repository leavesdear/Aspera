using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTRMoveState : EnemyState
{
    Enemy_NTR enemy;
    private float distanceBetweenPlayerAndBoss;
    public NTRMoveState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_NTR enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
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

        distanceBetweenPlayerAndBoss = Vector2.Distance(enemy.transform.position, player.transform.position);

        Vector2 EtoPdir = (player.transform.position - enemy.transform.position).normalized;
        if (distanceBetweenPlayerAndBoss > enemyBase.attackDistance)
        {
            enemy.SetVelocity(enemy.moveSpeed * EtoPdir.x, enemy.moveSpeed * EtoPdir.y);
            if (EtoPdir.x < 0 && enemy.EntityFliped)
            {
                enemy.Flip();
            }
            else if (EtoPdir.x > 0 && !enemy.EntityFliped)
            {
                enemy.Flip();
            }
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }



    }
}
