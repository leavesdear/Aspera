using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CupCat : Enemy
{
    public CupCatMoveState moveState { get; private set; }
    public CupCatDieState dieState { get; private set; }
    public CupCatDashState dashState { get; private set; }

    public GameObject healthBar;
    public float dashCoolDown;
    public float dashSpeed;
    public float dashTime;


    protected override void Awake()
    {
        base.Awake();
        dashState = new CupCatDashState(this, stateMachine, "dash", this);
        moveState = new CupCatMoveState(this, stateMachine, "move", this);
        dieState = new CupCatDieState(this, stateMachine, "die", this);
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
