using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlosHideState : EnemyState
{
    private Enemy_Flos enemy;
    //private float warningTimer;

    public FlosHideState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Flos _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.SpikeReleseDuration;
        //warningTimer = enemy.warningDuration;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.beFound)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        RootAttack();
    }

    private void RootAttack()
    {
        if (stateTimer < 0)
        {
            if (Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.attackCheckRadius)
            {
                enemy.StartWarning();
            }
            stateTimer = enemy.SpikeReleseDuration;
        }


        //if (enemy.isWarning)
        //{
        //    warningTimer -= Time.deltaTime;
        //}
        //if (enemy.isWarning && warningTimer <= 0)
        //{
        //    enemy.ReleseRoot();
        //    warningTimer = enemy.warningDuration;
        //}
        //if (stateTimer < 0)
        //{
        //    //Debug.Log(player.transform.position);
        //    //Debug.Log(enemy.transform.position);
        //    //Debug.Log(enemy.attackCheckRadius);

        //    if (Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.attackCheckRadius)
        //    {
        //        enemy.startWarning();
        //    }
        //    stateTimer = enemy.SpikeReleseDuration;
        //}
    }

    public override void Exit()
    {
        base.Exit();
    }


}
