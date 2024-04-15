using System.Collections.Generic;
using UnityEditor;

public class ItemStore
{
    public static Dictionary<string ,ItemData_SO> itemList = new Dictionary<string, ItemData_SO>() {
        { "Shield",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Shield.asset") },
        { "Sword",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Sword.asset") },
    };
}
