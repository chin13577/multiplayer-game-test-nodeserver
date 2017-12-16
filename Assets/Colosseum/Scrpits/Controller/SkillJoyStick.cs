using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillJoyStick : SkillButton, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public bool isSelectArea;
    public int index;
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
        image = GetComponent<Image>();
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
    public void UpdateStickPosition(Vector2 pos)
    {
        stickImage.anchoredPosition = new Vector2(pos.x * border.sizeDelta.x / 2.5f, pos.y * border.sizeDelta.y / 2.5f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSelectArea)
        {
            canvasGroup.alpha = 0.5f;
            pos = Calculate(eventData);
            ControllerManager.instance.SkillValueChange(ref pos, this);
        }
        ControllerManager.instance.SkillBtnPress(true, this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isSelectArea) return;
        pos = Calculate(eventData);
        ControllerManager.instance.SkillValueChange(ref pos, this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelectArea) return;
        canvasGroup.alpha = 0;
        pos = Vector2.zero;
        ControllerManager.instance.SkillValueChange(ref pos, this);

        if (isCancel == false)
        {
            ControllerManager.instance.SkillBtnPress(false, this);
        }
        else
            isCancel = false;
    }
}
