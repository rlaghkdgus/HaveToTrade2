using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Town : MonoBehaviour
{
    [SerializeField] private TownDB DB; // 이 버튼이 담고 있는 마을 정보

    public delegate void TownSelected(TownDB seletedTown);
    public static event TownSelected OnTownSelected; // 버튼 눌렀을 때의 이벤트

    public void SetTownImage()
    {
        Image imageSprite = GetComponent<Image>();
        imageSprite.sprite = DB.TownImage;
    }

    public void SetTownDB(TownDB db)
    {
        DB = db;
    }

    public void OnButtonClicked()
    {
        if(OnTownSelected != null)
        {
            OnTownSelected(DB);
        }
    }
}
