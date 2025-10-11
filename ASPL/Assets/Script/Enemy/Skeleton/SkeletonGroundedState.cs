using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    protected Transform player;
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //player = PlayerManger.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        //{
        //    stateMechine.changeState(enemy.battleState);
        //}
    }
}
