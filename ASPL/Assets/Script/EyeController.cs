using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    public Transform player; // 玩家Transform
    public float maxOffset = 4f; // 眼球最大偏移量
    public float delayTime = 0.3f; // 响应延迟时间
    public float followSpeed = 15f; // 跟随速度（值越大越快）

    private Vector3 initialLocalPos; // 初始本地位置
    private Camera mainCamera;
    private Vector2 currentOffset;
    private Coroutine moveCoroutine;
    private Vector3 lastPlayerPos;

    void Start()
    {
        mainCamera = Camera.main;
        initialLocalPos = transform.localPosition;
        currentOffset = Vector2.zero;
        lastPlayerPos = player.position;
    }

    void Update()
    {
        Vector3 currentPlayerPos = player.position;

        // 当检测到玩家位置变化时
        if (currentPlayerPos != lastPlayerPos)
        {
            // 计算目标偏移量
            Vector2 targetOffset = CalculateTargetOffset(currentPlayerPos);

            // 取消正在进行的移动并开始新的延迟跟随
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(DelayedFollow(targetOffset));

            lastPlayerPos = currentPlayerPos;
        }

        // 应用当前偏移量
        transform.localPosition = initialLocalPos + (Vector3)currentOffset;
    }

    Vector2 CalculateTargetOffset(Vector3 targetPosition)
    {
        // 计算眼球基准世界位置
        Vector3 eyeWorldPos = transform.parent.TransformPoint(initialLocalPos);

        // 计算指向玩家的方向向量
        Vector3 worldDir = targetPosition - eyeWorldPos;

        // 获取相机方向基准
        Vector3 camRight = mainCamera.transform.right;
        Vector3 camUp = mainCamera.transform.up;

        // 投影到相机平面
        float horizontal = Vector3.Dot(worldDir, camRight);
        float vertical = Vector3.Dot(worldDir, camUp);
        Vector2 offset = new Vector2(horizontal, vertical);

        // 限制最大偏移范围
        return offset.magnitude > maxOffset ?
            offset.normalized * maxOffset : offset;
    }

    IEnumerator DelayedFollow(Vector2 targetOffset)
    {
        // 等待延迟时间
        yield return new WaitForSeconds(delayTime);

        // 记录起始偏移量
        Vector2 startOffset = currentOffset;
        float progress = 0f;

        // 平滑过渡到目标偏移
        while (progress < 1f)
        {
            progress += followSpeed * Time.deltaTime;
            currentOffset = Vector2.Lerp(startOffset, targetOffset, progress);
            yield return null;
        }

        // 确保最终位置准确
        currentOffset = targetOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, maxOffset);
    }
}
