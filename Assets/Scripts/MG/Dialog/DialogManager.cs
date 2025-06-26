using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("데이터 소스")]
    [SerializeField] private DialogData dialogDataBase;

    [SerializeField] private string dialogDataBaseAddress = "Assets/DialogData/SO/DialogData.asset";

    [SerializeField] string chapterName;

    [Header("UI 참조")]
    [SerializeField] private GameObject nameBox;

    [SerializeField] private GameObject lineBox;

    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI lineText;

    public Image backgroundImage;

    [SerializeField] private Image fadeImage;

    [SerializeField] private GameObject dialogCanvas;

    [SerializeField] private Transform eventObjectParent;

    [SerializeField] private Button clickCheckButton;

    [Header("버튼 & 외부 참조")]
    [SerializeField] private Button autoPlayButton;

    [Header("캐릭터 관리")]
    public List<Sprite> CharacterSprite;
}

public enum SpeakerType
{
    Player,
    Target,
    Mono
}