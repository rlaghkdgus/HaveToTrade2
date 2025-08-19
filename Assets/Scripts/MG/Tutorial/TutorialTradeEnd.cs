using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTradeEnd : TutorialBase
{
    public bool isEnd = false;

    public override void Enter(TutorialController controller)
    {
        
    }

    public override void Execute(TutorialController controller)
    {
        if (isEnd)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit(TutorialController contorller)
    {
        
    }
}
