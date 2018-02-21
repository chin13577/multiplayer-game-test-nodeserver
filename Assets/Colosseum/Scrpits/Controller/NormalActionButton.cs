using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class NormalActionButton : ActionJoyStick, IPointerDownHandler , IPointerUpHandler
{
    void Awake()
    {
        base.Initial();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        InputHandler.instance.ActionBtnPress(true, this);
        InputHandler.instance.OnRollBtnPress(true, this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActivate == false)
            return;
        InputHandler.instance.ActionBtnPress(false, this);
        InputHandler.instance.OnRollBtnPress(false, this);
    }
}
