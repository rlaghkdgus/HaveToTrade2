using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogSystem))]  
public class TutorialDialog : TutorialBase
{
    private DialogSystem dialogSystem;

    public override void Enter(TutorialController controller)
    {
        dialogSystem = GetComponent<DialogSystem>();
    }

    public override void Execute(TutorialController controller)
    {
        bool isCompleted = dialogSystem.UpdateDialog();

        if(isCompleted == true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit(TutorialController contorller)
    {

    }
}
