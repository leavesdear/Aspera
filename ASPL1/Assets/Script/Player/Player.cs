using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public TeleportPoint teleportPoint;


    public PlayerStateMachine stateMachine;

    public float moveSpeed;
    public float dashDuration;
    public float dashSpeed;
    public float invincibilityTime;
    public float hitForce;

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayPrimaryAttack attackState { get; private set; }
    public PlayerHitState hitState { get; private set; }
    public PlayerBoomState boomState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    public bool isBusy;

    public bool canAttack = true;

    public LayerMask enemyLayer;

    public float deadBuffingTime;
    public TeleportPoint teleport;

    [Header("Boom State Settings")]
    public Transform firePoint;
    public GameObject boomCrosshairPrefab;
    public LineRenderer trajectoryRendererPrefab;
    public GameObject boomProjectilePrefab;
    public GameObject explosionPrefab;
    public float crosshairSpeed = 5f;


    public PlayerStats playerStats;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        playerStats = GetComponent<PlayerStats>();

        idleState = new PlayerIdleState(stateMachine, this, "idle");
        moveState = new PlayerMoveState(stateMachine, this, "move");
        attackState = new PlayPrimaryAttack(stateMachine, this, "attack");
        dashState = new PlayerDashState(stateMachine, this, "dash");
        hitState = new PlayerHitState(stateMachine, this, "hit");
        boomState = new PlayerBoomState(stateMachine, this, "boom");
        deadState = new PlayerDeadState(stateMachine, this, "die");
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        GetPlayerInput();

    }

    private void GetPlayerInput()
    {
        if (GameManager.instance.isInputLocked)
        { return; }

        if (playerStats.CanUseEnergy() && Input.GetMouseButtonDown(1) && canAttack)
        {
            playerStats.UseEnergy();
            stateMachine.ChangeState(boomState);
        }
        else
        {
            if (Input.GetMouseButton(0) && canAttack)
            {
                stateMachine.ChangeState(attackState);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(dashState);
            }
        }
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
