using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleState : EnemyState
{
    private int moveDir;
    private Enemy_Boss enemy;
    private float distanceBetweenPlayerAndBoss;

    public BossBattleState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Awake()
    {
        base.Awake();

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = pathGenerateInterval;
        AudioManager.instance.PlaySFX(3, true);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(3);
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        distanceBetweenPlayerAndBoss = Vector2.Distance(enemy.transform.position, player.transform.position);

        Vector2 EtoPdir = (player.transform.position - enemy.transform.position).normalized;
        if (distanceBetweenPlayerAndBoss > enemyBase.attackDistance)
        {
            enemy.SetVelocity(enemy.moveSpeed * EtoPdir.x, enemy.moveSpeed * EtoPdir.y);
            enemy.anim.SetFloat("x", EtoPdir.x);
            enemy.anim.SetFloat("y", EtoPdir.y);
        }



        //if (Vector2.Distance(enemy.transform.position, player.transform.position) > enemyBase.attackDistance)
        //    AutoPath();

        //// 攻击检测
        //if (Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.attackDistance && CanAttack())
        //{
        //    stateMechine.changeState(enemy.attackState);
        //}
        //else
        //{
        //    Vector2 direction = (pathPointList[currentIndex] - enemy.transform.position).normalized;
        //    //if (stateTimer < 0)
        //    //{ 
        //    //    stateTimer = pathGenerateInterval;

        //    //}
        //    enemy.SetVelocity(enemy.moveSpeed * direction.x, enemy.moveSpeed * direction.y);
        //    enemy.anim.SetFloat("x", direction.x);
        //    enemy.anim.SetFloat("y", direction.y);
        //}

        //原BOSS追逐逻辑


        //BOSS决策


        if (distanceBetweenPlayerAndBoss < enemy.attackDistance)//&& CanAttack()
        {
            stateMechine.ChangeState(enemy.attackState);
        }
        else if (Random.Range(0, 100) < 30 && SkillManger.Instance.smash.CanUseSkill())
        {
            stateMechine.ChangeState(enemy.smashState);
        }
        else if (Random.Range(0, 100) < 80 && distanceBetweenPlayerAndBoss > enemy.attackDistance * 3 && SkillManger.Instance.clash.CanUseSkill())
        {
            stateMechine.ChangeState(enemy.clashState);
        }
        else if (Random.Range(0, 100) < 20 && enemyHealthPercent <= 0.5f && SkillManger.Instance.clone.CanUseSkill())
        {
            stateMechine.ChangeState(enemy.cloneState);
        }
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
