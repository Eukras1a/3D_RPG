using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text itemName;
    public Text itemInfo;

    RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetTooltip(ItemData_SO item)
    {
        itemName.text = item.name;
        itemInfo.text = item.itemDescription;
    }
    private void OnEnable()
    {
        UpdateMousePosition();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    private void Update()
    {
        UpdateMousePosition();
    }
    public void UpdateMousePosition()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 mousePos = Input.mousePosition;
        float height = corners[1].y - corners[0].y;
        float width = corners[3].x - corners[0].x;
        if (mousePos.y < height)
            rectTransform.position = mousePos + (0.6f * height * Vector3.up);
        else if (Screen.width - mousePos.x > width)
            rectTransform.position = mousePos + (0.6f * width * Vector3.right);
        else
            rectTransform.position = mousePos + (0.6f * width * Vector3.left);
    }
}
