using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{

    public RectTransform button;
    RectTransform rect;
    Vector2 defaultPos;

    void Awake()
    {
        Initial();
    }
    void Initial()
    {
        rect = GetComponent<RectTransform>();
        print(defaultPos);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.enterEventCamera, out pos);
        pos.x = (pos.x / rect.sizeDelta.x) * 2 - 1;
        pos.y = (pos.y / rect.sizeDelta.y) * 2 + 1;
        pos = (pos.magnitude > 1) ? pos.normalized : pos;
        button.anchoredPosition = new Vector2(pos.x * rect.sizeDelta.x / 2.5f, pos.y * rect.sizeDelta.y / 2.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.enterEventCamera, out pos);
        pos.x = (pos.x / rect.sizeDelta.x) * 2 - 1;
        pos.y = (pos.y / rect.sizeDelta.y) * 2 + 1;
        pos = (pos.magnitude > 1) ? pos.normalized : pos;
        button.anchoredPosition = new Vector2(pos.x * rect.sizeDelta.x / 2.5f, pos.y * rect.sizeDelta.y / 2.5f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        button.anchoredPosition = Vector2.zero;
    }

}
