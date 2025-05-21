using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<ItemSlotUI> S_uiSlots;
    [SerializeField] private List<ItemSlotUI> B_uiSlots;
    [SerializeField] private GameObject S_SlotPrefab;
    [SerializeField] private GameObject B_SlotPrefab;

    [SerializeField] private Transform S_Inven;
    [SerializeField] private Transform B_Inven;

    [SerializeField] private List<TextMeshProUGUI> WeightTextList; 

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("S_Inven"))
            {
                S_Inven = transform.GetChild(i);
            }
            else if (transform.GetChild(i).CompareTag("B_Inven"))
            {
                B_Inven = transform.GetChild(i);
            }
        }

        for(int i = 0; i < ItemManager.Instance.playerInventory.inventory.Count; i++)
        {
            GenerateSlot();
        }

        InitUI(true);
    }

    public void InitUI(bool isBuy)
    {
        if (isBuy)
        {
            SetAllIndex();
            Show();
        }
        else
        {
            Show();
            SetAllIndex();
        }
        UpdateWeightText();
    }

    public void GenerateSlot()
    {
        GameObject S_SlotCopy = Instantiate(S_SlotPrefab, Vector2.zero, Quaternion.identity);
        S_SlotCopy.transform.SetParent(S_Inven.GetChild(0).GetChild(0));
        S_uiSlots.Add(S_SlotCopy.GetComponent<ItemSlotUI>());

        GameObject B_SlotCopy = Instantiate(B_SlotPrefab, Vector2.zero, Quaternion.identity);
        B_SlotCopy.transform.SetParent(B_Inven.GetChild(0).GetChild(0));
        B_uiSlots.Add(B_SlotCopy.GetComponent<ItemSlotUI>());
    }

    public void DeleteSlot(int index)
    {
        Destroy(S_uiSlots[index].gameObject);
        S_uiSlots.RemoveAt(index);
        Destroy(B_uiSlots[index].gameObject);
        B_uiSlots.RemoveAt(index);
    }

    public void UpdateWeightText()
    {
        List<ItemSorts> keys = new List<ItemSorts>(ItemManager.Instance.playerInventory.sortWeight.Keys);

        for(int i = 0; i < WeightTextList.Count; ++i)
        {
            if(i < keys.Count)
            {
                switch (keys[i])
                {
                    case ItemSorts.food:
                        WeightTextList[i].text = "식품 : " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].CurrentWeight + " / " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].MaxWeight;
                        break;
                    case ItemSorts.pFood:
                        WeightTextList[i].text = "가공식품 : " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].CurrentWeight + " / " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].MaxWeight;
                        break;
                    case ItemSorts.clothes:
                        WeightTextList[i].text = "옷 : " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].CurrentWeight + " / " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].MaxWeight;
                        break;
                    case ItemSorts.furniture:
                        WeightTextList[i].text = "가구 : " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].CurrentWeight + " / " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].MaxWeight;
                        break;
                    case ItemSorts.accesory:
                        WeightTextList[i].text = "장신구 : " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].CurrentWeight + " / " + ItemManager.Instance.playerInventory.sortWeight[keys[i]].MaxWeight;
                        break;
                }
            }
        }
    }

    private void SetAllIndex()
    {
        if(S_uiSlots.Count != 0)
        {
            for (int i = 0; i < S_uiSlots.Count; i++)
            {
                S_uiSlots[i].SetIndex(i);
                B_uiSlots[i].SetIndex(i);
            }
        }
    }

    private void Show()
    {
        if(S_uiSlots.Count != 0)
        {
            for (int i = 0; i < S_uiSlots.Count; i++)
            {
                S_uiSlots[i].Set(ItemManager.Instance.playerInventory.inventory[i]);
                B_uiSlots[i].Set(ItemManager.Instance.playerInventory.inventory[i]);
            }
        }
    }
    #region 디버그용 기능
    private void InvenClear()
    {
        ItemManager.Instance.playerInventory.inventory.Clear();
        B_uiSlots.Clear();
        S_uiSlots.Clear();
        foreach(Transform slot in S_Inven.GetChild(0).GetChild(0))
        {
            Destroy(slot.gameObject);
        }
        foreach(Transform slot in B_Inven.GetChild(0).GetChild(0))
        {
            Destroy(slot.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            InvenClear();
        }
    }
    #endregion
}
