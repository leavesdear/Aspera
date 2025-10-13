using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<ItemData> itemList = new List<ItemData>();

    ItemData GetDorppedItem()
    {
        int randomNumber = Random.Range(1, 101);//1-100
        List<ItemData> possibleItems = new List<ItemData>();
        foreach (ItemData item in itemList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            ItemData droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        return null;
    }

    public void instantiateItem(Vector3 spawnPostion)
    {

        ItemData droppedItem = GetDorppedItem();
        if (droppedItem != null)
        {
            GameObject itemGameObject = Instantiate(droppedItemPrefab, spawnPostion, Quaternion.identity);
            itemGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.icon;

            float dropForce = 300f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            itemGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }

}
