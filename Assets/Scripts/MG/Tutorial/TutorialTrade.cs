using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrade : TutorialBase
{
    [SerializeField] private TutorialOnEvent T_event;
    public bool fixItem = false;

    public override void Enter()
    {
        fixItem = true;
    }

    public override void Execute(TutorialController controller)
    {
        if (T_event.OnTrade)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        
    }
}
