using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatEditor : EditorWindow
{
    [MenuItem("Editor/Cheat Editor")]

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
                GUILayout.Label("装备");
                itemKeyList.Clear();
                foreach (KeyValuePair<string, ItemData_SO> item in ItemStore.itemList)
                {
                    itemKeyList.Add(item.Key);
                }
                foreach (var item in itemKeyList)
                {
                    if (GUILayout.Button(item, GUILayout.Width(120)))
                    {
                        GetItem(ItemStore.itemList[item]);
                    }
                }
                GUILayout.Label("属性");
                if (GUILayout.Button("升级", GUILayout.Width(120)))
                {
                    GameManager.Instance.playerStates.characterData.LevelUp();
                }
                if (GUILayout.Button("回复", GUILayout.Width(120)))
                {
                    GameManager.Instance.playerStates.characterData.currentHealth = GameManager.Instance.playerStates.characterData.maxHealth;
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
