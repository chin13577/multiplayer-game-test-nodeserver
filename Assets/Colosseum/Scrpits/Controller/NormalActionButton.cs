﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class NormalActionButton : ActionJoyStick, IPointerDownHandler , IPointerUpHandler
{
    public Action<bool> OnPress;
    void Awake()
    {
        isActivate = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        InputHandler.instance.ActionBtnPress(true, this);
        if (OnPress != null)
            OnPress(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        InputHandler.instance.ActionBtnPress(false, this);
        if (OnPress != null)
            OnPress(false);
    }
}
