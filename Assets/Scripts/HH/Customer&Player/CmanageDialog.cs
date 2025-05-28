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
                case 0:
                    SetDialog("아저씨거래전인사");
                    break;
                case 1:
                    SetDialog("무뚝뚝거래전잡담");
                    break;
                case 4:
                    SetDialog("유쾌한거래전인사");
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
                case 0:
                    SetDialog("아저씨거래후인사");
                    break;
                case 1 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("무뚝뚝거래거절");
                    break;
                case 1:
                    SetDialog("화난작별인사");
                    break;
                case 4:
                    SetDialog("유쾌한거래후인사");
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
                case 0 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("아이템이름출력");
                    break;
                case 4 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("유쾌한재흥정");
                    break;
                default:
                    InitSign();
                    break;
            }
        }
    }
 
  
    

}
