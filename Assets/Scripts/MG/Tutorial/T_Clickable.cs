using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class T_Clickable : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI 클릭");
        onClicked.Invoke();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Debug.Log("Object 클릭");
        onClicked.Invoke();
    }
}
