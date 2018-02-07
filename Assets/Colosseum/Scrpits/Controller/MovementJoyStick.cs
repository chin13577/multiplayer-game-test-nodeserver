using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MovementJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Action<Vector2> OnValueChange;
    public Action<bool, Vector2> OnPress;
    RectTransform button;
    RectTransform rect;

    void Awake()
    {
        Initial();
    }
    void Initial()
    {
        rect = GetComponent<RectTransform>();
        button = transform.GetChild(0).GetComponent<RectTransform>();
    }
    private Vector2 Calculate(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.enterEventCamera, out pos);
        pos.x = (pos.x / rect.sizeDelta.x) * 2 - 1;
        pos.y = (pos.y / rect.sizeDelta.y) * 2 + 1;
        pos = (pos.magnitude > 1) ? pos.normalized : pos;
        return pos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = Calculate(eventData);
        button.anchoredPosition = new Vector2(pos.x * rect.sizeDelta.x / 2.5f, pos.y * rect.sizeDelta.y / 2.5f);
        //if (OnValueChange != null) { OnValueChange(pos); }
        InputHandler.instance.MovementStickPress(true, pos);
        if (OnPress != null)
        {
            OnPress(true, pos);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Calculate(eventData);
        button.anchoredPosition = new Vector2(pos.x * rect.sizeDelta.x / 2.5f, pos.y * rect.sizeDelta.y / 2.5f);
        InputHandler.instance.MovementStickChange(pos);
        if (OnValueChange != null) { OnValueChange(pos); }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        button.anchoredPosition = Vector2.zero;
        InputHandler.instance.MovementStickPress(false, Vector2.zero);
        //if (OnValueChange != null) { OnValueChange(Vector2.zero); }
        if (OnPress != null)
        {
            OnPress(false, Vector2.zero);
        }
    }
}
