using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInInventoryUI(eventData.position)
                || InventoryManager.Instance.CheckInActionUI(eventData.position)
                || InventoryManager.Instance.CheckInEquipmentUI(eventData.position))
            {
                if (eventData.pointerEnter.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.GetComponentInParent<SlotHolder>();
                }
                if (targetHolder != null)
                {
                    if (targetHolder != InventoryManager.Instance.currentDrag.originalHolder && targetHolder != null)
                    {
                        switch (targetHolder.slotType)
                        {
                            case SlotType.BAG:
                                SwapItem();
                                break;
                            case SlotType.WEAPON:
                                if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                                {
                                    SwapItem();
                                }
                                break;
                            case SlotType.ACTION:
                                if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Usable)
                                {
                                    SwapItem();
                                }
                                break;
                            case SlotType.ARMOR:
                                if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                                {
                                    SwapItem();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    currentHolder.UpdateItem();
                    targetHolder.UpdateItem();
                }
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -5 * Vector2.one;
        t.offsetMin = 5 * Vector2.one;
    }
    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
        bool isSameItem = tempItem.itemData == targetItem.itemData;
        if (isSameItem && targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}