using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject buttonGetUp;
    public GameObject buttonMove;


    private Transform tree;
    public bool canMove;
    private Animator animator;

    [SerializeField] private float yOffset = 5f;    // 总移动距离

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isRising = false;
    private bool isMoving = false;


    [SerializeField] private float moveSpeed = 2f;  // 移动速度（单位/秒）

    void Start()
    {
        animator = GetComponent<Animator>();
        tree = GetComponent<Transform>();
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * yOffset;

        if (buttonGetUp != null)
            buttonGetUp.SetActive(true);
        if (buttonMove != null)
            buttonMove.SetActive(false);
    }

    void Update()
    {
        animator.SetBool("move", isMoving);
        if (isMoving)
        {
            AudioManager.instance.PlaySFX(23);
        }

        if (isRising)
        {
            AudioManager.instance.PlaySFX(1);
            // 计算每帧移动的距离
            float step = moveSpeed * Time.deltaTime;

            // 使用 MoveTowards 实现线性移动
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // 检查是否到达目标位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isRising = false;
                canMove = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.W) && !canMove)
        {
            StartMoving();
        }
        if (canMove)
        {
            if (buttonGetUp != null)
                buttonGetUp.SetActive(false);
            if (buttonMove != null)
                buttonMove.SetActive(true);

            float horizontalInput = 0f;

            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput = -1f; // 向左移动
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalInput = 1f; // 向右移动
                isMoving = true;
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                isMoving = false;
            }
            // 应用移动
            if (horizontalInput != 0f)
            {
                Vector3 movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);
                transform.Translate(movement);
            }
        }

    }

    // 开始移动的方法（可以被其他脚本调用）
    public void StartMoving()
    {
        isRising = true;
    }

}
