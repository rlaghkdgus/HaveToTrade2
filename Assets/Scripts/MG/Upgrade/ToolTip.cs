using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [Header("설명관련")]
    public RectTransform tooltipRect;
    #region Text
    public TextMeshProUGUI Name;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI pFoodText;
    public TextMeshProUGUI clothesText;
    public TextMeshProUGUI furText;
    public TextMeshProUGUI accText;
    #endregion
    public Canvas canvas;

    private void Awake()
    {
        tooltipRect = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        FollowMouseUI();
    }

    private void Update()
    {
        FollowMouseUI();
    }

    public void FollowMouseUI()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            null,
            out anchoredPos
        );

        tooltipRect.anchoredPosition = anchoredPos;

        ClampedPos();
    }

    public void ClampedPos()
    {
        Vector2 size = tooltipRect.sizeDelta;
        Vector2 canvasSize = (canvas.transform as RectTransform).sizeDelta;

        Vector2 clampedPos = tooltipRect.anchoredPosition;

        if (clampedPos.x + size.x > canvasSize.x / 2)
            tooltipRect.pivot = Vector2.one;
        if (clampedPos.x < -canvasSize.x / 2)
            clampedPos.x = -canvasSize.x / 2;

        if (clampedPos.y > canvasSize.y / 2)
            clampedPos.y = canvasSize.y / 2;
        if (clampedPos.y - size.y < -canvasSize.y / 2)
            tooltipRect.pivot = Vector2.zero;

        if (clampedPos.x + size.x > canvasSize.x / 2 && clampedPos.y - size.y < -canvasSize.y / 2)
        {
            tooltipRect.pivot = new Vector2(1, 0);
        }

        tooltipRect.anchoredPosition = clampedPos;
    }

    public void LoadInfo(UpgradeInfo info)
    {
        Name.text = info.Name;
        foodText.text = "식품 : " + info.Food.ToString();
        pFoodText.text = "가공식품 : " + info.pFood.ToString();
        clothesText.text = "옷 : " + info.Clothes.ToString();
        furText.text = "가구 : " + info.Furniture.ToString();
        accText.text = "장식품 : " + info.Accesory.ToString();
    }
}
