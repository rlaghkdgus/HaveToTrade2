using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiSlide : MonoBehaviour
{
    public RectTransform Slide_UI;
    public float duration;
    public float overshoot;
    public Vector2 startPos;
    public Vector2 endPos;

    public void MoveSlide()
    {
        Slide_UI.anchoredPosition = startPos;

        Slide_UI.DOAnchorPos(endPos, duration).SetEase(Ease.OutBack, overshoot);
    }
}
