using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� ������ ����, �ַ� ���������Ͽ� ���

public enum ItemSorts
{
    food,
    accesory,
    furniture,
    clothes,
    pFood
}
public enum CustomerState
{
    Idle,
    Start,
    ItemSet,
    SetUI,
    Buy,
    Sell,
    Reject,
    Bargain,
    BargainReject,
    End,
}
public enum RoadEventState
{
    Idle,
    Thief,
    Rock,
    
}
public enum QuestType
{
    Idle,
    Trade,
    Delivery
}
public enum VillageType
{
    Idle,
    GreStar,
    Smokian
}

