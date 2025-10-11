using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : EnemyState
{
    Enemy_Boss enemy;

    public BossDeadState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //enemy.cd.size = new Vector2(1, .2f);
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
