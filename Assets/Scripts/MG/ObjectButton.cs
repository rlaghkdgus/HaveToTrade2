using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectButton : MonoBehaviour
{
    public structureType s_Type;

    private SpriteRenderer sr;
    private Material origin_M;
    public Material outline_M;

    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origin_M = sr.material;
        outline_M.mainTexture = GetComponent<SpriteRenderer>().sprite.texture;
        
    }


    private void OnMouseEnter()
    {
        sr.material = outline_M;
    }

    private void OnMouseDown()
    {
        switch(s_Type)
        {
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

    private void OnMouseExit()
    {
        sr.material = origin_M;
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
