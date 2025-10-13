using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;



public class PlayerBoomState : PlayerState
{
    // 配置参数
    private float projectileSpeed = 20f;
    private float maxTrajectoryHeight = 30f;
    private int trajectoryResolution = 30;
    private float explosionRadius = 24f;
    private int explosionDamage = 50;
    private float konckBackForce = 12f;

    // 引用组件
    private Transform firePoint;
    private GameObject crosshair;
    private LineRenderer trajectoryRenderer;
    private Camera mainCamera;
    private Transform parent;

    // 运行时数据
    private Vector3 targetPoint;
    private bool isAiming;
    private bool isFiring;
    private GameObject currentProjectile; // 跟踪当前炮弹
    private bool isActive; // 状态激活标志

    public PlayerBoomState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
        : base(_stateMachine, _player, _animBoolName)
    {
        parent = player.gameObject.transform.parent;
    }

    public override void Enter()
    {
        base.Enter();
        isActive = true;
        player.SetZeroVelocity();

        // 清理可能残留的对象
        if (crosshair != null) Object.Destroy(crosshair);
        if (trajectoryRenderer != null) Object.Destroy(trajectoryRenderer?.gameObject);

        // 初始化新对象
        firePoint = player.firePoint;
        mainCamera = Camera.main;

        crosshair = Object.Instantiate(player.boomCrosshairPrefab);
        crosshair.SetActive(false);

        trajectoryRenderer = Object.Instantiate(player.trajectoryRendererPrefab);
        trajectoryRenderer.positionCount = trajectoryResolution;
        trajectoryRenderer.enabled = false;

        StartAiming();
    }

    public override void Exit()
    {
        base.Exit();
        isActive = false;

        // 清理发射中的炮弹
        if (isFiring && currentProjectile != null)
        {
            Object.Destroy(currentProjectile);
            CreateExplosion(targetPoint);
        }

        // 清理UI元素
        if (crosshair != null)
        {
            Object.Destroy(crosshair);
            crosshair = null;
        }
        if (trajectoryRenderer != null)
        {
            Object.Destroy(trajectoryRenderer.gameObject);
            trajectoryRenderer = null;
        }

        player.anim.SetBool("fire", false);
    }

    public override void Update()
    {
        base.Update();
        if (!isActive || isFiring) return;

        HandleAimingInput();
        UpdateAimingPosition();
        UpdateTrajectory();

        if (Input.GetMouseButtonUp(1) && !GameManager.instance.isInputLocked)
        {
            AudioManager.instance.PlaySFX(9);
            FireProjectile();
        }

        if (GameManager.instance.isInputLocked)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    private void StartAiming()
    {
        AudioManager.instance.PlaySFX(27);

        isAiming = true;
        targetPoint = firePoint.position;
        crosshair.SetActive(true);
        trajectoryRenderer.enabled = true;
    }

    private void HandleAimingInput()
    {
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0
        );

        Vector3 moveDirection = input.normalized;
        targetPoint += moveDirection * player.crosshairSpeed * Time.deltaTime;
        targetPoint.z = 0; // 保持Z轴为0
    }

    private void UpdateAimingPosition()
    {
        if (crosshair != null)
            crosshair.transform.position = targetPoint;
    }

    private void UpdateTrajectory()
    {
        if (trajectoryRenderer == null) return;

        Vector3 startPos = firePoint.position;
        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = (float)i / (trajectoryResolution - 1);
            trajectoryRenderer.SetPosition(i, CalculateTrajectoryPoint(startPos, targetPoint, t));
        }
    }

    private Vector3 CalculateTrajectoryPoint(Vector3 start, Vector3 end, float t)
    {
        Vector3 direction = end - start;
        float horizontalDistance = direction.magnitude;
        float verticalSpeed = Mathf.Sqrt(2 * Physics.gravity.magnitude * maxTrajectoryHeight);
        float time = (2 * verticalSpeed) / Physics.gravity.magnitude;

        Vector3 point = start + direction.normalized * (horizontalDistance * t);
        point.y += verticalSpeed * t * time - 0.5f * Physics.gravity.magnitude * Mathf.Pow(t * time, 2);
        return point;
    }

    private async void FireProjectile()
    {
        isFiring = true;
        player.anim.SetBool("fire", true);
        currentProjectile = Object.Instantiate(
            player.boomProjectilePrefab,
            firePoint.position,
            Quaternion.identity,
            parent
        );
        try
        {
            float progress = 0f;
            Vector3 startPos = firePoint.position;
            while (progress < 1f && currentProjectile != null && isActive)
            {
                progress += Time.deltaTime * projectileSpeed;
                currentProjectile.transform.position = CalculateTrajectoryPoint(startPos, targetPoint, progress);
                await System.Threading.Tasks.Task.Yield();
            }
        }
        finally
        {
            if (currentProjectile != null && isActive)
            {
                CreateExplosion(targetPoint);
                Object.Destroy(currentProjectile);
            }
            isFiring = false;

            if (isActive)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
    private void CreateExplosion(Vector3 position)
    {
        GameObject explosion = Object.Instantiate(
            player.explosionPrefab,
            position,
            Quaternion.identity
        );
        explosion.transform.localScale = Vector3.one * 10;//爆炸效果太小所以放大

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, explosionRadius, player.enemyLayer);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<EnemyStats>(out var damageable))
            {
                damageable.TakeDamage(explosionDamage);
            }
        }

        AudioManager.instance.PlaySFX(29);

        //后坐力
        Vector3 KnockBackDir = (player.transform.position - position).normalized;
        player.transform.position += KnockBackDir * konckBackForce;


        Object.Destroy(explosion, 2f);
    }
}