using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialClick : TutorialBase
{
    [SerializeField] private GameObject ClickTrigger;

    public override void Enter()
    {
        ClickTrigger.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if(hit.collider != null)
            {
                if(hit.collider.gameObject == ClickTrigger)
                {
                    controller.SetNextTutorial();
                }
            }
        }
    }

    public override void Exit()
    {
        ClickTrigger.SetActive(false);
    }
}
