using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public structureType s_Type;

    private SpriteRenderer sr;
    private Material origin_M;
    public Material outline_M;

    private Customer customer;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origin_M = sr.material;
        outline_M.mainTexture = GetComponent<SpriteRenderer>().sprite.texture;
        customer = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<Customer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sr.material = outline_M;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sr.material = origin_M;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (s_Type)
        {
            case structureType.Upgrade:
                UIManage.Instance.ShowUI("UpgradeUI");
                UpgradeManager.Instance.UpdateUI();
                break;
            case structureType.Guild:
                Player.Instance.FameBarCheck();
                UIManage.Instance.ShowUI("FameCheckUI");
                break;
            case structureType.Bar:
                if (!QuestSystem.Instance.questSign)
                {
                    QuestSystem.Instance.RandomQuest();
                    UIManage.Instance.ShowUI("QuestDescription");
                }
                break;
            case structureType.Bank:
                UIManage.Instance.ShowUI("BankUI");
                break;
        }
    }
    
}

public enum structureType
{
    Upgrade,
    Bar,
    Guild,
    Bank
}
