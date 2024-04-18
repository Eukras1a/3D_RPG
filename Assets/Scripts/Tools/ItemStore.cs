using System.Collections.Generic;
using UnityEditor;

public class ItemStore
{
    public static Dictionary<string ,ItemData_SO> itemList = new Dictionary<string, ItemData_SO>() {
        { "ľ��",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Shield_Wood.asset") },
        { "������",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Shield_Metal.asset") },
        { "���ֽ�",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Sword.asset") },
        { "˫�ֽ�",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Sword_2Hand.asset") },
        { "Ģ��",AssetDatabase.LoadAssetAtPath<ItemData_SO>("Assets/Game Data/Item Data/Usable Item/Mushroom.asset") },
    };
}
