using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTRDieState : EnemyState
{
    Enemy_NTR enemy;
    public NTRDieState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_NTR enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.canBeHurt = false;
        enemy.healthBar.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.canBeHurt = true;
        enemy.healthBar.SetActive(true);
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
    }
}
