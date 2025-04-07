using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class Fame
{
    public int fame;
    public float sale;
    public int tier;
}

public class Player : Singleton<Player>
{
    [Header("�÷��̾� ����")]
    public int money;
    public TMP_Text moneyText;

    [Header("��")]
    public Fame foodFame;
    public Fame accesoryFame;
    public Fame furnFame;
    public Fame clothFame;
    public Fame pFoodFame;

    [Header("�� �䱸��")]
    public int[] tier = new int[4];
    [Header("������ ������")]
    public float[] saleTier = new float[4];
    private void Awake()
    {
       
    }
    public void RenewMoney()
    {
        moneyText.text = money.ToString();
    }

    public void SkillUp(int sorts)
    {
        FameCheck(GetFameRef((ItemSorts)sorts));
    }
    private Fame GetFameRef(ItemSorts sorts)
    {
        if (sorts == ItemSorts.food) return foodFame;
        if (sorts == ItemSorts.accesory) return accesoryFame;
        if (sorts == ItemSorts.pFood) return pFoodFame;
        if (sorts == ItemSorts.clothes) return clothFame;
        return furnFame;
    }

    private void FameCheck(Fame target)
    {
        for (int i = 0; i < tier.Length; i++)
        {
            if (target.tier == i && target.fame >= tier[i])
            {
                target.tier++;
                target.sale = saleTier[i];
                break;
            }
        }
    }
}
