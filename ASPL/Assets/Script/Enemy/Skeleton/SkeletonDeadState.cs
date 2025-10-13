using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    Enemy_Skeleton enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.cd.size = new Vector2(1, .2f);
        enemy.canBeHurt = false;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.canBeHurt = true;
    }


}
