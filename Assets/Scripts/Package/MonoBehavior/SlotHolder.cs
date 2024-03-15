using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType
{
    BAG,
    WEAPON,
    ACTION,
    ARMOR
}
public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }
    public void UseItem()
    {
        if (itemUI.GetItem())
        {
            if (itemUI.GetItem().itemType == ItemType.Usable && itemUI.Bag.items[itemUI.Index].amount > 0 && GameManager.Instance.playerStates.CanApplyHealth())
            {
                GameManager.Instance.playerStates.ApplyHealth(itemUI.GetItem().usableItemData.healPoint);
                itemUI.Bag.items[itemUI.Index].amount -= 1;
                TaskManager.Instance.UpdateTaskProgress(itemUI.GetItem().itemName, -1);
            }
            UpdateItem();
        }
    }
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if (itemUI.GetItem() != null)
                {
                    if (GameManager.Instance.playerStates.state == CharacterStates.AnimatorStates.Shield && itemUI.GetItem().animatorStates == CharacterStates.AnimatorStates.Hand2)
                    {
                        if (InventoryManager.Instance.equipmentData.items[1].itemData != null)
                        {
                            InventoryManager.Instance.inventoryData.AddItem(InventoryManager.Instance.equipmentData.items[1].itemData, InventoryManager.Instance.equipmentData.items[1].itemData.itemAmount);
                            InventoryManager.Instance.equipmentData.items[1].itemData = null;
                            InventoryManager.Instance.infoUI.RefreshUI();
                            InventoryManager.Instance.inventoryUI.RefreshUI();
                        }
                    }
                    GetComponent<Image>().enabled = false;
                    GameManager.Instance.playerStates.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GetComponent<Image>().enabled = true;
                    GameManager.Instance.playerStates.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if (itemUI.GetItem() != null)
                {
                    if (GameManager.Instance.playerStates.state == CharacterStates.AnimatorStates.Hand2 && itemUI.GetItem().animatorStates == CharacterStates.AnimatorStates.Shield)
                    {
                        if (InventoryManager.Instance.equipmentData.items[0].itemData != null)
                        {
                            InventoryManager.Instance.inventoryData.AddItem(InventoryManager.Instance.equipmentData.items[0].itemData, InventoryManager.Instance.equipmentData.items[0].itemData.itemAmount);
                            InventoryManager.Instance.equipmentData.items[0].itemData = null;
                            InventoryManager.Instance.infoUI.RefreshUI();
                            InventoryManager.Instance.inventoryUI.RefreshUI();
                        }
                    }
                    GetComponent<Image>().enabled = false;
                    GameManager.Instance.playerStates.ChangeShield(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GetComponent<Image>().enabled = true;
                    GameManager.Instance.playerStates.UnEquipShield();
                }
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
            default:
                break;
        }
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.UpdateItemUI(item.itemData, item.amount);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.tooltip.SetTooltip(itemUI.GetItem());
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }
}
