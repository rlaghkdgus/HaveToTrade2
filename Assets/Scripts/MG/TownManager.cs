using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    private static TownManager instance;

    [SerializeField] public TownDB curTown; // ���� ���� ����
    [SerializeField] private TownDB nextTown; // ���� ���� ����

    public GameObject TownClone; // ���� ����
    public GameObject ButtonGroup;
    private Travel travel;
    private TownViewChanger changer;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        travel = GetComponent<Travel>();
        changer = GetComponent<TownViewChanger>();
        TownGenerate();
        changer.ButtonUIUpdate(curTown.TownPrefabs);
    }

    public static TownManager Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void OnEnable() // Town ��ư �̺�Ʈ ����
    {
        Town.OnTownSelected += HandleTownSelected;
    }

    private void OnDisable() // ����
    {
        Town.OnTownSelected -= HandleTownSelected;
    }

    private void HandleTownSelected(TownDB town) // �ʿ��� ���� ��ư�� ������ �̵��� ����
    {
        if(curTown != town)
        {
            // ���� ���� ������ Town���� �޾ƿ�
            nextTown = town;
            // Map ����
            var Map = GameObject.FindWithTag("Map");
            Destroy(Map);
            // �� ���� ����
            travel.LoadRoad(TownClone, nextTown.TownPrefabs[0], nextTown);
        }
        else
        {
            Debug.Log("���� ������ ������ ������");
        }
    }

    public void TownGenerate()
    {
        // ���� ���� ���� ����
        TownClone = Instantiate<GameObject>(curTown.TownPrefabs[changer.currentIndex], new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log("Ÿ�� ���� �Ϸ�");
    }

    public void UpdateTown()
    {
        curTown = nextTown;
        nextTown = null;
        TownClone = GameObject.FindGameObjectWithTag("Town");
        var MapButton = GameObject.FindWithTag("Canvas").transform.Find("ButtonGroup").transform.Find("OpenMap");
        MapButton.gameObject.SetActive(true);
        Debug.Log("������Ʈ �Ϸ�");
        changer.currentIndex = 0;
        changer.ButtonUIUpdate(curTown.TownPrefabs);
        ButtonGroup.SetActive(true);
    }
    
    public void ViewChangeButtonAction(int index)
    {
        changer.ViewChange(index, curTown.TownPrefabs);
    }
}
