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
    [Header("�� ������")]
    public Slider foodFameBar;
    public Slider accesoryFameBar;
    public Slider furnFameBar;
    public Slider clothFameBar;
    public Slider pFoodFameBar;
    [Header("���� ���� �ؽ�Ʈ")]
    public TMP_Text FameInfoText;
    public GameObject FameInfoButton;
    private int InfoPreIndex;
    [Header("�� �䱸��")]
    public int[] tier = new int[4];
    [Header("������ ������")]
    public float[] saleTier = new float[4];
    private void Awake()
    {
       
    }
    public void RenewMoney()
    {
        moneyText.text = "" + money;
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
    public void FameBarCheck()
    {
        if (accesoryFameBar != null)
            accesoryFameBar.value = accesoryFame.fame / 15f;
        if (foodFameBar != null)
            foodFameBar.value = foodFame.fame / 15f;
        if (pFoodFameBar != null)
            pFoodFameBar.value = pFoodFame.fame / 15f;
        if (furnFameBar != null)
            furnFameBar.value = furnFame.fame / 15f;
        if (clothFameBar != null)
            clothFameBar.value = clothFame.fame / 15f;
    }
    public void FameInfo(int buttonIndex)
    {
        switch(buttonIndex)
        {
            case 0:
                FameInfoText.text = "������ " + tier[0] + "% ����";
                break;
            case 1:
                FameInfoText.text = "������ " + tier[1] + " % ����";
                break;
            case 2:
                FameInfoText.text = "������ " + tier[2] + "% ����";
                break;
            case 3:
                FameInfoText.text = "������ " + tier[3] + "% ����";
                break;
        }
        
        if(FameInfoButton.activeSelf == false)
        {
            FameInfoButton.SetActive(true);
        }
        else
        {
            if (InfoPreIndex != buttonIndex)
            {
                InfoPreIndex = buttonIndex;
                return;
            }
            else
                InfoPreIndex = buttonIndex;
            FameInfoButton.SetActive(false);
        }

    }
}
