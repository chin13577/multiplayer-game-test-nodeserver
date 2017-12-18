using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public Action<Vector2> OnLeftJoyStickChange;
    public Action<Vector2> OnSkillBtnDrag;
    public Action<bool, int> OnSkillBtnPress;
    public NormalActionButton rollBtn;
    public NormalActionButton atkBtn;
    public TouchJoyStick leftJoyStick;
    public SkillJoyStick[] skillJoyStick;
    public SkillButton[] skillButton;

    private SkillButton selectedSkillBtn;
    public static ControllerManager _instance = null;
    public static ControllerManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ControllerManager>();
            }
            return _instance;
        }
    }
    //Awake is always called before any Start functions
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerCreated += OnPlayerCreated;
        leftJoyStick.OnValueChange += Callback_OnLeftJoyStickChange;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerCreated -= OnPlayerCreated;
        leftJoyStick.OnValueChange -= Callback_OnLeftJoyStickChange;
    }
    void OnPlayerCreated(PlayerController playerController)
    {
        if (playerController.isLocalPlayer)
        {
            skillJoyStick[0].isSelectArea = playerController.skill[0].isSelectArea;
            skillJoyStick[1].isSelectArea = playerController.skill[1].isSelectArea;
        }
    }
    void Callback_OnLeftJoyStickChange(Vector2 pos)
    {
        if (OnLeftJoyStickChange != null)
            OnLeftJoyStickChange(pos);
    }
    public void SkillBtnPress(bool isPress, SkillButton button)
    {
        if (isPress == true)
            selectedSkillBtn = button;
        else
            selectedSkillBtn = null;

        UpdateActivateSkillButton(isPress);
        if (OnSkillBtnPress != null)
            OnSkillBtnPress(isPress, GetSkillButtonIndex(button));
    }
    public void ActionBtnPress(bool isPress, SkillButton button)
    {
        if (isPress == true)
            selectedSkillBtn = button;
        else
            selectedSkillBtn = null;
        UpdateActivateSkillButton(isPress);
    }
    public void SkillValueChange(ref Vector2 value, SkillJoyStick button)
    {
        if (OnSkillBtnDrag != null)
            OnSkillBtnDrag(value);
    }
    void UpdateActivateSkillButton(bool isPress)
    {
        if (isPress)
        {
            for (int i = 0; i < skillButton.Length; i++)
            {
                if (selectedSkillBtn != skillButton[i])
                    skillButton[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < skillButton.Length; i++)
                skillButton[i].SetActive(true);
        }
    }
    int GetSkillButtonIndex(SkillButton btn)
    {
        for (int i = 0; i < skillJoyStick.Length; i++)
        {
            if (btn == skillJoyStick[i])
                return i;
        }
        return -1;
    }
}
