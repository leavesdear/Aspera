using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : BossGroundedState
{
    public BossMoveState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMechine, _setBoolName, _enemy)
    {
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
        // enemy.SetVelocity(enemy.moveSpeed * enemy.faceDir, rb.velocity.y);

        //if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
        //{
        //    enemy.Flip();
        //    enemy.stateMechine.changeState(enemy.idleState);
        //}
    }
}
