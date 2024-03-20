using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Data/Package/Shield Data")]
[System.Serializable]
public class ShieldData_SO : ScriptableObject
{
    public int healthIncrease;
    public int defenceIncrease;
}
