using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private int moveDir;
    private Transform player;
    private Enemy_Skeleton enemy;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
        stateTimer = enemy.battleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;//获取坐标比较而非向量

        if (enemy.IsPlayerDetected())
        {
            if (Vector2.Distance(enemy.transform.position, enemy.IsPlayerDetected().position) < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMechine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 10)
            {
                stateMechine.ChangeState(enemy.idleState);
            }
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time > enemy.laskAttackTime + enemy.attackCooldown)
        {
            enemy.laskAttackTime = Time.time;
            return true;
        }
        return false;
    }
}
