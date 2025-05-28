using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudManager : MonoBehaviour
{
    public List<Sprite> cloudSprites = new List<Sprite>();
    public GameObject cloudPrefab;
    public List<float> xPos = new List<float>();

    public int cloudCount;
    public Vector2 spawnXRange;
    public Vector2 spawnYRange;

    private void Awake()
    {
        xPos = CertainDistanceXpos(cloudCount, spawnXRange);

        for (int i = 0; i < cloudCount; ++i)
        {
            SpawnCloud(i);
        }
    }

    private void SpawnCloud(int index)
    {
        float startX = xPos[index];
        float startY = Random.Range(spawnYRange.x, spawnYRange.y);
        Vector3 spawnPos = new Vector3(startX, startY, 0);

        GameObject cloudClone = Instantiate(cloudPrefab);
        cloudClone.transform.position = spawnPos;
        cloudClone.GetComponent<SpriteRenderer>().sprite = cloudSprites[index];

        CloudMove move = cloudClone.GetComponent<CloudMove>();
        if(move != null)
        {
            move.InitCloud(11.5f - startX, new Vector3(cloudClone.transform.position.x - 15, cloudClone.transform.position.y, cloudClone.transform.position.z));
        }
    }

    private List<float> CertainDistanceXpos(int count, Vector2 range)
    {
        List<float> positons = new List<float>();

        float totalWidth = range.y - range.x;
        float interval = totalWidth / count;

        for(int i = 0; i < count; ++i)
        {
            float baseX = range.x + interval * i;
            float offset = Random.Range(-interval * 0.3f, interval * 0.3f);
            positons.Add(baseX + offset);
        }

        for(int i = 0; i < positons.Count; ++i)
        {
            int j = Random.Range(i, positons.Count);
            (positons[i], positons[j]) = (positons[j], positons[i]);
        }

        return positons;
    }
}
