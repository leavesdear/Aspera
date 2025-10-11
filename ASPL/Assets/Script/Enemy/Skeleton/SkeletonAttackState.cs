using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMechine, _setBoolName)
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
        enemy.laskAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
        if (triggerCalled)
        {
            stateMechine.ChangeState(enemy.battleState);
        }
    }
}
