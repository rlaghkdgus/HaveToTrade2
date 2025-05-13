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
            case structureType.Trade:
                customer.CustomerStart();
                break;
            case structureType.Upgrade:
                UpgradeManager.Instance.UpdateUI();
                break;
            case structureType.Guild:
                UIManage.Instance.GuildUI.SetActive(true);
                break;
            case structureType.Bar:
                UIManage.Instance.BarUI.SetActive(true);
                break;
            case structureType.Bank:
                UIManage.Instance.BankUI.SetActive(true);
                break;
        }
    }
    
}

public enum structureType
{
    Trade,
    Upgrade,
    Bar,
    Guild,
    Bank
}
