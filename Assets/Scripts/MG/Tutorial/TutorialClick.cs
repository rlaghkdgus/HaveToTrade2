using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialClick : TutorialBase
{
    [SerializeField] private T_Clickable target;
    private RectTransform rect;

    public string targetTag;
    [SerializeField] private bool clicked = false;

    private void OnEnable()
    {
        EventManager.TownBuildingClick += BuildingClick;
    }

    private void OnDisable()
    {
        EventManager.TownBuildingClick -= BuildingClick;
    }

    public override void Enter(TutorialController controller)
    {
        if(target == null)
        {
            GameObject tagtarget = GameObject.FindGameObjectWithTag(targetTag);
            target = tagtarget?.GetComponent<T_Clickable>();

            if(target == null)
            {
                Debug.Log("target 없음");
                return;
            }

            Debug.Log("target 찾음");
        }

        rect = target.GetComponent<RectTransform>();

        if (rect != null)
        {
            controller.highlighter?.Highlight(rect);
            Debug.Log("UI 인지");
        }
        else
        {
            controller.highlighter?.Highlight(target.gameObject, Camera.main);
            Debug.Log("Object 인지");
        }

        clicked = false;
        target.onClicked.AddListener(() => OnTargetClicked(controller));
    }

    public override void Execute(TutorialController controller)
    {
        if (clicked)
        {
            controller.SetNextTutorial();
        }
    }

    public void BuildingClick()
    {
        clicked = true;
    }

    private void OnTargetClicked(TutorialController controller)
    {
        Debug.Log("Object 클릭");
        controller.SetNextTutorial();
    }

    public override void Exit(TutorialController controller)
    {
        if(target != null)
        {
            target.onClicked.RemoveAllListeners();
        }

        clicked = false;
        controller.highlighter?.Hide();
    }
}
