using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemManager : Singleton<ItemManager>
{
    [Header("�����۵�����")]
    [SerializeField] ItemSO itemSO;
    [Header("�÷��̾� ������ ������")]
    //public List<pItem> playerItems;
    public InventoryContainer playerInventory;
    public InventoryUI inventoryUI;
    [Header("ǰ�� ���� ����")]
    public int itemCountLimit;// ǰ�� ���� ����
    [Header("������ ��ȯ�� ����� ����Ʈ(UI����)")]
    public List<int> productIndex; // �����ε��� �������
    public List<int> itemCountIndex;// ������ ���� ���� ����
    
    
    

    // ���� �������
    public int productCount = 0;//��ǰ ���� �з�
    private List<int> randIndex = new List<int>();// �ٸ� ������ �������� �̱����� ����Ʈ
    private int bargainPrice = 0; //������ ����
    public bool bargainSuccess = false;
    public int currentProductIndex = 0;
    public pItem buyItem;
    private static ItemManager instance;

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
  #region �ŷ��� ������ ����
  public void RandomSetItem(int sortcount)//Customer��ũ��Ʈ���� sortCount���޾ƿ�
  {
    for (int i = 0; i < sortcount; i++)
    {
       int randnum;
       // ���ο� ���� ��ȣ�� ã�� ������ �ݺ�
       do
       {
          randnum = Random.Range(0, itemSO.items.Length);
       }
       while (randIndex.Contains(randnum)); // �̹� ���� ��ȣ��� �ٽ� �̱�
       randIndex.Add(randnum); // ������ȣ ����
       productIndex.Add(randnum); //��ǰ �߰�, ����Ʈ�� ��ȣ�� ������ ��(���� ��������..�Ƹ�)
    }
    randIndex.Clear();//������ȣ ����
    
    }
    public void RandomSetItemSell(int sortcount)//�ǸŽ� ���� ������ ����
    {
        if(sortcount > playerInventory.inventory.Count)//���� �޾ƿ� ������ �÷��̾� �����ۿ� �ִ� �������� ���� ���
            sortcount = playerInventory.inventory.Count;
        for(int i = 0; i< sortcount; i++)
        {
            int randnum;
            // ���ο� ���� ��ȣ�� ã�� ������ �ݺ�
            do
            {
                randnum = Random.Range(0, playerInventory.inventory.Count);
            }
            while (randIndex.Contains(randnum) || playerInventory.inventory.Count - 1 < randnum); // �̹� ���� ��ȣ Ȥ�� �÷��̾ ������ ������ ���� ���� ��� �ٽû̱�
            randIndex.Add(randnum); // ������ȣ ����
            productIndex.Add(randnum); //��ǰ �߰�, ����Ʈ�� ��ȣ�� ������ ��(���� ��������..�Ƹ�)
        }
        randIndex.Clear();//������ȣ ����
    }
#endregion
    
    #region �ŷ� UI����
    public void SetUI()//���Ž� UI
    {
        currentProductIndex = productIndex[productCount];
        int randCount = Random.Range(1, itemCountLimit+1);//��ǰ�� ������ �󸶳� �ŷ��Ұ��� �������� ����
        PutInfo(randCount);
    }
    public void SetSellUI()//�ǸŽ� UI
    {
        int randCount;
        if(productIndex[productCount] >= playerInventory.inventory.Count)
            productIndex[productCount] = Random.Range(0, playerInventory.inventory.Count);
        currentProductIndex = productIndex[productCount];
        if(playerInventory.inventory[currentProductIndex].counts > itemCountLimit)
        {
            randCount = Random.Range(1,itemCountLimit+1);
        }
        else
        {
            randCount = Random.Range(1, playerInventory.inventory[currentProductIndex].counts + 1);
        }
        PutInfo(randCount);
    }
    #endregion
    #region ���Ź� �Ǹ� ���� ����
    public void BuyProduct() //���Ž�
    {
        int currentPrice;

        if (bargainSuccess == true)
            currentPrice = bargainPrice * itemCountIndex[productCount] * (int)salePoint(buyItem.sort) / 100;
        else
           currentPrice = itemSO.items[currentProductIndex].price * itemCountIndex[productCount] * (int)salePoint(buyItem.sort) / 100;

        if (Player.Instance.money < currentPrice)
        {
            productCount++;
            BargainClear();
            //�Ұ��� �ۼ�����
            return;
        }

        #region ���� ��� �κ�
        float itemTotalWeight = itemSO.items[currentProductIndex].weight * itemCountIndex[productCount];

        playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight += itemTotalWeight;
        if(playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight > playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].MaxWeight)
        {
            float over = playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight - playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].MaxWeight;
            over = Mathf.Min(over, itemTotalWeight);

            if (playerInventory.PublicCurrentWeight + over > playerInventory.PublicMaxWeight)
            {
                productCount++;
                playerInventory.sortWeight[itemSO.items[currentProductIndex].sort].CurrentWeight -= itemTotalWeight;
                Debug.LogWarning("Cannot add item");
                return;
            }
            playerInventory.PublicCurrentWeight += over;
        }
        
        #endregion

        Player.Instance.money -= currentPrice; // ���� ��� ����


        // playerItems ����Ʈ�� ���� �������� �ִ��� Ȯ��
        pItem existingItem = playerInventory.inventory.Find(item => item.stuffName == buyItem.stuffName);

        if (existingItem != null)
        {
            // ���� �������� �̹� �ִٸ� ������ ����
            existingItem.counts += itemCountIndex[productCount];
            inventoryUI.InitUI(true);
        }
        else
        {
            // ���ο� �������̶�� ����Ʈ�� �߰�
            buyItem.counts = itemCountIndex[productCount];
            playerInventory.inventory.Add(buyItem);
            inventoryUI.GenerateSlot();
            inventoryUI.InitUI(true);
        }
        if(QuestSystem.Instance.currentQuestType == QuestType.Trade)
            QuestSystem.Instance.QuestProgress(buyItem, itemTotalWeight);
        productCount++;
        buyItem = null;
        BargainClear();
    }
    public void SellProduct()//��ǰ �Ǹ�
    {
        int currentPrice;
        pItem itemToRemove = playerInventory.inventory[currentProductIndex];
        if (bargainSuccess == true)
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

        #region ���� ��� �κ�
        float itemTotalWeight = itemToRemove.weight * itemCountIndex[productCount];
        if (playerInventory.sortWeight[itemToRemove.sort].CurrentWeight <= playerInventory.sortWeight[itemToRemove.sort].MaxWeight)
        {
            playerInventory.sortWeight[itemToRemove.sort].CurrentWeight -= itemTotalWeight;
        }
        else
        {
            if(playerInventory.sortWeight[itemToRemove.sort].CurrentWeight - itemTotalWeight >= playerInventory.sortWeight[itemToRemove.sort].MaxWeight)
            {
                playerInventory.sortWeight[itemToRemove.sort].CurrentWeight -= itemTotalWeight;
                playerInventory.PublicCurrentWeight -= itemTotalWeight;
            }
            else
            {
                playerInventory.sortWeight[itemToRemove.sort].CurrentWeight -= itemTotalWeight;
                float margin = playerInventory.sortWeight[itemToRemove.sort].MaxWeight - playerInventory.sortWeight[itemToRemove.sort].CurrentWeight;
                float over = itemTotalWeight - margin;
                playerInventory.PublicCurrentWeight -= over;
            }
        }
        if (QuestSystem.Instance.currentQuestType == QuestType.Trade)
            QuestSystem.Instance.QuestProgress(itemToRemove, itemTotalWeight);
        #endregion
        productCount++;
        BargainClear();
    }

    #endregion
    #region ���� ����
    public void SetBargainPrice(float initialChance,int bargainValue,int bargainPoint, float bargainPercent)// �ʱ�Ȯ��(%),�������ð���, ��������, ���� ������ %����
    {
        float randomValue = Random.Range(0f, 100f);
        int diff; //���̰��
        int chancePoint; // ������Ȯ���� �󸶸� ���Ұ���
        float totalChance; // ����Ȯ��
        if (Customer.buyOrSell== true)//�����϶�
        {
            diff = itemSO.items[currentProductIndex].price - bargainValue;//�޾ƿ� �������� �����۰� ���̸� ��� ��
        }
        else//�Ǹ��϶�
        {
           if(bargainValue < 0)//�����ȶ��� 0������ ����...
                bargainValue = 0;
            diff = bargainValue - playerInventory.inventory[currentProductIndex].price; // �Ǹ�, �������� �� ���ƾ� �ϹǷ� ������ ���
        }
        chancePoint = diff / bargainPoint; //���̿� ������ ����
        totalChance = initialChance - (chancePoint * bargainPercent); //�ʱ�Ȯ���� ���� Ȯ�������� ����������%������ ���� ���, ���� ���� Ȯ��

        if (totalChance >= randomValue)//��� Ȯ���� random���� ���� ������ ������ ����, ex) Ȯ���� 30�̸� random������ 31�� �������� ����, �������� 29�̸� ����
        {
            bargainPrice = bargainValue;
            Customer.costText.text = "price : " + bargainPrice;
            bargainSuccess = true;
        }
        else
        {
            bargainSuccess = false;
        }
    }
    #endregion
    #region �ʱ�ȭ ���� �� �ڵ� �����
    public void ListClear()//����Ʈ ���ſ�
    {
        productIndex.Clear();
        itemCountIndex.Clear();
        productCount = 0;
    }
    public void BargainClear()//�������� �ʱ�ȭ��
    {
        bargainPrice = 0;
        bargainSuccess = false;
    }
    public void PutInfo(int randCount)//SetUI�������� �ߺ��Ǵºκ� �ڵ����̱�
    {
        itemCountIndex.Add(randCount); //� ����� �߰�
        if (Customer.buyOrSell == true)
        {
            buyItem = new pItem (itemSO.items[currentProductIndex]);
            pItem countItem = playerInventory.inventory.Find(item => item.stuffName == buyItem.stuffName); // ������ϴ� ������ ã��
            if (countItem != null)//���� �ִٸ�
            {
                Customer.playerCountTexts.text = "" + countItem.counts;
            }
            else //���ٸ�
            {
                Customer.playerCountTexts.text = "" + 0; //�÷��̾ ����ִ� �����ؽ�Ʈ �� 0 ����
            }
            Customer.productImages.sprite = itemSO.items[currentProductIndex].image; //��ǰ�� �̹���
            Customer.costText.text = "price : " + itemSO.items[currentProductIndex].price;//; ���� �ؽ�Ʈ�� �ݿ�
        }
        else
        {
            Customer.playerCountTexts.text = "" + playerInventory.inventory[currentProductIndex].counts;
            Customer.productImages.sprite = playerInventory.inventory[currentProductIndex].image;
            Customer.costText.text = "price : " + playerInventory.inventory[currentProductIndex].price;
        }
        CusProductCountSet();
        Customer.productTexts.text = "" + itemCountIndex[productCount];//���� �ؽ�Ʈ�� �ݿ�
    }
    #endregion
    private void CusProductCountSet()
    {
        switch (Customer.randcusnum)
        {
            case 1:
                if (Customer.buyOrSell == true && buyItem.sort == ItemSorts.food)
                {
                    itemCountIndex[productCount] = itemCountIndex[productCount] + 5;
                }
                else if (Customer.buyOrSell == false && playerInventory.inventory[currentProductIndex].sort == ItemSorts.food)//��������. ��¥�� buyorsell�� �Ǹŷ� �������̻� playerItem���� ������ �������� �ϳ��� �����Ƿ�.
                {
                    itemCountIndex[productCount] = itemCountIndex[productCount] + 5;
                    if (itemCountIndex[productCount] >= playerInventory.inventory[currentProductIndex].counts)
                        itemCountIndex[productCount] = playerInventory.inventory[currentProductIndex].counts;
                }
                break;
        }
    }
    private float salePoint(ItemSorts sorts)
    {
        float salePercent = 100.0f;
      
        switch(sorts)
        {
            case ItemSorts.food:
                salePercent -=Player.Instance.foodFame.sale;
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
}
