using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManage : Singleton<UIManage>//������������ �ϴ� �����ؾ���.
{
    public GameObject GuildUI;
    public GameObject BarUI;
    public GameObject BankUI;
    public GameObject basicUI;

    ///public GameObject wolfMiniUI;

    public void wolfMinigame(bool gameState)
    {
        if (gameState)
        {
            basicUI.SetActive(false);
            ///wolfMiniUI.SetActive(true);
        }
        else
        {
           // wolfMiniUI.SetActive(false);
            basicUI.SetActive(true);
        }
    }

    
}
