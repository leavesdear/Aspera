using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepDistanceWithPlayer : MonoBehaviour
{
    public Player player;
    public Vector3 distance;

    void Awake()
    {
    }

    private void Start()
    {
        player = PlayerManger.instance.player;
    }

    void Update()
    {
        transform.position = player.transform.position + distance;
    }
}
