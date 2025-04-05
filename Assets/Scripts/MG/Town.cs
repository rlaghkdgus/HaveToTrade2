using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    [SerializeField] private TownDB DB; // �� ��ư�� ��� �ִ� ���� ����

    public delegate void TownSelected(TownDB seletedTown);
    public static event TownSelected OnTownSelected; // ��ư ������ ���� �̺�Ʈ

    public void OnButtonClicked()
    {
        if(OnTownSelected != null)
        {
            OnTownSelected(DB);
        }
    }
}
