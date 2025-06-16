using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemManager : Singleton<ItemManager>
{
    [Header("아이템데이터")]
    [SerializeField] ItemSO itemSO;
    [Header("손님 참조")]
    [SerializeField] Customer customer;
    [Header("플레이어 데이터 아이템")]
    public InventoryContainer playerInventory;
    public InventoryUI inventoryUI;
    [Header("품목별 개수 제한")]
    public int itemCountLimit;
    [Header("데이터 교환시 사용할 리스트(UI포함)")]
    public List<int> productIndex;
    public List<int> itemCountIndex;
    private int maxItemCount;
    [SerializeField] TMP_InputField itemCountInputField;
    private int tempItemcount;
    [Header("디버그용")]
    public int productCount = 0;
    private List<int> randIndex = new List<int>();
    private int bargainPrice = 0;
    public bool bargainSuccess = false;
    public int currentProductIndex = 0;
    public pItem buyItem;
    public List<pItem> SelledItems;
    private static ItemManager instance;

    VillageType curVill;
    public GameObject weightPopUp;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerInventory.InitWeight();
    }

    #region 거래전 아이템 세팅
    public void RandomSetItem(int sortcount)
    {
        curVill = TownManager.Instance.curTown.TownType;
        for (int i = 0; i < sortcount; i++)
        {
            int randnum;
            do
            {
                randnum = Random.Range(0, itemSO.items.Length);
            }
            while (randIndex.Contains(randnum));
            randIndex.Add(randnum);
            productIndex.Add(randnum);
        }
        randIndex.Clear();
    }

    public void RandomSetItemSell(int sortcount)
    {
        if (sortcount > playerInventory.inventory.Count)
            sortcount = playerInventory.inventory.Count;
        for (int i = 0; i < sortcount; i++)
        {
            /*int randnum;
            do
            {
                randnum = Random.Range(0, playerInventory.inventory.Count);
            }
            while (randIndex.Contains(randnum) || playerInventory.inventory.Count - 1 < randnum);
            randIndex.Add(randnum);
            */
            productIndex.Add(0);
        }
        randIndex.Clear();
    }

    public void FixedSetItem(int setIndex)
    {
        productIndex.Clear();
        productIndex.Add(setIndex);    
    }
    #endregion

    #region 거래 UI세팅
    public void SetUI()
    {
        currentProductIndex = productIndex[productCount];
        int randCount = Random.Range(1, itemCountLimit + 1);
        PutInfo(randCount);
    }

    public void SetSellUI()
    {
        int randCount;
        pItem candidate;
        int tryCount = 0;
        const int maxTries = 30; // 무한 루프 방지용
        do
        {
            productIndex[productCount] = Random.Range(0, playerInventory.inventory.Count);
            currentProductIndex = productIndex[productCount];
            candidate = playerInventory.inventory[currentProductIndex];
            tryCount++;

            // 무한 루프 방지 조건
            if (tryCount > maxTries)
            {
                Debug.LogWarning("판매 아이템 설정에 너무 많은 반복 발생. 중복이더라도 강제 선택.");
                break;
            }

        } while (SelledItems.Exists(item => item.stuffName == candidate.stuffName));

        // 유효한 아이템 선택 완료
        SelledItems.Add(candidate);

        // 수량 결정
        if (candidate.counts > itemCountLimit)
        {
            randCount = Random.Range(1, itemCountLimit + 1);
        }
        else
        {
            randCount = Random.Range(1, candidate.counts + 1);
        }

        PutInfo(randCount);
    }
    #endregion
#region 아이템 이름 반환
    public string ItemNameReturn()
    {
        string itemName;
        if(customer.buyOrSell)
        {
            itemName = itemSO.items[currentProductIndex].stuffName;
        }
        else
        {
            if (playerInventory.inventory == null)
                itemName = null;
            itemName = playerInventory.inventory[currentProductIndex].stuffName;
        }
        return itemName;
    }

    #endregion
    #region 구매 및 판매 실제 과정
    public void BuyProduct()
    {
        int currentPrice;
        if (bargainSuccess)
            currentPrice = bargainPrice * itemCountIndex[productCount] * (int)salePoint(buyItem.sort) / 100;
        else
            currentPrice = itemSO.items[currentProductIndex].price * itemCountIndex[productCount] * (int)salePoint(buyItem.sort) / 100;

        if (Player.Instance.money < currentPrice)
        {
            productCount++;
            BargainClear();
            return;
        }

        float itemTotalWeight = itemSO.items[currentProductIndex].weight * itemCountIndex[productCount];
        playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight += itemTotalWeight;

        if (playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight >
            playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].MaxWeight)
        {
            Debug.LogError("인벤토리 용량 초과 : " + playerInventory.sortWeight[itemSO.items[currentProductIndex].sort]);
            playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight -= itemTotalWeight;
            StartCoroutine("WeightOverPopUp");
            productCount++;
            buyItem = null;
            BargainClear();
            return;
        }

        Player.Instance.money -= currentPrice;

        pItem existingItem = playerInventory.inventory.Find(item => item.stuffName == buyItem.stuffName);

        if (existingItem != null)
        {
            existingItem.counts += itemCountIndex[productCount];
            inventoryUI.InitUI(true);
        }
        else
        {
            buyItem.counts = itemCountIndex[productCount];
            playerInventory.inventory.Add(buyItem);
            inventoryUI.GenerateSlot();
            inventoryUI.InitUI(true);
        }

        if (QuestSystem.Instance.currentQuestType == QuestType.Trade && QuestSystem.Instance.questBuyOrSell)
            QuestSystem.Instance.QuestProgress(buyItem, itemTotalWeight,curVill);

        productCount++;
        buyItem = null;
        BargainClear();
    }

    public void SellProduct()
    {
        int currentPrice;
        pItem itemToRemove = playerInventory.inventory[currentProductIndex];

        if (bargainSuccess)
            currentPrice = bargainPrice * itemCountIndex[productCount];
        else
            currentPrice = playerInventory.inventory[currentProductIndex].price * itemCountIndex[productCount];

        Player.Instance.money += currentPrice;
        playerInventory.inventory[currentProductIndex].counts -= itemCountIndex[productCount];

        if (playerInventory.inventory[currentProductIndex].counts <= 0)
        {
            playerInventory.inventory.RemoveAt(currentProductIndex);
            inventoryUI.DeleteSlot(currentProductIndex);
        }

        inventoryUI.InitUI(false);

        float itemTotalWeight = itemToRemove.weight * itemCountIndex[productCount];
        if (playerInventory.sortWeight[itemToRemove.sort].CurrentWeight <= playerInventory.sortWeight[itemToRemove.sort].MaxWeight)
        {
            playerInventory.sortWeight[itemToRemove.sort].CurrentWeight -= itemTotalWeight;
        }

        if (QuestSystem.Instance.currentQuestType == QuestType.Trade && !QuestSystem.Instance.questBuyOrSell)
            QuestSystem.Instance.QuestProgress(itemToRemove, itemTotalWeight,curVill);

        productCount++;
        BargainClear();
    }
    #endregion

    #region 흥정 과정
    public void SetBargainPrice(float initialChance, int bargainValue, int bargainPoint, float bargainPercent)
    {
        float randomValue = Random.Range(0f, 100f);
        float chancePoint;
        float totalChance;

        if (customer.buyOrSell)
        {
            chancePoint = 100 -((float)bargainValue / itemSO.items[currentProductIndex].price) * 100f;
            
        }
        else
        {
            if (bargainValue < 0)
                bargainValue = 0; // 음수 흥정시 바로 0으로 변경

            float basePrice = playerInventory.inventory[currentProductIndex].price;
            float ratio = (float)bargainValue / basePrice;

            if (ratio >= 2f)
            {
                // 2배 이상이면 확률 0%
                chancePoint = 100f;
            }
            else if (ratio <= 1f)
            {
                // 원가 이하이면 확률 100%
                chancePoint = 0f;
            }
            else
            {
                // 선형적으로 감소: 1배일 때 0%, 2배일 때 100%
                chancePoint = (ratio - 1f) * 100f;
            }

        }
        totalChance = initialChance - chancePoint;



        if (totalChance >= randomValue)
        {
            bargainPrice = bargainValue;
            customer.costText.text = " " + bargainPrice;
            bargainSuccess = true;
        }
        else
        {
            bargainSuccess = false;
        }
    }
    #endregion

    #region 초기화 및 유틸
    public void ListClear()
    {
        productIndex.Clear();
        if (SelledItems != null)
        {
            SelledItems.Clear();
        }
        itemCountIndex.Clear();
        productCount = 0;
        
    }

    public void BargainClear()
    {
        bargainPrice = 0;
        bargainSuccess = false;
    }

    public void PutInfo(int randCount)
    {
        itemCountIndex.Add(randCount);
        maxItemCount = itemCountIndex[productCount];
        if (customer.buyOrSell)
        {
            buyItem = new pItem(itemSO.items[currentProductIndex]);
            pItem countItem = playerInventory.inventory.Find(item => item.stuffName == buyItem.stuffName);
            customer.playerCountTexts.text = "" + (countItem != null ? countItem.counts : 0);
            customer.productImages.sprite = itemSO.items[currentProductIndex].image;
            customer.costText.text = " " + itemSO.items[currentProductIndex].price;
        }
        else
        {
            customer.playerCountTexts.text = "" + playerInventory.inventory[currentProductIndex].counts;
            customer.productImages.sprite = playerInventory.inventory[currentProductIndex].image;
            customer.costText.text = " " + playerInventory.inventory[currentProductIndex].price;
        }

        CusProductCountSet();
        customer.productTexts.text = "" + itemCountIndex[productCount];
    }

    IEnumerator WeightOverPopUp()
    {
        GameObject popup = Instantiate(weightPopUp, GameObject.FindGameObjectWithTag("Canvas").transform);
        yield return new WaitForSeconds(1f);
        Destroy(popup);
    }
    #endregion

    private void CusProductCountSet()
    {
        switch (customer.currentCusList[customer.randcusnum].customerNum)
        {
            case 1:
                if (customer.buyOrSell == true && buyItem.sort == ItemSorts.food)
                {
                    maxItemCount = itemCountIndex[productCount] += 5;
                }
                else if (customer.buyOrSell == false && playerInventory.inventory[currentProductIndex].sort == ItemSorts.food)
                {
                    maxItemCount = itemCountIndex[productCount] += 5;
                    if (itemCountIndex[productCount] >= playerInventory.inventory[currentProductIndex].counts)
                        maxItemCount = itemCountIndex[productCount] = playerInventory.inventory[currentProductIndex].counts;
                }
                break;
        }
    }

    private float salePoint(ItemSorts sorts)
    {
        float salePercent = 100.0f;
        switch (sorts)
        {
            case ItemSorts.food:
                salePercent -= Player.Instance.foodFame.sale;
                break;
            case ItemSorts.pFood:
                salePercent -= Player.Instance.pFoodFame.sale;
                break;
            case ItemSorts.accesory:
                salePercent -= Player.Instance.accesoryFame.sale;
                break;
            case ItemSorts.clothes:
                salePercent -= Player.Instance.clothFame.sale;
                break;
            case ItemSorts.furniture:
                salePercent -= Player.Instance.furnFame.sale;
                break;
        }
        return salePercent;
    }
    public void PlusItemCount()
    {
        if (maxItemCount > itemCountIndex[productCount])
        {
            itemCountIndex[productCount]++;
            customer.productTexts.text = "" + itemCountIndex[productCount];
        }   
    }
    public void MinusItemCount()
    {
        if (itemCountIndex[productCount] > 1)
        {
            itemCountIndex[productCount]--;
            customer.productTexts.text = "" + itemCountIndex[productCount];
        }
    }
    public void SetItemCount()
    {
        StartCoroutine(SetItemCountCycle());
    }
    IEnumerator SetItemCountCycle()
    {
        Debug.Log("a");
        if (int.TryParse(itemCountInputField.text, out tempItemcount))//파싱
        {
            if(tempItemcount < 1 || tempItemcount > maxItemCount)
            {
                yield break;
            }
            else
            {
                itemCountIndex[productCount] = tempItemcount;
                customer.productTexts.text = "" + itemCountIndex[productCount];
            }
            itemCountInputField.text = "";
        }
        else
        {
            Debug.Log("Error");
            yield return null;
            //정수 이외 다른 값일시 돌아가도록
        }
    }
}