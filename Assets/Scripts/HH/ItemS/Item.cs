using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pItem //플레이어가 들고있는 아이템
{
    public int price;
    public string stuffName;
    public ItemSorts sort;
    public Sprite image;
    public int weight;
    public int counts;

    public pItem(ItemData itemData)
    {
        this.price = itemData.price;
        this.stuffName = itemData.stuffName;
        this.sort = itemData.sort;
        this.image = itemData.image;
        this.weight = itemData.weight;
    }
    public pItem(pItem other)
    {
        this.stuffName = other.stuffName;
        this.counts = other.counts;
        this.image = other.image;
        this.price = other.price;
        this.weight = other.weight;
        this.sort = other.sort;
    }
}



