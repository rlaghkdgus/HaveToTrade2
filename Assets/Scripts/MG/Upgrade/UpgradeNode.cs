using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNode : MonoBehaviour
{
    public int ID;

    public void UpgradeInfoChange()
    {
        Player.Instance.Upgrade_ID = ID;
        UpgradeManager.Instance.UpdateOutline();
        ItemManager.Instance.playerInventory.InitWeight();
    }

    public void ActiveOutline(bool OnOff)
    {
        GetComponent<Outline>().enabled = OnOff;
    }
}
