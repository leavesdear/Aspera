using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    #region States
    public BossIdleState idleState { get; private set; }
    public BossMoveState moveState { get; private set; }
    public BossBattleState battleState { get; private set; }
    public BossAttackState attackState { get; private set; }
    public BossDeadState deadState { get; private set; }
    public BossSmashState smashState { get; private set; }
    public BossClashState clashState { get; private set; }
    public BossCloneState cloneState { get; private set; }

    #endregion

    #region Smash
    public float keepInAirTime;
    public float smashTime;
    public float appearHight;
    public float fallSpeed;
    #endregion

    #region Attack
    public float moveToPlayerDiatance;
    public float appearToPlayerDiatance;
    #endregion

    #region Clone
    public float hoverDuration = 1f;
    public GameObject bossTrail;
    #endregion


    protected override void Awake()
    {
        base.Awake();
        player = PlayerManger.instance.player;
        idleState = new BossIdleState(this, stateMachine, "idle", this);
        //moveState = new BossMoveState(this, stateMechine, "Move", this);
        battleState = new BossBattleState(this, stateMachine, "move", this);
        attackState = new BossAttackState(this, stateMachine, "attack", this);
        deadState = new BossDeadState(this, stateMachine, "die", this);
        smashState = new BossSmashState(this, stateMachine, "Smash", this);
        clashState = new BossClashState(this, stateMachine, "clash", this);
        cloneState = new BossCloneState(this, stateMachine, "Clone", this);

    }

    protected override void Start()
    {
        base.Start();
        //stateMechine.Initialize(battleState);
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    stateMechine.changeState(smashState);
        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    stateMechine.changeState(attackState);
        //}
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    stateMechine.changeState(clashState);
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    stateMechine.changeState(cloneState);
        //}
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override bool IsStunned()
    {
        if (base.IsStunned())
        {
            //stateMechine.changeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

}
