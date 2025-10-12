using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    #endregion



    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "idle", this);
        //moveState = new SkeletonMoveState(this, stateMechine, "Move", this);
        //battleState = new SkeletonBattleState(this, stateMechine, "Move", this);
        //attackState = new SkeletonAttackState(this, stateMechine, "Attack", this);
        deadState = new SkeletonDeadState(this, stateMachine, "dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
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

}
