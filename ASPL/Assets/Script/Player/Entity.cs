using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats cs { get; private set; }

    #endregion


    [Header("KnockBack info")]
    public bool isKnocked;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackDirection;


    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    //[SerializeField] protected Transform wallCheck;
    //[SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public Vector2 faceDir;
    public Vector2 attackDir;
    public bool faceRight = true;

    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    public float attackCheckRadius;
    public Transform attackCheck;

    public bool canBeHurt = true;

    public bool EntityFliped = false;

    private Vector3 lastPosition;

    public System.Action onFlipped;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponent<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cs = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {

    }



    protected virtual void Update()
    {
        //AttackCheckPositionOffset();

        if (IsGroundDetected())
        {
            lastPosition = transform.position;
        }
        else
        {
            transform.position = new Vector3(lastPosition.x, lastPosition.y, 0);
        }
    }




    //private void AttackCheckPositionOffset()
    //{
    //    attackCheck.position = transform.position + new Vector3(0, 0, -attackCheckRadius);
    //}
    public virtual void damageEffect()
    {
        fx.StartCoroutine("FlashFX");
        //StartCoroutine("HitKnockBack");
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    // public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, wallCheckDistance, whatIsGround);

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        //Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * faceDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }


    private void SetFaceRightValue()
    {
        if (flipLeft == -1)
            faceRight = false;
        if (flipLeft == 1)
            faceRight = true;
    }

    private int flipLeft = -1;
    public void Flip()
    {
        //Vector3 newScale = transform.localScale;
        //newScale.x = Mathf.Abs(newScale.x) * flipLeft;
        //transform.localScale = newScale;
        //原翻转逻辑

        transform.Rotate(0, 180, 0);

        flipLeft *= -1;

        SetFaceRightValue();

        EntityFliped = !EntityFliped;
        if (onFlipped != null)
            onFlipped();
    }

    public void FlipController(float _x)
    {
        if (!faceRight && _x > 0)
        {
            Flip();
        }
        else if (faceRight && _x < 0)
        {
            Flip();
        }
    }

    //private IEnumerator HitKnockBack()
    //{
    //    isKnocked = true;
    //    rb.velocity = new Vector2(knockBackDirection.x * -faceDir, knockBackDirection.y);
    //    yield return new WaitForSeconds(knockBackDuration);
    //    isKnocked = false;
    //}

    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }
        SetVelocity(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        //FlipController(_xVelocity);
    }


    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public virtual void Die()
    {

    }


}
