using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuestSystem : Singleton<QuestSystem>
{
    [SerializeField] private QuestTable questTable;
    //public List<TMP_Text> questName = new List<TMP_Text>();
    [Header("����Ʈ ��ư")]
    public List<Button> questButtons = new List<Button>();
    [Header("����Ʈ ����")]
    public List<TMP_Text> questDescription = new List<TMP_Text>();
    [Header("����Ʈ ��ȣ Ȯ�ο�")]
    public List<int> questRandIndex = new List<int>();
    [Header("����ƮUI(on/off��)")]
    public GameObject questUI;
    [Header("�������� ����Ʈ �ؽ�Ʈ")]
    public TMP_Text targetQuestText;
    [Header("���� ����Ʈ �������� �����͸� ��� ������")]
    private int currentIndex;
    public ItemSorts qTargetItem;
    public string qTargetItemName;
    public float questTarget;
    public float questGoal;
    private int questReward;
    private VillageType questVillage;
    public QuestType currentQuestType;
    public bool questBuyOrSell; // �춧 true, �ȶ� false;
    [Header("����Ʈ ���� ��ȣ")]
    public bool questSign; // ����Ʈ ���������� Ȯ��
    public bool deliveryCheckSign;
    #region ����Ʈ ���� �� ����
    public void RandomQuest()
    {
        for(int i = 0; i < 3; i++)
        {
            int randnum;
            do
            {
                randnum = Random.Range(0, questTable.quest.Count);
            }while(questRandIndex.Contains(randnum));
            questRandIndex.Add(randnum);
            questDescription[i].text = questTable.quest[randnum].questText;
        }
    }
    public void QuestAccept(int buttonindex)
    {
        currentIndex = questRandIndex[buttonindex];
        qTargetItem = questTable.quest[currentIndex].sort;
        qTargetItemName = questTable.quest[currentIndex].questStuffName;
        questGoal = questTable.quest[currentIndex].questInfo;
        questReward = questTable.quest[currentIndex].reward;
        questBuyOrSell = questTable.quest[currentIndex].buyOrSell;
        currentQuestType = questTable.quest[currentIndex].questType;
        questVillage = questTable.quest[currentIndex].villageType;
        questUI.SetActive(false);
        TextReset();
        questSign = true;
        questRandIndex.Clear();
    }
    public void QuestClear()
    {
        targetQuestText.text = "";
        questTarget = 0;
        questVillage = VillageType.Idle;
        currentQuestType = QuestType.Idle;
        questSign = false;
        deliveryCheckSign = false;
        FameUp();
    }
    public void QuestProgress(pItem ItemCheck, float QuestWeight)
    {
        if (questSign == false || ItemCheck.sort != qTargetItem || Customer.buyOrSell != questBuyOrSell)
            return;
        questTarget += QuestWeight;
        TextReset();
        if (questTarget >= questGoal)
            QuestClear();
    }
    #endregion
    private void Start()
    {
        RandomQuest(); //����Ʈ ���� ��ġ       
    }
  
    private void TextReset()
    {
        if (currentQuestType == QuestType.Trade)
            targetQuestText.text = "target:" + qTargetItem.ToString() + "  " + questTarget + " / " + questGoal;
        else if (currentQuestType == QuestType.Delivery)
            targetQuestText.text = "target:" + qTargetItem.ToString() + " destination :" + questVillage.ToString();
        else
            targetQuestText.text = "";
    }
    private void FameUp()
    {
        switch(qTargetItem)
        {
            case ItemSorts.food:
                Player.Instance.foodFame.fame += questReward;
                break;
            case ItemSorts.furniture:
                Player.Instance.furnFame.fame += questReward;
                break;
            case ItemSorts.accesory:
                Player.Instance.accesoryFame.fame += questReward;
                break;
            case ItemSorts.clothes:
                Player.Instance.clothFame.fame += questReward;
                break;
            case ItemSorts.pFood:
                Player.Instance.pFoodFame.fame += questReward;
                break;
        }
    }
    public void DeliveryQuestCheck()
    {
        if (currentQuestType != QuestType.Delivery || TownManager.Instance.curTown.TownType != questVillage)
            //��� ����Ʈ�� �ƴҽ� UI �߰�����
            return;
        ItemManager.Instance.playerInventory.CheckItemWeight(qTargetItemName);
        if (deliveryCheckSign == false)
            return;
        else if(deliveryCheckSign == true)
        {
            ItemManager.Instance.playerInventory.QuestItemRemove(qTargetItemName);
            QuestClear();
        }
 

    }
}
