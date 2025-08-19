using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    public abstract void Enter(TutorialController controller);
    public abstract void Execute(TutorialController controller);
    public abstract void Exit(TutorialController controller);
}
