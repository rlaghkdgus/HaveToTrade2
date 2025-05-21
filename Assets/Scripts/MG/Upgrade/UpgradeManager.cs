using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public UpgradeExcelData UpgradeData;

    public List<UpgradeNode> nodes;

    private Dictionary<int, UpgradeInfo> U_info = new Dictionary<int, UpgradeInfo>();

    private void Awake()
    {
        for(int i = 0; i < UpgradeData.Carriage.Count; ++i)
        {
            U_info.Add(UpgradeData.Carriage[i].ID, UpgradeData.Carriage[i]);
        }
    }

    public UpgradeInfo InfoSearch_ID(int id)
    {
        return U_info[id];
    }

    public void UpdateUI()
    {
        UpdateName();
        UpdateOutline();
        UpdateUnlock();
    }

    private void UpdateName()
    {
        for(int i = 0; i < nodes.Count; ++i)
        {
            nodes[i].GetComponentInChildren<TextMeshProUGUI>().text = U_info[nodes[i].ID].Name;
        }
    }

    public void UpdateOutline()
    {
        for(int i = 0; i < nodes.Count; ++i)
        {
            if (nodes[i].ID == Player.Instance.Upgrade_ID)
            {
                nodes[i].ActiveOutline(true);
            }
            else
            {
                nodes[i].ActiveOutline(false);
            }
        }
    }

    private void UpdateUnlock()
    {
        for(int i = 0; i < nodes.Count; ++i)
        {
            switch (nodes[i].ID / 10)
            {
                case 1:
                    if (nodes[i].ID % 10 <= Player.Instance.foodFame.tier)
                    {
                        if (nodes[i].ID % 10 < 4)
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                        else if (Player.Instance.priorityFame.Contains(Player.Instance.foodFame))
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                        
                    }
                    else
                    {
                        nodes[i].GetComponent<Button>().interactable = false;
                    }
                    break;
                case 2:
                    if (nodes[i].ID % 10 <= Player.Instance.pFoodFame.tier)
                    {
                        if (nodes[i].ID % 10 < 4)
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                        else if (Player.Instance.priorityFame.Contains(Player.Instance.pFoodFame))
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                    }
                    else
                    {
                        nodes[i].GetComponent<Button>().interactable = false;
                    }
                    break;
                case 3:
                    if (nodes[i].ID % 10 <= Player.Instance.clothFame.tier)
                    {
                        if (nodes[i].ID % 10 < 4)
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                        else if (Player.Instance.priorityFame.Contains(Player.Instance.clothFame))
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                    }
                    else
                    {
                        nodes[i].GetComponent<Button>().interactable = false;
                    }
                    break;
                case 4:
                    if (nodes[i].ID % 10 <= Player.Instance.furnFame.tier)
                    {
                        if (nodes[i].ID % 10 < 4)
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                        else if (Player.Instance.priorityFame.Contains(Player.Instance.furnFame))
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                    }
                    else
                    {
                        nodes[i].GetComponent<Button>().interactable = false;
                    }
                    break;
                case 5:
                    if (nodes[i].ID % 10 <= Player.Instance.accesoryFame.tier)
                    {
                        if (nodes[i].ID % 10 < 4)
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                        else if (Player.Instance.priorityFame.Contains(Player.Instance.accesoryFame))
                        {
                            nodes[i].GetComponent<Button>().interactable = true;
                        }
                    }
                    else
                    {
                        nodes[i].GetComponent<Button>().interactable = false;
                    }
                    break;
            }
        }
    }
}
