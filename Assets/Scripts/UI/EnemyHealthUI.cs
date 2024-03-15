using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public GameObject HealthUIPrefab;
    public bool alwaysVisible;
    public float visibleTime;
    float remainvisibleTime;
    Transform HealthUIPoint;
    Transform UIbar;
    Image healthSlider;
    Transform cam;
    CharacterStates currentStates;

    void Awake()
    {
        currentStates = GetComponent<CharacterStates>();
        currentStates.UpdateHealthUIOnAttack += UpdateHealth;
        HealthUIPoint = transform.Find("HealthUIPoint");
    }
    void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.CompareTag("EnemyHealthUI") && canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(HealthUIPrefab, canvas.transform).transform;
                //UIbar.localScale = new Vector2(GetComponent<BoxCollider>().size.x, GetComponent<BoxCollider>().size.x/5);
                healthSlider = UIbar.GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0 && UIbar != null)
        {
            Destroy(UIbar.gameObject);
        }
        UIbar.gameObject.SetActive(true);
        remainvisibleTime = visibleTime;
        healthSlider.fillAmount = (float)currentHealth / maxHealth;
    }

    void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = HealthUIPoint.position;
            UIbar.forward = -cam.forward;

            if (remainvisibleTime <= 0 && !alwaysVisible)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                remainvisibleTime -= Time.deltaTime;
            }
        }
    }
}
