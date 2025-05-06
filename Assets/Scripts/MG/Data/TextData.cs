using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextData
{
    public string Type; // 텍스트 종류
    public int SpeakerIndex; // 대화 분기
    public string Name; // 현재 화자 이름
    public string Dialog; // 대화 내용
}
