using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManage : Singleton<UIManage>//동적생성으로 싹다 변경해야함.
{
    [SerializeField] private List<GameObject> uiPrefabs;
    [SerializeField] private List<GameObject> uiField;
    [SerializeField] private GameObject FadeUI;
    [SerializeField] private float FadeTime = 1f;
    private Dictionary<string, GameObject> uiDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> uiF_dic = new Dictionary<string, GameObject>();


    private GameObject _curUI;
    public GameObject CurrentUI 
    {
        get => _curUI;
        set
        {
            if(_curUI != null && value == null)
            {
                EventManager.OnTownBuildingUIOff();
            }

            _curUI = value;
        }
    }
    private bool OnUI = false;
    private bool GUISign = false;
    public GameObject basicUI;

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
            GameObject newUI = Instantiate(prefab, transform.GetChild(0));
            newUI.transform.SetSiblingIndex(transform.GetChild(0).childCount - 3);
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
        if (uiName == "BankUI")
        {
            StartCoroutine(BankActive(uiName));
            return;
        }
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
                    OnUI = false;
                    GUISign = false;
                    return;
                }
               
                Destroy(CurrentUI);
                CurrentUI = null;
                //CurrentUI.SetActive(false);
                OnUI = false;
                GUISign = false;
            }
            else if(OnUI && !GUISign && CurrentUI.name != "QuestDescription(Clone)")
            {
                CurrentUI.SetActive(false);
                CurrentUI = null;
                OnUI = false;
            }
            else if(!OnUI)
            {
                GenerateUI("Config");
                OnUI = true;
                GUISign = true;
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
        CurrentUI = null;
        OnUI = false;
        GUISign = false;
    }
    public void HideBank()
    {
        CurrentUI.SetActive(false);
        OnUI = false; 
    }
    IEnumerator BankActive(string uiName)
    {
        GameObject fade = Instantiate(FadeUI, GameObject.FindGameObjectWithTag("Canvas").transform);
        yield return new WaitForSeconds(FadeTime);
        uiF_dic[uiName].SetActive(true);
        CurrentUI = uiF_dic[uiName];
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
