using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject buttonGetUp;
    public GameObject buttonMove;

    private Transform tree;

    void Start()
    {
        tree = GetComponent<Transform>();
        buttonGetUp.SetActive(true);
        buttonMove.SetActive(false);
    }

    void Update()
    {

    }
}
