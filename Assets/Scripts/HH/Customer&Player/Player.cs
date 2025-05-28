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
    [Header("플레이어 정보")]
    public int money;
    public TMP_Text moneyText;
    public int Upgrade_ID;
    
    [Header("명성")]
    public Fame foodFame;
    public Fame accesoryFame;
    public Fame furnFame;
    public Fame clothFame;
    public Fame pFoodFame;
    public List<Fame> priorityFame;
    [Header("명성 게이지")]
    public Slider foodFameBar;
    public Slider accesoryFameBar;
    public Slider furnFameBar;
    public Slider clothFameBar;
    public Slider pFoodFameBar;
    [Header("명성별 설명 텍스트")]
    
    
    [Header("명성 요구량")]
    public int[] tier = new int[4];
    [Header("레벨별 할인율")]
    public float[] saleTier = new float[4];
    private void Awake()
    {
       
    }
    public void RenewMoney()
    {
        moneyText.text = "" + money;
    }

    public void AnimationChange(bool isChange)
    {
        GetComponent<SpriteRenderer>().enabled = !isChange;
        for(int i = 0; i < 2; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(isChange);
        }
    }

    public void SkillUp(int sorts)
    {
        FameCheck(GetFameRef((ItemSorts)sorts));
        UpgradeFameCheck(GetFameRef((ItemSorts)sorts));
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

    private void UpgradeFameCheck(Fame target)
    {
        if(priorityFame.Count < 2 && target.tier >= 4 && !priorityFame.Contains(target))
        {
            priorityFame.Add(target);
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
   
}
