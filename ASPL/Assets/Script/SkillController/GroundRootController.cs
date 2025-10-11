using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundRootController : MonoBehaviour
{
    [Header("减速参数")]
    [SerializeField] private float slowdownFactor = 0.5f;
    [SerializeField] private bool resetOnExit = true;

    private float PlayerCurrentSpeed;
    private float originalSpeed;


    Transform[] childs;

    void Start()
    {
        childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i);
        }
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].rotation = Camera.main.transform.rotation;
        }
    }

    // 关键修改 1：使用 2D 碰撞检测方法
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCurrentSpeed = other.GetComponent<Player>().moveSpeed;
            // 关键修改 2：保存原始速度
            originalSpeed = PlayerCurrentSpeed;
            ApplySlowdown(slowdownFactor, other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 优化：保持恒定减速，避免指数级衰减
            PlayerCurrentSpeed = originalSpeed * slowdownFactor;
        }
    }

    // 关键修改 3：使用 2D 碰撞退出方法
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && resetOnExit)
        {
            other.GetComponent<Player>().moveSpeed = originalSpeed;
        }
    }

    private void ApplySlowdown(float factor, Collider2D player)
    {
        player.GetComponent<Player>().moveSpeed = originalSpeed * factor;
    }

}
