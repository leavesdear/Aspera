using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundedState : EnemyState
{
    protected Enemy_Boss enemy;
    public BossGroundedState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
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
