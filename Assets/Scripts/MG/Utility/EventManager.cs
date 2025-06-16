using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public static event Action<bool> OnGenerateTown;

    public static event Action<ItemSorts> ChangeFame;

    public static event Action LoanClear;

    public static event Action TownMoveMax;

    public static event Action TownBuildingClick;

    public static event Action TownBuildingUIOff;

    public static void OnGenerateTownCall_Cloud(bool cloud)
    {
        OnGenerateTown?.Invoke(cloud);
        Debug.Log("타운생성 이벤트 호출");
    }

    public static void OnChangeFameCall_Tier(ItemSorts sort)
    {
        ChangeFame?.Invoke(sort);
        Debug.Log("명성 이벤트 호출");
    }

    public static void OnLoanClearCall()
    {
        LoanClear?.Invoke();
        Debug.Log("빚 해결 엔딩 호출");
    }

    public static void OnTownMoveMaxCall()
    {
        TownMoveMax?.Invoke();
        Debug.Log("타운 이동 제한 엔딩 호출");
    }

    public static void OnTownBuildingClick()
    {
        TownBuildingClick?.Invoke();
    }

    public static void OnTownBuildingUIOff()
    {
        TownBuildingUIOff?.Invoke();
    }
}
