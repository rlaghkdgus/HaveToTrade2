using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEvent : MonoBehaviour
{
    public int wolfCountSet;             
    public GameObject wolfPrefab;
    public float spawnRadius = 5f;
    public Transform EventSpawn;
    public static int wolfCount;
    GameObject spawner;
    GameObject thisObject;
    RoadEvent roadEvent;

    private void Awake()
    {
        UIManage.Instance.wolfMinigame(true);
        settingSpawnPos();
        wolfCount = wolfCountSet;
    
    }
    private void Start()
    {
        roadEvent = FindObjectOfType<RoadEvent>();
        thisObject = GameObject.FindWithTag("WolfMiniGame");
        StartCoroutine(SpawnWolves());
        StartCoroutine(GameEnd());
    }
    void settingSpawnPos()
    {
        spawner = GameObject.FindWithTag("RoadEvent");
    }
    IEnumerator SpawnWolves()
    {
        for (int i = 0; i < wolfCountSet; i++)
        {    
            Vector2 spawnOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = (Vector2)spawner.transform.position + spawnOffset;
            spawnPos.z = 0f; // 카메라 시야에 맞게 조정
            GameObject wolf = Instantiate(wolfPrefab, spawnPos, Quaternion.identity, spawner.transform);
            yield return YieldCache.WaitForSeconds(1f);
        }
    }
    IEnumerator GameEnd()
    {
        while(true)
        {
            if (wolfCount <= 0)
            {
                roadEvent.rState.Value = RoadEventState.Idle;
                Destroy(thisObject);
            }
            yield return YieldCache.WaitForSeconds(1f);
        }
    }
    private void OnDestroy()
    {
        UIManage.Instance.wolfMinigame(false);
    }
   
}
