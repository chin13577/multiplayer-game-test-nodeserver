using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //public static event Action<bool,Vector2> OnMovementBtnPress;
    //public static event Action<Vector2> OnMovementBtnDrag;
    //public static event Action<Vector2> OnActionBtnDrag;
    //public static event Action<bool, ActionJoyStick> OnSkillBtnPress;
    //public static event Action<bool, ActionJoyStick> OnRollBtnPress;
    public static event Action<bool, ActionJoyStick> OnActionBtnPress;
    public NormalActionButton rollBtn;
    public MovementJoyStick movementStick;
    public ActionJoyStick[] actionStick;

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
    //Player player;
    //Awake is always called before any Start functions
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    //private void OnEnable()
    //{
    //    Player.OnPlayerCreated += OnPlayerCreated;
    //    rollBtn.OnPress += Callback_OnRollBtnPress;
    //}

    //private void OnDisable()
    //{
    //    Player.OnPlayerCreated -= OnPlayerCreated;
    //    rollBtn.OnPress -= Callback_OnRollBtnPress;
    //}

    //void OnPlayerCreated(Player playerController)
    //{
    //    if (playerController.isLocal)
    //    {
    //        this.player = playerController;
    //    }
    //}

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
            controlList[i].OnRollBtnPress(isPress, button);
        }
        if (OnActionBtnPress != null)
            OnActionBtnPress(isPress, button);
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
            controlList[i].OnSkillBtnPress(isPress, button);
            controlList[i].OnActionBtnDrag( button.pos);
        }
        print(isPress);
        if (OnActionBtnPress != null)
            OnActionBtnPress(isPress, button);
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
