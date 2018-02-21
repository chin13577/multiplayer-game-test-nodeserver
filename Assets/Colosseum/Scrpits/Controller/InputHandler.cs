using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public NormalActionButton rollBtn;
    public MovementJoyStick movementStick;
    public ActionJoyStick[] actionStick;
    public CancelButton cancelBtn;

    private ActionJoyStick selectedSkillBtn;
    private List<IControllable> controlList;
    public static InputHandler _instance = null;

    public static InputHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InputHandler>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
  
    public void AddController(IControllable controllable)
    {
        if (controlList == null)
            controlList = new List<IControllable>();
        controlList.Add(controllable);
    }
    public bool RemoveController(IControllable controllable)
    {
        return controlList.Remove(controllable);
    }
    public void MovementStickPress(bool isPress, Vector2 pos)
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            controlList[i].OnMovementBtnPress(isPress, pos);
            controlList[i].OnMovementBtnDrag(pos);
        }
    }
    public void MovementStickDrag(Vector2 pos)
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            controlList[i].OnMovementBtnDrag(pos);
        }
    }
    public void OnRollBtnPress(bool isPress, ActionJoyStick button)
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            controlList[i].OnRollBtnPress(isPress, cancelBtn.IsCancelSkill(), button);
        }
        cancelBtn.ActivateCancelBtn(isPress, button);
    }
    public void ActionBtnPress(bool isPress, ActionJoyStick button)
    {
        if (isPress == true)
            selectedSkillBtn = button;
        else
            selectedSkillBtn = null;
        UpdateActivateSkillButton(isPress);

        for (int i = 0; i < controlList.Count; i++)
        {
            controlList[i].OnSkillBtnPress(isPress, cancelBtn.IsCancelSkill(), button);
            controlList[i].OnActionBtnDrag(button.pos);
        }

        cancelBtn.ActivateCancelBtn(isPress, button);
    }
    public void ActionBtnDrag(ref Vector2 value)
    {
        for (int i = 0; i < controlList.Count; i++)
        {
            controlList[i].OnActionBtnDrag(value);
        }
    }
    void UpdateActivateSkillButton(bool isPress)
    {
        if (isPress)
        {
            for (int i = 0; i < actionStick.Length; i++)
            {
                if (selectedSkillBtn != actionStick[i])
                    actionStick[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < actionStick.Length; i++)
                actionStick[i].SetActive(true);
        }
    }
}
