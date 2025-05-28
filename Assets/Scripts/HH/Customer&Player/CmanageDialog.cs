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
   public void InitSign()//��ȣ �ʱ�ȭ
    {
        noDialog = true;
        changeState = false;
        tradefail = false;
    }
   public void DialogBranch(int customnum)// �б��� ����
    {
        if (customer.cState.Value == CustomerState.Start)
        {
            switch (customnum)
            {
                case 0:
                    SetDialog("�������ŷ����λ�");
                    break;
                case 1:
                    SetDialog("���ҶҰŷ������");
                    break;
                case 4:
                    SetDialog("�����Ѱŷ����λ�");
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
                    SetDialog("�������ŷ����λ�");
                    break;
                case 1 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("���ҶҰŷ�����");
                    break;
                case 1:
                    SetDialog("ȭ���ۺ��λ�");
                    break;
                case 4:
                    SetDialog("�����Ѱŷ����λ�");
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
                    SetDialog("�������̸����");
                    break;
                case 4 when !ItemManager.Instance.bargainSuccess && tradefail:
                    SetDialog("������������");
                    break;
                default:
                    InitSign();
                    break;
            }
        }
    }
 
  
    

}
