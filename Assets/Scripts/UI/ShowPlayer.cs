using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MainMenu menu;
    public void OnPointerEnter(PointerEventData eventData)
    {
        menu.ShowSelectPlayer(gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menu.ShowSelectPlayer(string.Empty);
    }
}
