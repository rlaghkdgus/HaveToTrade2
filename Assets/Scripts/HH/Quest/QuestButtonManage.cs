using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestButtonManage : MonoBehaviour
{


    [SerializeField] private List<Button> questButtons;
    [SerializeField] private List<TMP_Text> questTitleTexts;
    [SerializeField] private List<TMP_Text> questDescriptionTexts;
    private int InfoPreIndex;
    private void Awake()
    {
        for (int i = 0; i < questButtons.Count; i++)
        {
            int index = i;
            questButtons[i].onClick.AddListener(() => QuestSystem.Instance.QuestAccept(index));
            
        }
    }
    private void Start()
    {
        for (int i = 0; i < questButtons.Count; i++)
        {
            int randnum = QuestSystem.Instance.questRandIndex[i];
            questTitleTexts[i].text = QuestSystem.Instance.questTable.quest[randnum].questName;
            questDescriptionTexts[i].text = QuestSystem.Instance.questTable.quest[randnum].questText;
        }
    }


}
