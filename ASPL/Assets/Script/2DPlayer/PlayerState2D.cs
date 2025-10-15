using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState2D
{
    protected PlayerStateMachine2D stateMachine;
    protected Player2D player;

    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;

    public bool triggerCalled = false;
    private string animBoolName;
    protected float stateTimer;


    public PlayerState2D(PlayerStateMachine2D _stateMachine, Player2D _player, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }


    public virtual void Enter()
    {

        rb = player.rb;
        triggerCalled = false;
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        if (!GameManager.instance.isInputLocked)
        {
            yInput = Input.GetAxisRaw("Vertical");
            xInput = Input.GetAxisRaw("Horizontal");
        }

        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
