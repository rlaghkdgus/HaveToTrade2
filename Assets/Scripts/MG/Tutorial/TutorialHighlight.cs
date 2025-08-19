using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHighlight : MonoBehaviour
{
    public GameObject background;
    public GameObject hole;
    public RectTransform holeRectTransform;

    private void Awake()
    {
        Hide();
    }

    public void Highlight(RectTransform uiTarget)
    {
        if (!holeRectTransform || !uiTarget || !background) return;

        background.SetActive(true);
        hole.SetActive(true);
        holeRectTransform.position = uiTarget.position;
        holeRectTransform.sizeDelta = uiTarget.sizeDelta;
    }

    public void Highlight(GameObject Object, Camera camera)
    {
        if (!holeRectTransform || !Object || !camera || !background) return;

        Renderer renderer = Object.GetComponent<Renderer>();
        if(renderer == null)
        {
            Debug.LogWarning("Renderer 없음");
            return;
        }

        background.SetActive(true);
        hole.SetActive(true);
        Bounds bounds = renderer.bounds;
        Vector3[] screenPoints = new Vector3[8];

        // 바운딩 박스의 8개 꼭짓점을 화면 좌표로 변환
        screenPoints[0] = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
        screenPoints[1] = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z));
        screenPoints[2] = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
        screenPoints[3] = camera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z));
        screenPoints[4] = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
        screenPoints[5] = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z));
        screenPoints[6] = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z));
        screenPoints[7] = camera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));
        
        // 화면 좌표의 최소/최대 값을 찾아 사각형을 만듦
        Vector2 min = screenPoints[0];
        Vector2 max = screenPoints[0];
        foreach (Vector3 screenPoint in screenPoints)
        {
            min.x = Mathf.Min(screenPoint.x, min.x);
            min.y = Mathf.Min(screenPoint.y, min.y);
            max.x = Mathf.Max(screenPoint.x, max.x);
            max.y = Mathf.Max(screenPoint.y, max.y);
        }
        
        // 화면 좌표를 기준으로 마스크 위치와 크기 설정
        holeRectTransform.position = (min + max) / 2;
        holeRectTransform.sizeDelta = max - min;
    }

    public void Hide()
    {
        background?.SetActive(false);
        hole?.SetActive(false);
    }
}
