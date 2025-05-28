using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel : MonoBehaviour
{
    [Header("배경 재생성 간격")]
    [SerializeField] private float interval = 17.8f; // 배경 재생성 간격

    [Header("각 레이어 속도, F = 맨앞, M = 중간, B = 맨뒤")]
    [SerializeField] private float speed_F = 8f; // 맨 앞 레이어 속도
    [SerializeField] private float speed_M = 7f; // 중간 레이어 속도
    [SerializeField] private float speed_B = 6f; // 맨 뒤 레이어 속도

    [Header("다음 마을 생성 타이밍")]
    [SerializeField] private int NextIndex = 0; // 길 생성을 멈추고 다음 마을을 생성할 타이밍을 재는 변수
    [SerializeField] private int RoadMaxIndex;

    [Header("각 레이어 리스트")]
    [SerializeField] private List<GameObject> ForwardList = new List<GameObject>(); // 맨 앞 레이어 리스트
    [SerializeField] private List<GameObject> MiddleList = new List<GameObject>(); // 중간 레이어 리스트
    [SerializeField] private List<GameObject> BackList = new List<GameObject>(); // 맨 뒤 레이어 리스트
    private int index_F = 1; // 재생성 위치 지정을 위한 index
    private int index_M = 1;
    private int index_B = 1;

    [Header("마을 이동, 생성 관련")]
    public bool OnMove = false;
    [SerializeField] private GameObject curTownClone;
    [SerializeField] private GameObject nextTown;
    public GameObject nextTownClone;
    private VillageType nextTownType;

    [SerializeField] private List<int> RandomRoad;

    [Header("페이드 인/아웃")]
    [SerializeField] private GameObject fadeUI;
    [SerializeField] private bool isFade = false;
    [SerializeField] private float FadeTime = 1f;

    [Header("이동 이벤트")]
    [SerializeField] RoadEvent roadEvent;
    [Header("손님 거래신호용")]
    [SerializeField] Customer customer;
    private void CombinationRoad(TownDB nextTownDB)
    {
        RandomRoad.Clear();
        RandomRoad.Add(Random.Range(0, nextTownDB.RoadPrefabs_F.Count));
        RandomRoad.Add(Random.Range(0, nextTownDB.RoadPrefabs_M.Count));
        RandomRoad.Add(Random.Range(0, nextTownDB.RoadPrefabs_B.Count));
    }
   
    
    private void CopyRoad(GameObject clone, int type) // forward = 0, middle = 1, back = 2
    {
        // 길 동적 생성
        var curRoad = Instantiate<GameObject>(clone, new Vector3(0, 0, 0), Quaternion.identity);
        var nextRoad = Instantiate<GameObject>(clone, new Vector3(0, 0, 0), Quaternion.identity);
        // 레이어 위치에 따라 구분
        switch (type)
        {
            case 0:
                ForwardList.Add(curRoad);
                ForwardList.Add(nextRoad);
                break;
            case 1:
                MiddleList.Add(curRoad);
                MiddleList.Add(nextRoad);
                break;
            case 2:
                BackList.Add(curRoad);
                BackList.Add(nextRoad);
                break;
        }
        SortBG(type);
    }

    public void LoadRoad(GameObject curTown, GameObject nextTown, TownDB nextTownDB)
    {
        CombinationRoad(nextTownDB);
        CopyRoad(nextTownDB.RoadPrefabs_F[RandomRoad[0]], 0);
        CopyRoad(nextTownDB.RoadPrefabs_M[RandomRoad[1]], 1);
        CopyRoad(nextTownDB.RoadPrefabs_B[RandomRoad[2]], 2);
        nextTownType = nextTownDB.TownType;
        curTownClone = curTown;
        this.nextTown = nextTown;
        StartCoroutine(MoveRoad());
    }

    private void InitRoad()
    {
        ForwardList.Clear();
        MiddleList.Clear();
        BackList.Clear();
        RandomRoad.Clear();

        GameObject[] RoadObj = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject obj in RoadObj)
        {
            Destroy(obj);
        }
        NextIndex = 0;
        index_F = 1;
        index_M = 1;
        index_B = 1;
        curTownClone = null;
        nextTown = null;
        nextTownClone = null;
    }

    private void SortBG(int type)
    {
        List<GameObject> RoadList = null;
        switch (type)
        {
            case 0:
                RoadList = ForwardList;
                break;
            case 1:
                RoadList = MiddleList;
                break;
            case 2:
                RoadList = BackList;
                break;
        }

        for (int i = 0; i < RoadList.Count; i++)
        {
            var curBG = RoadList[i];
            curBG.transform.localPosition = Vector3.right * interval * i;
        }
    }

    private IEnumerator MoveRoad()
    {
        GameObject fadeInOut = Instantiate(fadeUI, GameObject.FindGameObjectWithTag("Canvas").transform);
        GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");

        yield return new WaitForSeconds(FadeTime);
        switch (nextTownType)
        {
            case VillageType.Smokian:
                SoundManager.Instance.BGMplay(true, BGMtype.MeatRoad);
                break;
            case VillageType.GoldBen:
                SoundManager.Instance.BGMplay(true, BGMtype.MineRoad);
                break;
        }
        for (int i = 0; i < clouds.Length; ++i)
        {
            Destroy(clouds[i]);
        }
        if (curTownClone != null)
        {
            Destroy(curTownClone);
        }
        Player.Instance.AnimationChange(true);
        OnMove = true;
        yield return YieldCache.WaitForSeconds(1.0f);
        //roadEvent.RoadEventSet();
        if (roadEvent.rState.Value != RoadEventState.Idle)
        {
            OnMove = false;
        }
        
    }



   
    private void Update()
    {
        if (OnMove)
            MoveBackGround();

        if (isFade)
            StartCoroutine(ArriveTown());
    }

    private void MoveBackGround()
    {
        for (int i = 0; i < ForwardList.Count; i++)
        {
            ForwardList[i].transform.localPosition += Vector3.left * Time.deltaTime * speed_F;
            if (NextIndex < RoadMaxIndex)
            {
                if (ForwardList[i].transform.localPosition.x <= -interval)
                {
                    ForwardList[i].transform.localPosition = new Vector3(ForwardList[index_F].transform.localPosition.x + interval, 0f, 0f);
                    index_F = (index_F + 1) % ForwardList.Count;
                    NextIndex++;
                }
            }
            else if (nextTownClone == null && NextIndex == RoadMaxIndex)
            {
                isFade = true;
                NextIndex++;
                //new Vector3(ForwardList[index_F].transform.localPosition.x + interval, 0f, 0f)
            }
        }

        for (int i = 0; i < MiddleList.Count; i++)
        {
            MiddleList[i].transform.localPosition += Vector3.left * Time.deltaTime * speed_M;
            if (MiddleList[i].transform.localPosition.x <= -interval)
            {
                MiddleList[i].transform.localPosition = new Vector3(MiddleList[index_M].transform.localPosition.x + interval, 0f, 0f);
                index_M = (index_M + 1) % MiddleList.Count;
            }
        }

        for (int i = 0; i < BackList.Count; i++)
        {
            BackList[i].transform.localPosition += Vector3.left * Time.deltaTime * speed_B;
            if (BackList[i].transform.localPosition.x <= -interval)
            {
                BackList[i].transform.localPosition = new Vector3(BackList[index_B].transform.localPosition.x + interval, 0f, 0f);
                index_B = (index_B + 1) % BackList.Count;
            }
        }
    }

    private IEnumerator ArriveTown()
    {
        isFade = false;
        GameObject fadeInout = Instantiate(fadeUI, GameObject.FindGameObjectWithTag("Canvas").transform);

        yield return new WaitForSeconds(FadeTime);
        nextTownClone = Instantiate<GameObject>(nextTown, Vector3.zero, Quaternion.identity);
        Player.Instance.AnimationChange(false);
        switch (nextTownType)
        {
            case VillageType.Smokian:
                SoundManager.Instance.BGMplay(true, BGMtype.Meat);
                break;
            case VillageType.GoldBen:
                SoundManager.Instance.BGMplay(true, BGMtype.Mine);
                break;
        }
        yield return new WaitForSeconds(FadeTime);
        OnMove = false;
        InitRoad();

        TownManager.Instance.UpdateTown();
        customer.tradeOn();
        isFade = false;
        yield return null;
    }
}
