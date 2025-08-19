using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class SaleInfo
{
    public float maxSale = 100f;
    public float minSale = 60f;
    public ItemSorts sort;
}
public class ClimateEvent : MonoBehaviour
{
    public List<SaleInfo> sortSales;
    public Data<ClimateState> c_State = new Data<ClimateState>();
    public TMP_Text climateText;
    public float curEventSale;
    [Header("디버그용")]
    public ClimateState curClimate;
    private void Awake()
    {
        c_State.onChange += ChangeBumperCrop;
        c_State.onChange += ChangeFakeJewel;
        c_State.onChange += ChangeFurnEvent;
        c_State.onChange += ChangeClothEvent;
    }
    private void Start()
    {
        ChangeState();
    }
    public void ChangeState()
    {
        var values = System.Enum.GetValues(typeof(ClimateState));
        int rand = Random.Range(0, values.Length);
        c_State.Value = (ClimateState)values.GetValue(rand);
        curClimate = c_State.Value;
    }

    private void ChangeBumperCrop(ClimateState _cState)
    {
        if(_cState == ClimateState.BumperCrop)
        {
            curEventSale = GetRandomSaleBySort(ItemSorts.food);
            climateText.text = "풍년";
        }
    }
    private void ChangeFakeJewel(ClimateState _cState)
    {
        if (_cState == ClimateState.FakeJewel)
        {
            curEventSale = GetRandomSaleBySort(ItemSorts.accesory);
            climateText.text = "이 보석은 가짜다 !";
        }
    }
    private void ChangeFurnEvent(ClimateState _cState)
    {
        if (_cState == ClimateState.FurnEvent)
        {
            curEventSale = GetRandomSaleBySort(ItemSorts.furniture);
            climateText.text = "유적 발견!";
        }
    }
    private void ChangeClothEvent(ClimateState _cState)
    {
        if (_cState == ClimateState.ClothEvent)
        {
            curEventSale = GetRandomSaleBySort(ItemSorts.clothes);
            climateText.text = "의류 기술 개발";
        }
    }
    public void SetEventPrice(int curPrice,pItem curItem)
    {
        if (c_State.Value == ClimateState.Idle)
            return;
        switch(c_State.Value)
        {
            case ClimateState.BumperCrop when curItem.sort == ItemSorts.food || curItem.sort == ItemSorts.pFood:
            case ClimateState.FakeJewel when curItem.sort == ItemSorts.accesory:
            case ClimateState.ClothEvent when curItem.sort == ItemSorts.clothes:
            case ClimateState.FurnEvent when curItem.sort == ItemSorts.furniture:
                curPrice = curPrice * (int)curEventSale / 100;
                break;
        }
        ItemManager.Instance.currentPrice = curPrice;
    }
    public float GetRandomSaleBySort(ItemSorts targetSort)
    {
        SaleInfo info = sortSales.Find(s => s.sort == targetSort);
        if (info != null)
        {
            return Random.Range(info.minSale, info.maxSale); // min 이상 max 미만
        }
        return 100f; // 못 찾았을 경우
    }
}
