using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<TownDB> townDB_List = new List<TownDB>();
    [SerializeField] private List<Town> townList = new List<Town>();

    private void OnEnable()
    {
        Town[] towns = GetComponentsInChildren<Town>(true);

        foreach (Town town in towns)
        {
            if(town.transform != transform)
            {
                townList.Add(town);
            }
        }

        for (int i = 0; i < townDB_List.Count; ++i)
        {
            if (townDB_List[i] == TownManager.Instance.curTownDataCall())
            {
                townDB_List.RemoveAt(i);
            }
        }

        for (int i = 0; i < townDB_List.Count; ++i)
        {
            townList[i].SetTownDB(townDB_List[i]);
            townList[i].SetTownImage();
        }
    }
}
