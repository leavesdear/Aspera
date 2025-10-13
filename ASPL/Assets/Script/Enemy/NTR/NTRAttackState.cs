using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTRAttackState : EnemyState
{
    Enemy_NTR enemy;
    public NTRAttackState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_NTR enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            enemy.stateMachine.ChangeState(enemy.moveState);
        }
    }
}
