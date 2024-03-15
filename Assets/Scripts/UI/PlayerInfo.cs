using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Text health;
    public Text defence;
    public Text level;
    public Text exp;
    public Text attack;

    void LateUpdate()
    {
        
    }
    public void UpdateInfo()
    {
        health.text = "Health:" + GameManager.Instance.playerStates.CurrentHealth + "/" + GameManager.Instance.playerStates.MaxHealth;
        defence.text = "Defence:" + GameManager.Instance.playerStates.BaseDefence + "/" + GameManager.Instance.playerStates.CurrentDefence;
        level.text = "Level:" + GameManager.Instance.playerStates.CurrentLevel + "/" + GameManager.Instance.playerStates.MaxLevel;
        exp.text = "Exp:" + GameManager.Instance.playerStates.CurrentExp + "/" + GameManager.Instance.playerStates.BaseExp;
        attack.text = "Damage:" + GameManager.Instance.playerStates.attackData.minDamage + "-" + GameManager.Instance.playerStates.attackData.maxDamage;
    }
}
