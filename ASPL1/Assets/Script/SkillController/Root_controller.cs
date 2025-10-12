using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root_controller : FaceCameraController
{
    [Header("Settings")]
    [SerializeField] private float attackCheckRadius = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private LayerMask playerLayer;

    private float lastDamageTime;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= damageInterval)
        {
            CheckForPlayers();
            lastDamageTime = Time.time;
        }
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
