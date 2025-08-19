using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialInput : TutorialBase
{
    [SerializeField] private TMP_InputField inputfield;
    [SerializeField] private RectTransform bargainUI;
    [SerializeField] private int requireValue;
    [SerializeField] private TextMeshProUGUI failText;

    [SerializeField] private Customer customer;

    public override void Enter(TutorialController controller)
    {
        controller.highlighter?.gameObject.SetActive(true);
        controller.highlighter?.Highlight(bargainUI);
    }

    public void InputValueCheck(TutorialController controller)
    {
        int value = int.Parse(inputfield.text);

        if(value == requireValue)
        {
            controller.SetNextTutorial();
        }
        else
        {
            if (failText) failText.text = "다시 입력해볼까?";
        }
    }

    public override void Execute(TutorialController controller)
    {
        
    }

    public override void Exit(TutorialController controller)
    {
        controller.highlighter?.gameObject.SetActive(false);
        if (failText) failText.text = "";
    }
}
