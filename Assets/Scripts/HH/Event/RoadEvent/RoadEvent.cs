using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadEvent : MonoBehaviour
{
    [SerializeField] Travel travel;
    public Data<RoadEventState> rState = new Data<RoadEventState>();
    [Header("이벤트 발동용")]
    [SerializeField] GameObject thiefEvent;
    [SerializeField] GameObject rockEventOnOff;
    [SerializeField] RockEvent rockEvent;
    [SerializeField] GameObject shootingEvent;
    [SerializeField] Transform eventRocation;
    [Header("여행이벤트 확률")]
    [SerializeField] float idle;
    [SerializeField] float thief;
    [SerializeField] float rock;
    [SerializeField] float Shooting;
    private void Awake()
    {
        rState.onChange += SetIdle;
        rState.onChange += SetThief;
        rState.onChange += SetRock;
        rState.onChange += SetShooting;
    }

    public void RoadEventSet()
    {
        float Randnum = Random.Range(0, 100f);

       // if (Randnum >= idle && Randnum < thief) rState.Value = RoadEventState.Idle;
       // else if (Randnum >= thief && Randnum < rock) rState.Value = RoadEventState.Thief;
       // else if (Randnum >= rock && Randnum <= 100f) rState.Value = RoadEventState.Rock;

        rState.Value = RoadEventState.Shooting; // 테스트용

    }
    #region 이동 이벤트
    private void SetIdle(RoadEventState _rState)
    {
        if (_rState == RoadEventState.Idle)
        {
            if (!travel.OnMove)
                travel.OnMove = true;
        }
    }
    private void SetThief(RoadEventState _rState)
    {
        if (_rState == RoadEventState.Thief)
        {

        }
    }
    private void SetRock(RoadEventState _rState)
    {
        if (_rState == RoadEventState.Rock)
        {
            rockEventOnOff.SetActive(true);
        }
    }
    private void SetShooting(RoadEventState _rState)
    {
        if (_rState == RoadEventState.Shooting)
        {
            Vector3 position = new Vector3(0, 0, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Instantiate(shootingEvent, position, rotation,eventRocation);
        }
    }

    #endregion
}
