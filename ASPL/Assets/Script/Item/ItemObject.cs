﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private ItemData itemData;
    //private void Start()
    //{
    //    sr = GetComponent<SpriteRenderer>();

    //    sr.sprite = itemData.icon;
    //}

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();

        if (itemData != null)
        {
            gameObject.name = "ItemObject-" + itemData.name;
            sr.sprite = itemData.icon;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
