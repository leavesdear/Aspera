using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmashState : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 hoverPosition;
    public BossSmashState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        hoverPosition = PlayerManger.instance.player.transform.position + new Vector3(0, enemy.appearHight, -enemy.appearHight);

        enemy.SetZeroVelocity();
        stateTimer = enemy.keepInAirTime;
        enemy.anim.SetInteger("smash", 1);
        AudioManager.instance.PlaySFX(11, true);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.anim.GetInteger("smash") == 1)
        {
            enemy.transform.position = hoverPosition;
        }
        if (enemy.anim.GetInteger("smash") == 2)
        {
            AudioManager.instance.StopSFX(11);

            enemy.transform.position -= new Vector3(0, enemy.fallSpeed, -enemy.fallSpeed) * Time.deltaTime;

            if (enemy.transform.position.z >= 0)
            {
                enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
                enemy.rb.velocity = new Vector3(0, 0, 0);
                stateTimer = enemy.smashTime;
                enemy.anim.SetInteger("smash", 3);
                SkillManger.Instance.smash.UseSkill();
            }
        }

        if (stateTimer <= 0)
        {
            switch (enemy.anim.GetInteger("smash"))
            {
                case 1:
                    stateTimer = Mathf.Infinity;
                    enemy.anim.SetInteger("smash", 2);
                    break;
                case 3:
                    if (triggerCalled)
                    {
                        //enemy.anim.SetInteger("smash", 1);
                        enemy.stateMachine.ChangeState(enemy.battleState);
                    }
                    break;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}
