using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
/*
 * �ŷ� ���� �� ���� ��, �ŷ� ��ǰ�� ���� �� ���ڸ� ���� ����
 * �մ� �ൿ���� ��� : ����(Start) -> ���(Idle) -> �����۸Ŵ������� ������ ���������� UI����-> ����(����)-> ���� or �Ǹ� -> ����(End) -> ����, �մԼ� ��ŭ �ݺ�
 */


/*
 * �ؾ��� �� : �����۸Ŵ��� ������ �ŷ��ý���(�մԺ� �ŷ� �ɼ� ����) ���� �ε��� ����(��)
 * �ŷ��� �ǹ� ��ư Ŭ�� ����
 * ������ ��� �ε����� SetCustomer���� ���� randcusnum�� �ƴ� Npc ������ �ε����� ó���� �� 
 */
[System.Serializable]
public class customerPrefab
{
    public int customerNum;
    public string customerName;
    public string customerDescription;
    public GameObject[] cusPrefab;
    public float cusBargainChance;
    public int cusProductCount;
}
public class Customer : MonoBehaviour
{
    [Header("�մ� ��ȭ��")]
    [SerializeField] List <DialogSystem> customerDialog;
    [SerializeField] CmanageDialog newDialogSys;
    [SerializeField] private GameObject FadeUI;
    [SerializeField] private float FadeTime = 1f;
    [Header("�ӽ� UI ����")]
    [SerializeField] GameObject blockButtonClick;
    [Header("�մԼ�, ����� ���۽� ���� ����")]
    public int cusCount;
    [Header("�մ� ���� �ɼ�")]
    public float speed; //�մ� �̵��ӵ�
    public float tradeDelay; // �մ� �ŷ� ������
    public float rejectDelay; // �մ� ���� �� ���� �ŷ� ������
    public float fadeDuration; // ���̵� �ƿ� ���� �ð�
    [Header("���� ���� �ɼ�")]
    public float initialChance; // ������ �ʱ� Ȯ��
    public int bargainPoint; // ����Ȯ�� ����
    public float bargainChance; // �ش� ������ ���������� ����Ȯ���� ��%�� ��������
    [Header("��� ������ �ŷ��Ұ���")]
    public int tradeSortCount; // �մ��� ��� ������ �ŷ��Ұ���
    [Header("���� ��ȣ(Ȯ�ο�)")]
    public int preBargainValue;
    public bool reBargain = false;
    [Header("�մ� ������(�մ� �� ��� �� ����),���� �� ���� ��ġ ����")]
    public List<customerPrefab> currentCusList;
    public List<customerPrefab> cusList;
    public List<customerPrefab> SmokianCusList;
    public List<customerPrefab> GoldBenCusList;
    public List<Transform> customerTransform;// ����, �ŷ���ġ, ����


    [Header("�մ� �ŷ�â")]
    public GameObject CustomerUI;
    public GameObject BuyUI;
    public GameObject SellUI;
    [Header("���� â")]
    public GameObject BargainUI;
    [SerializeField] private TMP_Text nonNegoText;
    [SerializeField] private float N_TextDuration = 1.5f;
    [SerializeField] private TMP_Text cText;
    [SerializeField] private TMP_Text pTxt;
    [SerializeField] private TMP_Text pcTxt; // �÷��̾ ����ִ� ��ǰ ���� ī��Ʈ
    [SerializeField] private TMP_InputField bargainField;
    [SerializeField] private Image pImg;
    [Header("������ On/Off ������Ʈ")]
    [SerializeField] private GameObject bargainButton;
    [SerializeField] private GameObject rejectButton;
    [Header("���� ��ư")]
    [SerializeField] GameObject GoTownButton;

    [Header("�մ� ��ȭ")]
    public TMP_Text talkText; // ��ȭ�ؽ�Ʈ
    [SerializeField] Transform textTransform;//��ȭ������ġ
    [SerializeField] float typingDelay;//Ÿ���� ����
    [Header("�ŷ� �� ��ư ����")]
    [SerializeField] GameObject buttonEdit;

    public Data<CustomerState> cState = new Data<CustomerState>();//���º� �̺�Ʈ
    private GameObject newCustomer;
    public int bargainValue;
    public int randcusnum = 0;
    public TMP_Text productTexts;
    public Image productImages;
    public TMP_Text playerCountTexts;// �÷��̾ ����ִ� ��ǰ ���� ī��Ʈ
    public TMP_Text costText;//���� �ؽ�Ʈ, product�� price�� �ձ��ڰ� ���ļ�..
    public bool buyOrSell;//���϶�����, �����϶��Ǹ�

    bool OnTrade = true;
   [SerializeField] GameObject tradeButton;
    [Header("Dialog��")]
    public bool Intrade; 
    public void tradeOn()
    {
        OnTrade = true;
        tradeButton.SetActive(true);
    }
    void Start()
    {
        productTexts = pTxt;
        productImages = pImg;
        playerCountTexts = pcTxt;
        costText = cText;
        cusCount = Random.Range(3, 6);
        Player.Instance.RenewMoney();
        //cState.Value = CustomerState.Start;
    }
    private void Awake()//���º�ȭ ���� ����
    {
        cState.onChange += SetCustomer;
        cState.onChange += CustomerSetItem;
        cState.onChange += CustomerSetUI;
        cState.onChange += BuyItem;
        cState.onChange += SellItem;
        cState.onChange += RejectItem;
        cState.onChange += BargainItem;
        cState.onChange += CustomerExit;
    }
    #region ��ư���� �ൿ���� ��ȭ
    public void CustomerStart()
    {
        if (!OnTrade)
            return;
        SoundManager.Instance.SFXplay(SoundType.UI_Button);
        cState.Value = CustomerState.Start;
    }
    public void CustomerBuy()
    {
        SoundManager.Instance.SFXplay(SoundType.Buy);
        cState.Value = CustomerState.Buy;
    }
    public void CustomerSell()
    {
        SoundManager.Instance.SFXplay(SoundType.Sell);
        cState.Value = CustomerState.Sell;
    }
    public void CustomerReject()
    {
        SoundManager.Instance.SFXplay(SoundType.Reject);
        cState.Value = CustomerState.Reject;
    }
    public void CustomerBargain()
    {
        SoundManager.Instance.SFXplay(SoundType.Bargain);
        cState.Value = CustomerState.Bargain;
    }
    #endregion
    #region �մ� ������ ����
    List<customerPrefab> SetRegionCustomer()//�������� �ٸ� �մԵ��� ����Ʈ ����
    {
        List<customerPrefab> list = new List<customerPrefab>();
        list.AddRange(cusList);// �⺻�մ� ����Ʈ �߰�
        switch (TownManager.Instance.curTown.TownType)
        {
            case VillageType.GoldBen:
                list.AddRange(GoldBenCusList);
                break;
            case VillageType.Smokian:
                list.AddRange(SmokianCusList);
                break;
            default:
                break;
        }
        return list;  
    }
    #endregion
    #region �մ� �ൿ����
    private void SetCustomer(CustomerState _cState)
    {
        if(_cState == CustomerState.Start)//�մ� ��ü ������ �̵�
        {
            blockButtonClick.SetActive(true);
            OnTrade = false;
            Intrade = true;
            if(cusCount <= 0)
            {
                cusCount = Random.Range(3, 6);
            }
            currentCusList = SetRegionCustomer();
            randcusnum = Random.Range(0,currentCusList.Count);
            randcusnum = 0; //�׽�Ʈ��, �ּ��ؾ���.
            int randcusprefab = Random.Range(0, currentCusList[randcusnum].cusPrefab.Length);
            newCustomer = Instantiate(currentCusList[randcusnum].cusPrefab[randcusprefab], customerTransform[0]);
            //CusBargainPointSet(currentCusList[randcusnum].customerNum);
            
            StartCoroutine(MoveCustomerToPosition(newCustomer, customerTransform[1].position));
        }
    }
    private void CustomerSetItem(CustomerState _cState)
    {
        if(_cState == CustomerState.ItemSet)//�մԴ��� �÷��̾�� ����/�Ǹ� �� ������ ���� 
        {
            reBargain = false;
            int randsort = Random.Range(1, tradeSortCount+1);
            BuyOrSell();//���� or �Ǹ� �������� ������
            if (buyOrSell == true)
                ItemManager.Instance.RandomSetItem(randsort);
            else
                ItemManager.Instance.RandomSetItemSell(randsort);
            cState.Value = CustomerState.SetUI;
        }
    }
    private void CustomerSetUI(CustomerState _cState)
    {
        if(_cState == CustomerState.SetUI)//UI�� ǥ�� �� ItemManager�� productCount�� ���� �Ǵ�
        {
            if (buyOrSell == true)
            {
                ItemManager.Instance.SetUI();
                UIon();
                BuyUI.SetActive(true);
            }
            else
            {
                ItemManager.Instance.SetSellUI();
                UIon();
                SellUI.SetActive(true);
            }

        }
    }
    private void BuyItem(CustomerState _cState)
    {
        if(_cState == CustomerState.Buy)//����
        {
            StartCoroutine(DelayBuy());
        }
    }
    private void SellItem(CustomerState _cState)
    {
        if(_cState == CustomerState.Sell)//�Ǹ�
        {
            StartCoroutine(DelaySell());
        }
    }
    private void RejectItem(CustomerState _cState)
    {
        if(_cState == CustomerState.Reject)//����
        {
            StartCoroutine(DelayReject());
        }
    }
    private void BargainItem(CustomerState _cState)
    {
        if(_cState == CustomerState.Bargain)
        {
            CustomerUI.SetActive(false);
            BargainUI.SetActive(true);
        }
    }
 
    private void CustomerExit(CustomerState _cState)
    {
        if(_cState == CustomerState.End)//����
        {
            initialChance = 100f;
            //StartCoroutine(MoveAndFadeOutCustomer(newCustomer, customerTransform[2].position, fadeDuration)); //���� �� ���̵� �ƿ�
            cusCount--;
            StartCoroutine(TradeEnd());
        }
    }
    #endregion
    #region �մ� ���� ���
    public void BuyOrSell()//���� Ȥ�� �Ǹ� ��ȣ(����)
    {
        if (ItemManager.Instance.playerInventory.inventory.Count == 0)
        {
            buyOrSell = true;
            return;
        }
        if (Random.value > 0.5)
            buyOrSell = true;
            //buyOrSell = false;
        else
            buyOrSell = false;

    }
    
    public void BargainStart()//��ư���� ��������
    {
        StartCoroutine("BargainCycle");
    }
    IEnumerator BargainCycle()//��������
    {
        Debug.Log("a");
        if (int.TryParse(bargainField.text, out bargainValue))//�Ľ�
        {
            if (reBargain == true && buyOrSell && preBargainValue > bargainValue)
            {
                nonNegoTextTween();
                yield break;
            }
            else if (reBargain == true && !buyOrSell && bargainValue > preBargainValue)
            {
                nonNegoTextTween();
                yield break;
            }
            ItemManager.Instance.SetBargainPrice(initialChance, bargainValue, bargainPoint, bargainChance);
            BargainUI.SetActive(false);
            bargainField.text = "";
            yield return YieldCache.WaitForSeconds(1.0f);
            if (ItemManager.Instance.bargainSuccess == true)//���������� UI����
            {
                bargainButton.SetActive(false);
                rejectButton.SetActive(false);
                CustomerUI.SetActive(true);
                if (reBargain)
                    reBargain = false;
            }
            else
            {
                preBargainValue = bargainValue;
                reBargain = CusBargainReject(currentCusList[randcusnum].customerNum);
                if (reBargain)
                    yield break;
                yield return null;
                bargainButton.SetActive(false);
                CustomerUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Error");
            yield return null;
            //���� �̿� �ٸ� ���Ͻ� ���ư�����
        }
    }
    private void nonNegoTextTween()// ���� ���н� �ؽ�Ʈ ȿ��
    {
        if (!DOTween.IsTweening(nonNegoText))
        {
            nonNegoText.DOFade(1f, N_TextDuration)
    .SetEase(Ease.Linear)
    .OnComplete(() =>
    {
        nonNegoText.DOFade(0f, N_TextDuration)
            .SetEase(Ease.Linear);
    });
        }
    }
  
    IEnumerator MoveCustomerToPosition(GameObject customer, Vector3 targetPosition)//�մ� ����
    {
        while (customer.transform.position != targetPosition)
        {
            customer.transform.position = Vector2.MoveTowards(customer.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        yield return DialogPlay();
        cState.Value = CustomerState.ItemSet;
    }
    IEnumerator MoveAndFadeOutCustomer(GameObject customer, Vector3 targetPosition, float duration)//�մ�����
    {
        SpriteRenderer spriteRenderer = customer.GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        float elapsed = 0f;

        Vector3 startPosition = customer.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // �̵� ó��
            float t = elapsed / duration;
            customer.transform.position = Vector2.Lerp(startPosition, targetPosition, t);

            // ���̵� �ƿ� ó��
            float alpha = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        // ������ ��ġ�� ���� ����
        customer.transform.position = targetPosition;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Destroy(customer); // ���̵� �ƿ� �� ������Ʈ ����
    }
    IEnumerator DelayBuy()//��������
    {
        ItemManager.Instance.BuyProduct();
        CustomerUI.SetActive(false);
        BuyUI.SetActive(false);
        Player.Instance.RenewMoney();
        //yield return Typing("wow!");
        yield return YieldCache.WaitForSeconds(tradeDelay);
        if (ItemManager.Instance.productCount == ItemManager.Instance.productIndex.Count)
            cState.Value = CustomerState.End;
        else
        cState.Value = CustomerState.SetUI;
    }
    IEnumerator DelaySell()//�Ǹ�����
    {
        ItemManager.Instance.SellProduct();
        CustomerUI.SetActive(false);
        SellUI.SetActive(false);
        Player.Instance.RenewMoney();
        yield return YieldCache.WaitForSecondsRealTime(tradeDelay);
        if (ItemManager.Instance.productCount == ItemManager.Instance.productIndex.Count)
            cState.Value = CustomerState.End;
        else
            cState.Value = CustomerState.SetUI;
    }
    IEnumerator TradeEnd()//�ŷ� ����
    {
        reBargain = false;
        ItemManager.Instance.ListClear();
        yield return DialogPlay();
                
        yield return MoveAndFadeOutCustomer(newCustomer, customerTransform[2].position, fadeDuration);
        yield return YieldCache.WaitForSeconds(fadeDuration * 1.5f);
        if (cusCount <= 0)
        {
            currentCusList.Clear();
            cState.Value = CustomerState.Idle;
            Intrade = false;
            tradeButton.SetActive(false);
            buttonEdit.SetActive(true);
            blockButtonClick.SetActive(false);
        }
        else
        {
            cState.Value = CustomerState.Start;
        }
    }
    IEnumerator DelayReject()//���� ������
    {
        ItemManager.Instance.productCount++;//������ ������ǰ���� �ѱ�
        CustomerUI.SetActive(false);
        if(buyOrSell == true)
            BuyUI.SetActive(false);
        else
            SellUI.SetActive(false);
        yield return YieldCache.WaitForSeconds(rejectDelay);
        if (ItemManager.Instance.productCount == ItemManager.Instance.productIndex.Count)
            cState.Value = CustomerState.End;
        else
            cState.Value = CustomerState.SetUI;
    }
    #region �մ� ��� ȿ��
    IEnumerator Typing(string talk)
    {
        talkText.text = null;
        for(int i = 0; i< talk.Length; i++)
        {
            talkText.text += talk[i];
            yield return YieldCache.WaitForSeconds(typingDelay);
        }
        talkText.text = null;
        
    }
    private IEnumerator DialogPlay()
    {
        newDialogSys.DialogBranch(currentCusList[randcusnum].customerNum);
        if (newDialogSys.noDialog)
            yield break;
        for (int i = 0; i < customerDialog.Count; i++)
        {
            yield return new WaitUntil(() => customerDialog[i].UpdateDialog());
            if (i != customerDialog.Count - 1)
            {
                GameObject FadeInOut = Instantiate(FadeUI);
                yield return YieldCache.WaitForSeconds(FadeTime);
            }
            customerDialog[i].SetActiveFalseUI();

        }
    }
    #endregion
    private void UIon()// UI �ϰ� on
    {
        rejectButton.SetActive(true);
        bargainButton.SetActive(true);
        CustomerUI.SetActive(true);
    }
    #endregion
    #region �մ� ���� ���

    private void CusBargainPointSet(int customnum)
    {
        initialChance += currentCusList[customnum].cusBargainChance;
    }
    private IEnumerator StateChangeDialog(CustomerState _cstate)
    {
        yield return DialogPlay();
        cState.Value = _cstate;
    }
    
    private bool CusBargainReject(int customnum)
    {
        switch(customnum)
        {
            case 1:
                if(ItemManager.Instance.bargainSuccess == false)
                {
                    StopCoroutine("BargainCycle");
                    newDialogSys.tradefail = true;
                    cState.Value = CustomerState.End;
                    return true;
                }
                break;
            case 0:
            case 4:
                if(ItemManager.Instance.bargainSuccess == false)
                {
                    if (reBargain)
                        return false;
                    newDialogSys.tradefail = true;
                    StartCoroutine(StateChangeDialog(CustomerState.Bargain));
                    return true;
                }
                break;
        }
        return false;
    }
    #endregion
}
