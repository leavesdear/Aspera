using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Vector3 offset;

    public float rotateTime = .2f;

    private Transform player;

    private bool isRotating = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector3(0, 0, 0);
        //offset = Camera.main.transform.position - player.transform.position;
    }

    void Update()
    {
        //Camera.main.transform.position = player.transform.position;//+ offset
        if (transform != null)
            transform.position = player.position + offset;
        Rotate();
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isRotating)
        {
            StartCoroutine(RotateAround(-45, rotateTime));
        }
        if (Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            StartCoroutine(RotateAround(45, rotateTime));
        }
    }

    IEnumerator RotateAround(float angle, float time)
    {
        float number = 60 * time;
        float nextAngle = angle / number;
        isRotating = true;

        for (int i = 0; i < number; i++)
        {
            transform.Rotate(new Vector3(0, 0, nextAngle));
            yield return new WaitForFixedUpdate();
        }

        isRotating = false;
    }
}
