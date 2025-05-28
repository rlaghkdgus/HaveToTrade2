using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DialogData : ScriptableObject
{
    // public List<TextData> 부분까지는 고정
    // 뒤에 변수 이름만 각 시트별 이름과 동일하게 설정
    public List<TextData> TextEX; 
	public List<TextData> TextEX2;
    public List<TextData> 무뚝뚝거래전잡담;
    public List<TextData> 화난작별인사;
    public List<TextData> 무뚝뚝거래거절;
    public List<TextData> 유쾌한재흥정;
    public List<TextData> 아이템이름출력;
    public List<TextData> 유쾌한거래전인사;
    public List<TextData> 유쾌한거래후인사;
    public List<TextData> 아저씨거래전인사;
    public List<TextData> 아저씨거래후인사;
}
