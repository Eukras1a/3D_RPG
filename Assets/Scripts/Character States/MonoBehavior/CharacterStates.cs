using System;
using UnityEngine;

[System.Serializable]
public class CharacterStates : MonoBehaviour
{
    public event Action<int, int> UpdateHealthUIOnAttack;
    public CharacterData_SO characterDataTemplate;
    public AttackData_SO attackDataTemplate;

    [HideInInspector] public CharacterData_SO characterData;
    [HideInInspector] public AttackData_SO attackData;
    [HideInInspector] public AttackData_SO baseAttackData;
    [HideInInspector] public bool isCritical;
    //public ItemData_SO weaponEquiped;

    [Header("Equipment")]
    public Transform weaponSlot;
    public Transform shieldSlot;

    private int _health;
    private int _denfence;
    private RuntimeAnimatorController baseAnimator;

    [System.Serializable]
    public enum AnimatorStates
    {
        None,
        Unarmed,
        Shield,
        Hand2
    }
    public AnimatorStates state = AnimatorStates.Unarmed;
    void Awake()
    {
        if (characterDataTemplate != null)
        {
            characterData = Instantiate(characterDataTemplate);
        }
        if (attackDataTemplate != null)
        {
            attackData = Instantiate(attackDataTemplate);
            baseAttackData = Instantiate(attackDataTemplate);
        }
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    #region Read from CharacterData_SO
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.maxHealth;
            else
                return 0;
        }
        set
        {
            characterData.maxHealth = value;
        }
    }
    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            else
                return 0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }
    public int BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            else
                return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }
    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            else
                return 0;
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    public int CurrentLevel
    {
        get
        {
            if (characterData != null)
                return characterData.currentLevel;
            else
                return 0;
        }
        set
        {
            characterData.currentLevel = value;
        }
    }
    public int MaxLevel
    {
        get
        {
            if (characterData != null)
                return characterData.maxLevel;
            else
                return 0;
        }
        set
        {
            characterData.maxLevel = value;
        }
    }
    public int CurrentExp
    {
        get
        {
            if (characterData != null)
                return characterData.currentExp;
            else
                return 0;
        }
        set
        {
            characterData.currentExp = value;
        }
    }
    public int BaseExp
    {
        get
        {
            if (characterData != null)
                return characterData.baseExp;
            else
                return 0;
        }
        set
        {
            characterData.baseExp = value;
        }
    }
    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStates attacker, CharacterStates defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            Debug.Log("暴击！" + attacker.gameObject.tag + "对" + defener.gameObject.tag + "造成了" + damage + "点伤害！"
                + defener.gameObject.tag + "当前血量:" + CurrentHealth);
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        else
        {
            Debug.Log(attacker.gameObject.tag + "对" + defener.gameObject.tag + "造成了" + damage + "点伤害！"
                + defener.gameObject.tag + "当前血量:" + CurrentHealth);
        }
        UpdateHealthUIOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (attacker.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<FloatText>().CreatFloatTextInUI(damage.ToString(), defener.transform.position + new Vector3(0, 1.8f, 0), attacker.isCritical);
        }
        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killPoint);
        }
    }
    public void TakeDamage(int damage, CharacterStates defener)
    {
        int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        Debug.Log("对" + defener.gameObject.tag + "造成了" + currentDamage + "点伤害！"
                + defener.gameObject.tag + "当前血量:" + CurrentHealth);
        UpdateHealthUIOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
        {
            GameManager.Instance.playerStates.characterData.UpdateExp(characterData.killPoint);
        }
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }
    #endregion

    #region Equip Weapon
    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.equipmentPrefab != null)
        {
            Instantiate(weapon.equipmentPrefab, weaponSlot);
            GetComponent<Animator>().runtimeAnimatorController = weapon.overrideController;
            attackData.AddWeaponData(weapon.weaponData);
            state = weapon.animatorStates;
        }
    }
    public void UnEquipWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        attackData.RestoreAttackData(baseAttackData);
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
        state = AnimatorStates.Unarmed;
    }
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }
    public void EquipShield(ItemData_SO shield)
    {
        if (shield.equipmentPrefab != null)
        {
            Instantiate(shield.equipmentPrefab, shieldSlot);
            _health = shield.shieldData.healthIncrease;
            _denfence = shield.shieldData.defenceIncrease;
            MaxHealth += shield.shieldData.healthIncrease;
            CurrentHealth += shield.shieldData.healthIncrease;
            CurrentDefence += shield.shieldData.defenceIncrease;
        }
        GetComponent<Animator>().runtimeAnimatorController = shield.overrideController;
        state = shield.animatorStates;
    }
    public void UnEquipShield()
    {
        if (shieldSlot.transform.childCount != 0)
        {
            for (int i = 0; i < shieldSlot.transform.childCount; i++)
            {
                Destroy(shieldSlot.transform.GetChild(i).gameObject);
            }
        }
        CurrentDefence -= _denfence;
        if (CurrentHealth < MaxHealth - _health)
        {
            MaxHealth -= _health;
        }
        else
        {
            MaxHealth -= _health;
            CurrentHealth = MaxHealth;
        }
        _health = 0;
        _denfence = 0;
        state = AnimatorStates.Unarmed;
    }
    public void ChangeShield(ItemData_SO shield)
    {
        UnEquipShield();
        EquipShield(shield);
    }
    #endregion

    #region Data Change
    public bool CanApplyHealth()
    {
        return CurrentHealth != MaxHealth;
    }
    public void ApplyHealth(int amount)
    {
        if (CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
    }
    #endregion
}
