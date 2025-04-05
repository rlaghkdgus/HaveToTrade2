using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RockEvent : MonoBehaviour
{
    [SerializeField]protected int count;
    public GameObject rock;
    public GameObject SysOnOff;
    public TMP_Text text;

    [SerializeField] private Travel travel;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            count++;
            text.text = "" + count;
        }
        if(count == 10)
        {
            count = 0;
            SysOnOff.SetActive(false);
            travel.rState.Value = RoadEventState.Idle;
            text.text = "";
        }
    }
}
