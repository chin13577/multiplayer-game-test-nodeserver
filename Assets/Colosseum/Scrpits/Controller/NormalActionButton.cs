using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class NormalActionButton : SkillButton, IPointerDownHandler , IPointerUpHandler
{
    public Action<bool> OnPress;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        if (OnPress != null)
            OnPress(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        if (OnPress != null)
            OnPress(false);
    }
}
