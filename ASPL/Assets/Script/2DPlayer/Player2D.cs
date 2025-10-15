using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player2D : Entity
{
    public PlayerStateMachine2D stateMachine;

    public float moveSpeed;

    public IdleState2D idleState { get; private set; }
    public MoveState2D moveState { get; private set; }
    public JumpState2D jumpState { get; private set; }
    public ShadowState2D shadowState { get; private set; }
    public SleepState sleepState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = GetComponent<PlayerStateMachine2D>();

        idleState = new IdleState2D(stateMachine, this, "idle");
        moveState = new MoveState2D(stateMachine, this, "move");
        jumpState = new JumpState2D(stateMachine, this, "jump");
        shadowState = new ShadowState2D(stateMachine, this, "shadow");
        sleepState = new SleepState(stateMachine, this, "sleep");

        stateMachine.Initialize(sleepState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        GetPlayerInput();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void GetPlayerInput()
    {
        if (GameManager.instance.isInputLocked)
        { return; }
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

}
