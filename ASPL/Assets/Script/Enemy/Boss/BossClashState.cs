using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClashState : EnemyState
{
    private Enemy_Boss enemy;
    public BossClashState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.transform.position = new Vector3(0, 24, 0);

        SkillManger.Instance.clash.UseSkill();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        if (triggerCalled)
        {
            enemy.stateMachine.ChangeState(enemy.battleState);
        }
    }
}
