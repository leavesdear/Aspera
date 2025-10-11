using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using Pathfinding;

public class Enemy : Entity
{
    #region Move
    public float idleTime;
    public float moveSpeed;
    public float defaultMoveSpeed;
    #endregion
    #region Attack
    public float attackDistance;
    public float detectionRadius = 10f; // 扇形半径
    public float detectionAngle = 60f;  // 扇形角度（总角度）

    #endregion
    public EnemyStateMechine stateMachine;
    [Header("Stunned")]
    public bool canBeStunned;
    public GameObject counterImage;
    [SerializeField] protected LayerMask whatIsPlayer;
    public float laskAttackTime;
    public float attackCooldown;
    public float battleTime;
    public float battleDiatance;

    public bool beAttacked = false;

    public Player player;
    //private string lastAnimName;
    public Seeker seeker;
    protected override void Awake()
    {
        base.Awake();
        Player player = new Player();
        stateMachine = new EnemyStateMechine();
    }



    protected override void Start()
    {
        base.Start();
        defaultMoveSpeed = moveSpeed;
        player = PlayerManger.instance.player;
        seeker = GetComponent<Seeker>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public void FlipToPlayerWhileAttack()
    {
        if (this.faceRight && player.transform.position.x <= this.transform.position.x || !this.faceRight && player.transform.position.x >= this.transform.position.x)
        {
            this.Flip();
        }

    }


    public virtual void FreezeTimer(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTimer(false);
    }

    // public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(PlayerCheck.position, faceDir, 50, whatIsPlayer);


    public virtual Transform IsPlayerDetected()
    {
        // 1. 先做球形范围检测（性能更好）
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            whatIsPlayer
        );

        // 2. 遍历检测到的物体，筛选角度符合的
        foreach (Collider hit in hits)
        {
            Vector3 dirToPlayer = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle >= detectionAngle * 0.5f)
            { // 检测角度是双向的，所以取一半
                return hit.transform; // 玩家在扇形范围内
            }
        }
        return null;
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        // Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + faceDir * attackDistance, transform.position.y));

        //敌人发现玩家的扇形面
        //绘制扇形边缘
        Vector3 leftDir = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, detectionAngle * 0.5f, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, -leftDir * detectionRadius);
        Gizmos.DrawRay(transform.position, -rightDir * detectionRadius);

        // 绘制扇形弧线
        Handles.color = new Color(1, 1, 0, 0.1f);
        Handles.DrawSolidArc(
            transform.position,
            Vector3.up,
            -leftDir,
            detectionAngle,
            detectionRadius
        );
    }

    public virtual void AttackWindowOpen()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void AttackWindowClose()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool IsStunned()
    {
        if (canBeStunned)
        {
            AttackWindowClose();
            canBeStunned = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Die()
    {
        GetComponent<ItemBag>().instantiateItem(transform.position);
        base.Die();
        canBeHurt = false;
    }

    //public void AssignLastAnimName(string _lastAnimName)
    //{
    //    lastAnimName = _lastAnimName;
    //}
}
