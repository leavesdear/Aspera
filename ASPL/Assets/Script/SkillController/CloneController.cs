using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloneController : FaceCameraController
{
    public float arcHeight = 3f;
    public float arcDuration = 1.5f;
    public float horizontalSpeed = 5f;
    public float xOffset = 50f;

    public Transform attackCheck;
    public float attackCheckRadius;
    public GameObject trail;

    private bool cloneFaceLeft;

    private float horizontalDuration;

    private Vector3 targetPosition;

    SpriteRenderer sr;
    [SerializeField] Material flashMaterial;
    private Animator anim;

    public void SetBoolCloneFaceLeftValue(bool _cloneFaceLeft)
    {
        cloneFaceLeft = _cloneFaceLeft;
    }


    private void Awake()
    {
        trail.SetActive(true);
        sr = GetComponent<SpriteRenderer>();
        sr.material = flashMaterial;
        anim = GetComponent<Animator>();

        arcHeight = SkillManger.Instance.cloneClash.arcHeight;
        arcDuration = SkillManger.Instance.cloneClash.arcDuration;
        horizontalSpeed = SkillManger.Instance.cloneClash.horizontalSpeed;
        xOffset = SkillManger.Instance.cloneClash.xOffset;

        horizontalDuration = xOffset / horizontalSpeed * 2;

    }

    protected override void Start()
    {
        base.Start();
        if (cloneFaceLeft)
        {
            targetPosition = PlayerManger.instance.player.transform.position + new Vector3(xOffset, 0, 0);
            Flip();
        }
        else
        {
            targetPosition = PlayerManger.instance.player.transform.position + new Vector3(-xOffset, 0, 0);
        }
        StartCoroutine("AttackRoutine");
    }

    private void Update()
    {
        //持续伤害检测
        if (anim.GetInteger("clone") == 2)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
            foreach (var hit in collider)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    PlayerStats target = hit.GetComponent<PlayerStats>();
                    GetComponent<EnemyStats>().DoDamage(target);
                }
            }
        }
    }

    IEnumerator AttackRoutine()
    {

        anim.SetInteger("clone", 1);

        // 阶段一：弧形移动
        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < arcDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / arcDuration;

            // 抛物线运动公式
            float yOffset = Mathf.Sin(progress * Mathf.PI) * arcHeight;
            transform.position = Vector3.Lerp(startPos, targetPosition, progress)
                                + new Vector3(0, yOffset, 0);

            yield return null;
        }

        anim.SetInteger("clone", 2);
        // 阶段二：水平滑行
        Vector3 moveDirection = Vector3.right; // 向右移动
        if (cloneFaceLeft)
        {
            moveDirection = Vector3.left;
        }

        elapsedTime = 0;
        while (elapsedTime < horizontalDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.Translate(moveDirection * horizontalSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(this.transform.parent.gameObject);
    }


    private int flipLeft = -1;
    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * flipLeft;
        transform.localScale = newScale;
        flipLeft *= -1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
