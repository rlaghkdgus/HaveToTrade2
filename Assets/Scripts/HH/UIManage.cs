using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManage : Singleton<UIManage>//동적생성으로 싹다 변경해야함.
{
    [SerializeField] private List<GameObject> uiPrefabs;
    [SerializeField] private List<GameObject> uiField;

    private Dictionary<string, GameObject> uiDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> uiF_dic = new Dictionary<string, GameObject>();

    public GameObject CurrentUI;
    private bool OnUI = false;
    private bool GUISign = false;
    public GameObject basicUI;

    [Header("임시 UI 세팅")]
     public GameObject FameUI;
    public GameObject QuestUI;
    public GameObject BankUI;

    private void Awake()
    {
        foreach(var prefab in uiPrefabs)
        {
            if(prefab != null)
            {
                uiDictionary.Add(prefab.name, prefab);
            }
        }

        foreach(var uiObj in uiField)
        {
            if(uiObj != null)
            {
                uiF_dic.Add(uiObj.name, uiObj);
            }
        }
    }

    private void Update()
    {
        HideUI();
    }

    public void GenerateUI(string uiName)
    {
        if(uiDictionary.TryGetValue(uiName, out GameObject prefab))
        {
            GameObject newUI = Instantiate(prefab, transform);
            CurrentUI = newUI;
            OnUI = true;
            GUISign = true;
        }
        else
        {
            Debug.LogError($"UI Prefab {uiName} not found");
        }
    }

    public void ShowUI(string uiName)
    {
        uiF_dic[uiName].SetActive(true);
        CurrentUI = uiF_dic[uiName];
        OnUI = true;
    }

    public void HideUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnUI && GUISign && CurrentUI.name != "QuestDescription(Clone)")
            {
                if(CurrentUI.tag == "Map")
                {
                    TownManager.Instance.ButtonGroup.SetActive(true);
                    Destroy(CurrentUI);
                }
                Destroy(CurrentUI);
                //CurrentUI.SetActive(false);
                OnUI = false;
                GUISign = false;
            }
            else if(OnUI && !GUISign && CurrentUI.name != "QuestDescription(Clone)")
            {
                CurrentUI.SetActive(false);
                OnUI = false;
            }
            else if(!OnUI)
            {
                ShowUI("Config");
                OnUI = true;
            }
            else if(CurrentUI.name == "QuestDescription(Clone)" && !QuestSystem.Instance.questSign)
            {
                return;
            }
        }
    }
    public void HideQuest() // 퀘스트 UI 닫을때만.
    {
        Destroy(CurrentUI);
        OnUI = false;
    }

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
