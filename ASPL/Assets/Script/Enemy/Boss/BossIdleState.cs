using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossGroundedState
{
    public BossIdleState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMechine, _setBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
        enemy.transform.position = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
    }
}
