using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : EnemyState
{
    private Enemy_Boss enemy;
    public BossAttackState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        int dir;
        if (Random.Range(1, 10) < 5)
        {
            if (enemy.faceRight)
            {
                enemy.Flip();
            }

            dir = 1;
        }
        else
        {
            dir = -1;
            if (!enemy.faceRight)
            {
                enemy.Flip();
            }
        }
        enemy.transform.position = PlayerManger.instance.player.transform.position + new Vector3(dir * enemy.appearToPlayerDiatance, 0, 0);
        AudioManager.instance.PlaySFX(2);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
        if (triggerCalled)
        {
            if (enemy.faceRight)
                enemy.Flip();
            stateMechine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.laskAttackTime = Time.time;
    }
}
