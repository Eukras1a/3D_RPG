using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "AttackData/Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;
    public float criticalMultiplier;//±©»÷ÏµÊý
    public float criticalChance;//±©»÷ÂÊ

    public void AddWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        criticalChance = weapon.criticalChance;
        criticalMultiplier = weapon.criticalMultiplier;

        minDamage += weapon.minDamage;
        maxDamage += weapon.maxDamage;
    }
    public void RestoreAttackData(AttackData_SO data)
    {
        attackRange = data.attackRange;
        skillRange = data.skillRange;
        coolDown = data.coolDown;
        criticalChance = data.criticalChance;
        criticalMultiplier = data.criticalMultiplier;

        minDamage = data.minDamage;
        maxDamage = data.maxDamage;
    }

    public void LevelUp()
    {
        minDamage += 1;
        maxDamage += 1;
    }
}
