using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStay : TutorialBase
{
    [SerializeField] private float WaitTime;
    private float timeCount;

    public override void Enter(TutorialController controller)
    {
        timeCount = 0;
    }

    public override void Execute(TutorialController controller)
    {
        timeCount += Time.deltaTime;
        if (timeCount >= WaitTime)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit(TutorialController controller)
    {
        timeCount = 0;
    }
}
