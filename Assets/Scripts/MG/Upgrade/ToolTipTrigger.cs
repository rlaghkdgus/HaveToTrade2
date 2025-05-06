using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPrefab;

    private GameObject tooltipClone;
    private UpgradeNode node;

    private void Awake()
    {
        node = GetComponent<UpgradeNode>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipClone == null)
        {
            tooltipClone = Instantiate(tooltipPrefab, transform.parent.parent.parent);
            tooltipClone.GetComponent<ToolTip>().LoadInfo(UpgradeManager.Instance.InfoSearch_ID(node.ID));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tooltipClone != null)
        {
            Destroy(tooltipClone);
            tooltipClone = null;
        }
    }
}
