using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FameUIManage : MonoBehaviour
{
    public Slider foodFameBar;
    public Slider accesoryFameBar;
    public Slider furnFameBar;
    public Slider clothFameBar;
    public Slider pFoodFameBar;
    public TMP_Text FameInfoText;
    public GameObject FameInfoButton;
    private int InfoPreIndex;
    [SerializeField] private List<Button> fameSkillButton;
   [SerializeField] private List<Button> fameDescriptionButton;
    private void Awake()
    {
           if (accesoryFameBar != null)
                accesoryFameBar.value = Player.Instance.accesoryFame.fame / 15f;
            if (foodFameBar != null)
                foodFameBar.value = Player.Instance.foodFame.fame / 15f;
            if (pFoodFameBar != null)
                pFoodFameBar.value = Player.Instance.pFoodFame.fame / 15f;
            if (furnFameBar != null)
                furnFameBar.value = Player.Instance.furnFame.fame / 15f;
            if (clothFameBar != null)
                clothFameBar.value = Player.Instance.clothFame.fame / 15f;
        
    }
    private void Start()
    {
        for (int i = 0; i < fameSkillButton.Count; i++)
        {
            int index = i;
            
            if (fameSkillButton != null)
            {
                fameSkillButton[i].onClick.AddListener(() => Player.Instance.SkillUp(index));
            }
            
        }
        for (int i = 0; i < fameDescriptionButton.Count; i++)
        {
            int index = i;
            if (fameDescriptionButton[i] != null)
            {
                fameDescriptionButton[i].onClick.AddListener(() => FameInfo(index));
            }
        }
    }
    public void FameInfo(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                FameInfoText.text = "������ " + Player.Instance.tier[0] + "% ����";
                break;
            case 1:
                FameInfoText.text = "������ " + Player.Instance.tier[1] + " % ����";
                break;
            case 2:
                FameInfoText.text = "������ " + Player.Instance.tier[2] + "% ����";
                break;
            case 3:
                FameInfoText.text = "������ " + Player.Instance.tier[3] + "% ����";
                break;
        }

        if (FameInfoButton.activeSelf == false)
        {
            FameInfoButton.SetActive(true);
        }
        else
        {
            if (InfoPreIndex != buttonIndex)
            {
                InfoPreIndex = buttonIndex;
                return;
            }
            else
                InfoPreIndex = buttonIndex;
            FameInfoButton.SetActive(false);
        }

    }

}
