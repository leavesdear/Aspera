using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransTrigger : MonoBehaviour
{
    [Header("触发设置")]
    public string targetTag = "Player";
    public TeleportPoint targetTeleportPoint;

    private void Start()
    {
        // 确保碰撞箱是触发器
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        //if (other.CompareTag(targetTag))
        //{
        //Debug.Log("1");
        if (targetTeleportPoint != null)
        {
            targetTeleportPoint.TriggerAction();
        }
        else
        {
            Debug.LogWarning("TeleportTrigger: 没有设置目标TeleportPoint！");
        }
        //}
    }
}

