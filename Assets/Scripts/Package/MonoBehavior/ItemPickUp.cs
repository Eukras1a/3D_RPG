using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO:·Å½ø±³°ü
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            TaskManager.Instance.UpdateTaskProgress(itemData.itemName,itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}
