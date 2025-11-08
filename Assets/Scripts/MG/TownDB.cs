using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewTownDB", menuName = "DataBase/TownDB")]
public class TownDB : ScriptableObject
{
    public string TownName; // 마을 이름
    public Sprite TownImage;
    public VillageType TownType;
    public bool UseCloud;

    public List<GameObject> TownPrefabs; // 마을 프리팹

    public GameObject RoadPrefabs_F; // 앞
    public GameObject RoadPrefabs_M; // 중간
    public GameObject RoadPrefabs_B; // 뒤
}
