using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public Action<Vector2> OnLeftJoyStickChange;
    public Action<Vector2> OnSkillBtnDrag;
    public Action<bool, int> OnSkillBtnPress;
    public static ControllerManager instance = null;
    public TouchJoyStick leftJoyStick;
    public SkillJoyStick[] skillJoyStick;

    private SkillJoyStick selectedSkillBtn;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        leftJoyStick.OnValueChange += Callback_OnLeftJoyStickChange;
    }
    private void OnDisable()
    {
        leftJoyStick.OnValueChange -= Callback_OnLeftJoyStickChange;
    }
    void Callback_OnLeftJoyStickChange(Vector2 pos)
    {
        if (OnLeftJoyStickChange != null)
            OnLeftJoyStickChange(pos);
    }
    public void SkillBtnPress(bool isPress, SkillJoyStick button)
    {
        if (isPress == true)
        {
            if (selectedSkillBtn == null)
                selectedSkillBtn = button;
            else
                return;
        }
        else
        {
            if (selectedSkillBtn == button)
                selectedSkillBtn = null;
            else
                return;
        }
        if (OnSkillBtnPress != null)
            OnSkillBtnPress(isPress, GetSkillButtonIndex(button));
    }
    public void SkillValueChange(ref Vector2 value, SkillJoyStick button)
    {
        if (selectedSkillBtn != button)
            return;
        if (selectedSkillBtn.isSelectArea == false)
            return;
        selectedSkillBtn.UpdateStickPosition(value);
        if (OnSkillBtnDrag != null)
            OnSkillBtnDrag(value);

    }
    int GetSkillButtonIndex(SkillJoyStick btn)
    {
        for (int i = 0; i < skillJoyStick.Length; i++)
        {
            if (btn == skillJoyStick[i])
                return i;
        }
        return -1;
    }
}
