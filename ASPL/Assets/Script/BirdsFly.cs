using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BirdsFly : MonoBehaviour
{
    private float Timer;
    public float moveSpeed;
    public float moveDuration;
    public Rigidbody2D rb { get; private set; }
    public GameObject player;
    public void Awake()
    {
        Timer = moveDuration;
        rb = GetComponent<Rigidbody2D>();
        //if (player.transform.position.x - transform.position.x < 0)
        //   Flip();
    }

    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer < 0)
        {
            Timer = moveDuration;
            Flip();
        }
        rb.velocity = new Vector3(moveSpeed, 0, 0);

    }

    private void Flip()
    {
        transform.Rotate(0, 0, 180);
        moveSpeed *= -1;
    }
}
