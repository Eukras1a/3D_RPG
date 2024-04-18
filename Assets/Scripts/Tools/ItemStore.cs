using System.Collections.Generic;
using UnityEditor;

public class ItemStore
{
    public static Dictionary<string ,ItemData_SO> itemList = new Dictionary<string, ItemData_SO>() {
        { "Ä¾¶Ü",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Shield_Wood.asset") },
        { "½ðÊô¶Ü",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Shield_Metal.asset") },
        { "µ¥ÊÖ½£",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Sword.asset") },
        { "Ë«ÊÖ½£",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Sword_2Hand.asset") },
        { "Ä¢¹½",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Usable Item/Mushroom.asset") },
    };
}
