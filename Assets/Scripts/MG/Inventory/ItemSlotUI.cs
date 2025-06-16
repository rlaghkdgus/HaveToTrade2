using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI Count;
    [SerializeField] private TextMeshProUGUI Weight;

    [SerializeField] private int myIndex;

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void Set(pItem slotitem)
    {
        icon.sprite = slotitem.image;

        if (price != null && Weight != null && Count != null)
        {
            price.text = "구매가격 : " + slotitem.price.ToString();
            Count.text = "개수 : " + slotitem.counts.ToString();
            Weight.text = "무게 : " + slotitem.weight.ToString();
        }
    }
}
