using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;

    SlotHolder currentSlotHolder;

    void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();
    }
    void Update()
    {
        if (Input.GetKeyDown(actionKey) && currentSlotHolder.itemUI.GetItem())
        {
            currentSlotHolder.UseItem();
        }
    }
}