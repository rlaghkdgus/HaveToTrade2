using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Bank : MonoBehaviour
{

    public int loan;
    [SerializeField] TMP_Text loanText;
    [SerializeField] TMP_InputField repayField;
    [SerializeField] GameObject repayUIButton;
    int repayValue;
    [SerializeField] GameObject repayPopUp;
    private void Start()
    {
        loanText.text = "" + loan;
    }
    public void LoanPay()
    {
        StartCoroutine(LoanRepayment());
    }
    IEnumerator LoanRepayment()
    {
        if (int.TryParse(repayField.text, out repayValue))//파싱
        {
            
            if(repayValue <= 0 || (Player.Instance.money - repayValue) < 0)
            {
                renewUI();
                yield break;
            }
            
            Player.Instance.money -= repayValue;
            loan -= repayValue;
            if (loan < 0)
            {
                loan = 0;
            }
            loanText.text = "" + loan;
            Player.Instance.RenewMoney();
            renewUI();
            StartCoroutine(RePayPopUp());
        }
        else
        {
            renewUI();
            Debug.Log("Error");
            yield break;
            //정수 이외 다른 값일시 돌아가도록
        }
    }
    IEnumerator RePayPopUp()
    {
        yield return YieldCache.WaitForSeconds(0.1f);
        GameObject popup = Instantiate(repayPopUp, GameObject.FindGameObjectWithTag("Canvas").transform);
        yield return YieldCache.WaitForSeconds(1f);
        Destroy(popup);
    }
    
    void renewUI()
    {
        repayField.text = "";
        
        repayUIButton.SetActive(true);
    }

}
