using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupCatDashState : EnemyState
{
    Enemy_CupCat enemy;
    Vector2 EtoPdir;
    public CupCatDashState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_CupCat enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        EtoPdir = (player.transform.position - enemy.transform.position).normalized;
        stateTimer = enemy.dashTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.rb.velocity = EtoPdir * enemy.dashSpeed;
        if (stateTimer < 0)
        {
            enemy.stateMachine.ChangeState(enemy.moveState);
        }
    }
}
