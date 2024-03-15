using UnityEngine;
using UnityEngine.EventSystems;

public class TaskTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;
    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        TaskUI.Instance.tooltip.gameObject.SetActive(true);
        TaskUI.Instance.tooltip.SetTooltip(currentItemUI.currentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TaskUI.Instance.tooltip.gameObject.SetActive(false);
    }
}
