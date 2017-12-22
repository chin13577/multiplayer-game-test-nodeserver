using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillJoyStick : ActionJoyStick, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool isSelectArea;
    bool isCancel;
    public Image skillImage;
    public Image skillCooldownImage;
    public CanvasGroup canvasGroup;
    public RectTransform stickImage;
    public RectTransform border;

    Vector2 pos;

     void Awake()
    {
        Initial();
    }

    void Initial()
    {
        canvasGroup.alpha = 0;
        if (isSelectArea == false)
            canvasGroup.gameObject.SetActive(false);
        isActivate = true;
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
    public void UpdateStickPosition(Vector2 pos)
    {
        stickImage.anchoredPosition = new Vector2(pos.x * border.sizeDelta.x / 2.5f, pos.y * border.sizeDelta.y / 2.5f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        if (isSelectArea)
        {
            canvasGroup.alpha = 0.5f;
            pos = Calculate(eventData);
            UpdateStickPosition(pos);
            InputHandler.instance.ActionBtnDrag(ref pos);
        }
        InputHandler.instance.ActionBtnPress(true, this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        if (!isSelectArea) return;
        pos = Calculate(eventData);
        UpdateStickPosition(pos);
        InputHandler.instance.ActionBtnDrag(ref pos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        if (!isSelectArea) return;
        canvasGroup.alpha = 0;
        pos = Vector2.zero;
        UpdateStickPosition(pos);
        InputHandler.instance.ActionBtnDrag(ref pos);

        if (isCancel == false)
            InputHandler.instance.ActionBtnPress(false, this);
        else
            isCancel = false;
    }
}
