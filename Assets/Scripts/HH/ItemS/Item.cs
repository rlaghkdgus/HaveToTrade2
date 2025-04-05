using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pItem //�÷��̾ ����ִ� ������
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
}



