using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashSwordController : MonoBehaviour
{
    [Header("旋转设置")]
    public float rotationSpeed = 180f; // 每秒自转度数

    [Header("正弦波设置")]
    public float amplitude = 1f;      // 波幅
    public float frequency = 1f;      // 频率（每秒周期数）
    public float moveSpeed = 1f;      // 水平移动速度


    [Header("Settings")]
    [SerializeField] private float attackCheckRadius = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private LayerMask playerLayer;

    private float lastDamageTime;


    public Vector3 _startPosition;
    private float _spawnTime;

    private void Awake()
    {
        _spawnTime = Time.time;
        _startPosition = EnemyManager.instance.boss.transform.position;
        transform.position = new Vector3(0, 0, 0);
    }

    void Start()
    {

    }

    void Update()
    {

        if (Time.time - lastDamageTime >= damageInterval)
        {
            CheckForPlayers();
            lastDamageTime = Time.time;
        }

        // 自转逻辑
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 正弦运动逻辑
        float elapsedTime = Time.time - _spawnTime;
        float horizontalOffset = moveSpeed * elapsedTime;
        float verticalOffset = Mathf.Sin(elapsedTime * Mathf.PI * 2 * frequency) * amplitude;

        // 更新位置
        transform.position = _startPosition + new Vector3(verticalOffset, -horizontalOffset, 0);
    }

    private void CheckForPlayers()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            attackCheckRadius,
            playerLayer
        );

        foreach (Collider2D hit in colliders)
        {
            if (hit.TryGetComponent(out PlayerStats playerStats))
            {
                playerStats.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, attackCheckRadius);
    }
}
