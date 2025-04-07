using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    [SerializeField] private List<DialogSystem> dialogs;

    [SerializeField] private GameObject FadeUI;
    [SerializeField] private float FadeTime = 1f;

    [SerializeField] private GameObject ButtonGruop;

    public void TestDialog() // ��ư�� ������ ����
    {
        if (dialogs.Count > 0)
        {
            StartCoroutine(DialogPlay());
        }
    }

    private IEnumerator DialogPlay()
    {
        for (int i = 0; i < dialogs.Count; i++)
        {
            yield return new WaitUntil(() => dialogs[i].UpdateDialog());
            if (i != dialogs.Count - 1)
            {
                GameObject FadeInOut = Instantiate(FadeUI);
                yield return new WaitForSeconds(FadeTime);
            }
            dialogs[i].SetActiveFalseUI();
            Debug.Log("���� ���");
        }

        ButtonGruop.SetActive(true);
    }
}