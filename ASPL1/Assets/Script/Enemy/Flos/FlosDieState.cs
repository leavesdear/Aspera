using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlosDieState : EnemyState
{
    private Enemy_Flos enemy;
    public FlosDieState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Flos _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
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
