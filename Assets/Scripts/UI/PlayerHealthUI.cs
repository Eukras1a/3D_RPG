using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text level;
    Image health;
    Image exp;
    void Awake()
    {
        health = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        exp = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        level = transform.GetChild(2).GetComponent<Text>();
    }
    void Update()
    {
        UpdateInfo();
    }
    void UpdateInfo()
    {
        if (!SceneController.Instance.IsTrans)
        {
            health.fillAmount = (float)GameManager.Instance.playerStates.CurrentHealth / GameManager.Instance.playerStates.MaxHealth;
            exp.fillAmount = (float)GameManager.Instance.playerStates.CurrentExp / GameManager.Instance.playerStates.BaseExp;
            level.text = "LEVEL:" + GameManager.Instance.playerStates.CurrentLevel;
        }
    }
}
