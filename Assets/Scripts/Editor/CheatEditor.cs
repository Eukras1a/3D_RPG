
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CheatEditor : EditorWindow
{
    [MenuItem("Cheat/Cheat Editor")]
    
    public static void Init()
    {
        CheatEditor editorWindow = GetWindow<CheatEditor>("Cheat Editor");
        editorWindow.autoRepaintOnSceneChange = true;
    }
    List<string> itemKeyList = new List<string>();
    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            if (GameManager.Instance.IsPlayerInitialized)
            {
                GUILayout.Label("½ÇÉ«ÊôÐÔ");
                itemKeyList.Clear();
                foreach (KeyValuePair<string, ItemData_SO> item in ItemStore.itemList)
                {
                    itemKeyList.Add(item.Key);
                }
                foreach (var item in itemKeyList)
                {
                    if (GUILayout.Button(item))
                    {
                        GetItem(ItemStore.itemList[item]);
                    }
                }
            }
        }
    }
    void GetItem(ItemData_SO itemData)
    {
        InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
        InventoryManager.Instance.inventoryUI.RefreshUI();
        TaskManager.Instance.UpdateTaskProgress(itemData.itemName, itemData.itemAmount);
    }
}
