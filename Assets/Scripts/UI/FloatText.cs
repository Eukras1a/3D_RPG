using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class FloatText : MonoBehaviour
{
    public GameObject textPrefab;
    private ObjectPool<TextMeshProUGUI> floatTextPool;
    TextStyle defaultStyle = new TextStyle(35f, 35f, 0f, Color.red, HorizontalAlignmentOptions.Geometry, 1f, 0.7f, 0.5f, new Vector3(0f, 3f, 0f), new Vector3(0f, 3f, 0f), new Vector3(0f, -4f, 0f));

    private void Start()
    {
        floatTextPool = new ObjectPool<TextMeshProUGUI>(Creat_UIText, Get_UIText, Release_UIText, Destroy_UIText);
    }
    private TextMeshProUGUI Creat_UIText()
    {
        GameObject gameObject = Instantiate(textPrefab, transform, false);
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        return gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void Get_UIText(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(true);
    }
    private void Release_UIText(TextMeshProUGUI text)
    {
        text.gameObject.SetActive(false);
    }
    private void Destroy_UIText(TextMeshProUGUI text)
    {
        Destroy(text.gameObject);
    }
    public void CreatFloatTextInUI(string content, Vector3 localPosition, bool isCritical, Transform followTarget = null)
    {
        StartCoroutine(CreatFloatTextCore(defaultStyle, content, isCritical, localPosition, followTarget));
    }
    private IEnumerator CreatFloatTextCore(TextStyle style, string content, bool isCritical, Vector3 localPosition, Transform followTarget)
    {
        TMP_Text text = floatTextPool.Get();
        text.horizontalAlignment = style.alignment;
        text.color = style.fontColor;
        text.fontSize = style.fontStartSize;
        float fontStartSize = style.fontStartSize;
        float fontFinalSize = style.fontFinalSize;
        text.text = content;
        if (isCritical)
        {
            text.fontSize = style.fontStartSize * 1.2f;
            fontStartSize = style.fontStartSize * 1.2f;
            fontFinalSize = style.fontFinalSize * 1.2f;
            text.text = content + " !";
        }
        float fontSizeScaleTime = style.fontSizeScaleTime;
        Color color = style.fontColor;
        float finalTime = style.lifeTime;
        float animateTime = style.animateTime;
        float startFadeTime = style.startFadeTime;
        Vector3 gravity = style.gravity;

        Vector3 worldPosition;//实例的世界坐标。
        float startTime = Time.time;
        float currentTime = 0;
        Vector3 currentVelocity = new Vector3(Random.Range(style.minVelocity.x, style.maxVelocity.x), Random.Range(style.minVelocity.y, style.maxVelocity.y), Random.Range(style.minVelocity.z, style.maxVelocity.z));
        Vector3 currentMovement = Vector3.zero;
        float alphaLerp = 0;
        float alphaTimer = style.lifeTime - style.startFadeTime;
        float fontSizeChangeLerp = 0;
        while (currentTime < finalTime)
        {
            if (followTarget != null)
            {
                worldPosition = followTarget.position + localPosition;
            }
            else
            {
                worldPosition = localPosition;
            }
            if (currentTime < animateTime)
            {
                currentVelocity += gravity * Time.deltaTime;
                currentMovement += currentVelocity * Time.deltaTime;

            }
            worldPosition += currentMovement;
            if (currentTime > startFadeTime)
            {
                alphaLerp += 1 * Time.deltaTime / alphaTimer;
                text.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1, 0, alphaLerp));
            }
            if (currentTime < fontSizeScaleTime)
            {
                fontSizeChangeLerp += 1 * Time.deltaTime / fontSizeScaleTime;
                text.fontSize = Mathf.Lerp(fontStartSize, fontFinalSize, fontSizeChangeLerp);
            }

            text.transform.position = Camera.main.WorldToScreenPoint(worldPosition);

            yield return null;
            currentTime = Time.time - startTime;
        }
        floatTextPool.Release(text as TextMeshProUGUI);
    }
}
