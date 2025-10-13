using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlosIdleState : EnemyState
{
    private Enemy_Flos enemy;
    private GameObject groundRoot;
    public FlosIdleState(Enemy _enemyBase, EnemyStateMechine _stateMechine, string _setBoolName, Enemy_Flos _enemy) : base(_enemyBase, _stateMechine, _setBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.SpikeReleseDuration / 2;
        // groundRoot = Instantiate(enemy.groundRootPrefab, enemy.transform.position, Quaternion.identity, enemy.parent);
        enemy.healthBar.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        Destroy(groundRoot);
    }

    public override void Update()
    {
        base.Update();

        RootAttack();
    }

    private void RootAttack()
    {
        if (stateTimer < 0)
        {
            stateTimer = enemy.SpikeReleseDuration / 2;
            if (Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.attackCheckRadius)
            {
                enemy.StartWarning();
            }

        }
    }
}
