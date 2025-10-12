using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class Enemy_NTR : Enemy
{
    public NTRMoveState moveState { get; private set; }
    public NTRAttackState attackState { get; private set; }
    public NTRDieState dieState { get; private set; }

    public GameObject healthBar;

    protected override void Awake()
    {
        base.Awake();
        moveState = new NTRMoveState(this, stateMachine, "move", this);
        attackState = new NTRAttackState(this, stateMachine, "attack", this);
        dieState = new NTRDieState(this, stateMachine, "die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(dieState);
    }
}
