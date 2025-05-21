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
        if (int.TryParse(repayField.text, out repayValue))//�Ľ�
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
        }
        else
        {
            renewUI();
            Debug.Log("Error");
            yield break;
            //���� �̿� �ٸ� ���Ͻ� ���ư�����
        }
    }

    void renewUI()
    {
        repayField.text = "";
        
        repayUIButton.SetActive(true);
    }

}
