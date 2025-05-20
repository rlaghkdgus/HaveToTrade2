using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmanageDialog : MonoBehaviour
{
    [SerializeField] Customer customer;
    [SerializeField] List<DialogSystem> dialogSystem;
    [SerializeField] private GameObject FadeUI;
    [SerializeField] private float FadeTime = 1f;
    private bool changeState = false;
    public bool noDialog = true;
    public bool tradefail = false;
    CustomerState changeCState;

    private void SetDialog(string dialogName)
    {
        noDialog = false;
        dialogSystem[dialogSystem.Count - 1].selectedDialogName = dialogName;
        dialogSystem[dialogSystem.Count - 1].DialogListLoading();
    }
   public void InitSign()//신호 초기화
    {
        noDialog = true;
        changeState = false;
        tradefail = false;
    }
   public void DialogBranch(int customnum)// 분기점 관리
    {
        if (customer.cState.Value == CustomerState.Start)
        {
            switch (customnum)
            {
                case 1:
                    SetDialog("TextEX3");
                    break;
                default:
                    InitSign();
                    break;
            }
        }
        else if (customer.cState.Value == CustomerState.End)
        {
            switch (customnum)
            {
                case 1 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("CustomerReject1");
                    break;
                case 1:
                    SetDialog("Customer2");
                    break;
                default:
                    InitSign();
                    break;
            }
        }
        else if (customer.cState.Value == CustomerState.Bargain)
        {
            switch (customnum)
            {
                case 4 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("CustomerReject2");
                    break;
                default:
                    InitSign();
                    break;
            }
        }
    }
 
  
    

}
