using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Action<Vector2> OnValueChange;
    public Action<bool> OnPress;

    public bool isSelectArea;
    bool isCancel;
    public Image skillImage;
    public Image skillCooldownImage;
    public CanvasGroup canvasGroup;
    public RectTransform stickImage;
    public RectTransform border;

    void Awake()
    {
        Initial();
    }
    
    void Initial()
    {
        canvasGroup.alpha = 0;
        if (isSelectArea == false)
            canvasGroup.gameObject.SetActive(false);
    }
    public void SetSkillImage(Sprite img)
    {
        skillImage.sprite = img;
        skillCooldownImage.sprite = img;
    }
    private Vector2 Calculate(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(border, eventData.position, eventData.enterEventCamera, out pos);
        pos.x = (pos.x / border.sizeDelta.x) * 2 - 1;
        pos.y = (pos.y / border.sizeDelta.y) * 2 + 1;
        pos = (pos.magnitude > 1) ? pos.normalized : pos;
        return pos;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSelectArea)
        {
            canvasGroup.alpha = 0.5f;
            Vector2 pos = Calculate(eventData);
            stickImage.anchoredPosition = new Vector2(pos.x * border.sizeDelta.x / 2.5f, pos.y * border.sizeDelta.y / 2.5f);
            if (OnValueChange != null) { OnValueChange(pos); }
        }
        if (OnPress != null)
            OnPress(true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isSelectArea) return;
        Vector2 pos = Calculate(eventData);
        stickImage.anchoredPosition = new Vector2(pos.x * border.sizeDelta.x / 2.5f, pos.y * border.sizeDelta.y / 2.5f);
        if (OnValueChange != null) { OnValueChange(pos); }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelectArea) return;
        canvasGroup.alpha = 0;
        stickImage.anchoredPosition = Vector2.zero;
        if (OnValueChange != null) { OnValueChange(Vector2.zero); }

        if (isCancel == false)
        {
            if (OnPress != null)
                OnPress(false);
        }
        else
            isCancel = false;
    }
}
