using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMechine, _setBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (stateTimer < 0)
        //{
        //    stateMechine.changeState(enemy.moveState);
        //}
    }
}
