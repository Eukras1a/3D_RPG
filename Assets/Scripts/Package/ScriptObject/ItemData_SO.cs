using UnityEngine;

public enum ItemType
{
    Usable,
    Weapon,
    Armor
}

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Package/Item Data")]
[System.Serializable]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    public bool stackable;
    [TextArea]
    public string itemDescription = "";

    [Header("Equipment")]
    public GameObject equipmentPrefab;
    public AttackData_SO weaponData;
    public ShieldData_SO shieldData;
    public CharacterStates.AnimatorStates animatorStates;
    public AnimatorOverrideController overrideController;

    [Header("Usable Item")]
    public UsableItemData_SO usableItemData; 
}
