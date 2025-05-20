using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManage : Singleton<UIManage>
{
    [SerializeField] private List<GameObject> uiPrefabs;
    [SerializeField] private List<GameObject> uiField;

    private Dictionary<string, GameObject> uiDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> uiF_dic = new Dictionary<string, GameObject>();

    private GameObject CurrentUI;
    private bool OnUI = false;

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
            if (OnUI)
            {
                //Destroy(CurrentUI);
                CurrentUI.SetActive(false);
                OnUI = false;
            }
            else
            {
                ShowUI("Config");
                OnUI = true;
            }
        }
    }
}
