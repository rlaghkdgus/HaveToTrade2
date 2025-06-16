using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOnEvent : MonoBehaviour
{
    public bool OnTrade = false;

    private void OnEnable()
    {
        OnTrade = true;
    }
}
