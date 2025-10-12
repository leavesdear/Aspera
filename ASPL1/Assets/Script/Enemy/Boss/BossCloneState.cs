using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossCloneState : EnemyState
{
    private Enemy_Boss enemy;
    private GameObject trail;
    private int counter = 0;

    public BossCloneState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Awake()
    {
        base.Awake();
        enemy.anim.SetInteger("clone", 0);
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.hoverDuration;
        trail = enemy.bossTrail;
        trail.SetActive(true);

        enemy.transform.position = player.transform.position + new Vector3(0, enemy.appearHight + 15, -enemy.appearHight);
        enemy.SetZeroVelocity();

        AudioManager.instance.PlaySFX(11, true);
    }


    public override void Update()
    {
        base.Update();

        if (enemy.anim.GetInteger("clone") == 0)
        {
            enemy.transform.position = player.transform.position + new Vector3(0, enemy.appearHight + 15, -enemy.appearHight);
        }

        //持续伤害检测
        if (enemy.anim.GetInteger("clone") == 2)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
            foreach (var hit in collider)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    PlayerStats target = hit.GetComponent<PlayerStats>();
                    enemy.GetComponent<EnemyStats>().DoDamage(target);
                }
            }
        }

        if (stateTimer <= enemy.hoverDuration * 2 / 3 && stateTimer >= enemy.hoverDuration / 3 && counter == 0)
        {
            counter++;
            SkillManger.Instance.clone.UseSkill(false);
        }
        else if (stateTimer < enemy.hoverDuration / 3 && stateTimer > 0 && counter == 1)
        {
            counter++;
            SkillManger.Instance.clone.UseSkill(true);
        }
        else if (stateTimer <= 0 && enemy.anim.GetInteger("clone") != 1 && counter == 2)
        {
            counter = 0;
            enemy.anim.SetInteger("clone", 1);

            AudioManager.instance.StopSFX(3);

            SkillManger.Instance.cloneClash.UseSkill();
            stateTimer = Mathf.Infinity;
        }
    }

    public override void Exit()
    {
        base.Exit();
        trail.SetActive(false);
        enemy.anim.SetInteger("clone", 0);
        enemy.SetZeroVelocity();
    }

}
