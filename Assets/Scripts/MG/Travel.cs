using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel : MonoBehaviour
{
    [Header("��� ����� ����")]
    [SerializeField] private float interval = 17.8f; // ��� ����� ����

    [Header("�� ���̾� �ӵ�, F = �Ǿ�, M = �߰�, B = �ǵ�")]
    [SerializeField] private float speed_F = 8f; // �� �� ���̾� �ӵ�
    [SerializeField] private float speed_M = 7f; // �߰� ���̾� �ӵ�
    [SerializeField] private float speed_B = 6f; // �� �� ���̾� �ӵ�

    [Header("���� ���� ���� Ÿ�̹�")]
    [SerializeField] private int NextIndex = 0; // �� ������ ���߰� ���� ������ ������ Ÿ�̹��� ��� ����
    [SerializeField] private int RoadMaxIndex;

    [Header("�� ���̾� ����Ʈ")]
    [SerializeField] private List<GameObject> ForwardList = new List<GameObject>(); // �� �� ���̾� ����Ʈ
    [SerializeField] private List<GameObject> MiddleList = new List<GameObject>(); // �߰� ���̾� ����Ʈ
    [SerializeField] private List<GameObject> BackList = new List<GameObject>(); // �� �� ���̾� ����Ʈ
    private int index_F = 1; // ����� ��ġ ������ ���� index
    private int index_M = 1;
    private int index_B = 1;

    [Header("���� �̵�, ���� ����")]
    public bool OnMove = false;
    [SerializeField] private GameObject curTownClone;
    [SerializeField] private GameObject nextTown;
    public GameObject nextTownClone;
    private VillageType nextTownType;

    [SerializeField] private List<int> RandomRoad;

    [Header("���̵� ��/�ƿ�")]
    [SerializeField] private GameObject fadeUI;
    [SerializeField] private bool isFade = false;
    [SerializeField] private float FadeTime = 1f;

    [Header("�̵� �̺�Ʈ")]
    [SerializeField] RoadEvent roadEvent;
    [Header("�մ� �ŷ���ȣ��")]
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
        // �� ���� ����
        var curRoad = Instantiate<GameObject>(clone, new Vector3(0, 0, 0), Quaternion.identity);
        var nextRoad = Instantiate<GameObject>(clone, new Vector3(0, 0, 0), Quaternion.identity);
        // ���̾� ��ġ�� ���� ����
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
