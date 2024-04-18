using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    [Header("Package Data")]
    public PackageData_SO inventoryTemplate;
    public PackageData_SO actionTemplate;
    public PackageData_SO equipmentTemplate;

    [HideInInspector] public PackageData_SO inventoryData;
    [HideInInspector] public PackageData_SO actionData;
    [HideInInspector] public PackageData_SO equipmentData;

    [Header("Container")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI infoUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject infoPanel;

    [Header("Tooltip")]
    public Tooltip tooltip;

    bool isOpenBag = false;
    bool isOpenInfo = false;

    protected override void Awake()
    {
        base.Awake();
        if (inventoryTemplate != null)
        {
            inventoryData = Instantiate(inventoryTemplate);
        }
        if (actionTemplate != null)
        {
            actionData = Instantiate(actionTemplate);
        }
        if (equipmentTemplate != null)
        {
            equipmentData = Instantiate(equipmentTemplate);
        }
    }
    void Start()
    {
        SaveManager.Instance.GetInventoryData();
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        infoUI.RefreshUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpenBag = !isOpenBag;
            bagPanel.SetActive(isOpenBag);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            isOpenInfo = !isOpenInfo;
            infoPanel.SetActive(isOpenInfo);
        }
        if (isOpenInfo)
        {
            infoUI.GetComponent<PlayerInfo>().UpdateInfo();
        }
    }
    public void SaveData(string name)
    {
        SaveManager.Instance.Save(inventoryData, name + inventoryData.name);
        SaveManager.Instance.Save(actionData, name + actionData.name);
        SaveManager.Instance.Save(equipmentData, name + equipmentData.name);
    }
    public void LoadData(string name)
    {
        SaveManager.Instance.Load(inventoryData, name + inventoryData.name);
        SaveManager.Instance.Load(actionData, name + actionData.name);
        SaveManager.Instance.Load(equipmentData, name + equipmentData.name);
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        infoUI.RefreshUI();
    }
    #region CheckUI
    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)inventoryUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)actionUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < infoUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)infoUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    public void CheckTaskItem(string requireName)
    {
        foreach (var item in inventoryData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == requireName)
                {
                    TaskManager.Instance.UpdateTaskProgress(requireName, item.itemData.itemAmount);
                }
            }
        }
        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == requireName)
                {
                    TaskManager.Instance.UpdateTaskProgress(requireName, item.itemData.itemAmount);
                }
            }
        }
    }
    public PackageItem SearchTaskItemInBag(ItemData_SO data)
    {
        return inventoryData.items.Find(i => i.itemData == data);
    }
    public PackageItem SearchTaskItemInAction(ItemData_SO data)
    {
        return actionData.items.Find(i => i.itemData == data);
    }
    public void SetInfo()
    {
        isOpenInfo = false;
    }
    public void SetBag()
    {
        isOpenBag = false;
    }
}
