using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMovementBtnDrag;
    public static event Action<Vector2> OnActionBtnDrag;
    public static event Action<bool, ActionJoyStick> OnSkillBtnPress;
    public static event Action<bool, ActionJoyStick> OnRollBtnPress;
    public NormalActionButton rollBtn;
    public MovementJoyStick movementStick;
    public ActionJoyStick[] actionStick;

    private ActionJoyStick selectedSkillBtn;
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
    Player player;
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
        Player.OnPlayerCreated += OnPlayerCreated;
        movementStick.OnValueChange += Callback_OnMovementStickChange;
        rollBtn.OnPress += Callback_OnRollBtnPress;
    }

    private void OnDisable()
    {
        Player.OnPlayerCreated -= OnPlayerCreated;
        movementStick.OnValueChange -= Callback_OnMovementStickChange;
        rollBtn.OnPress -= Callback_OnRollBtnPress;
    }
    private void Callback_OnRollBtnPress(bool press, ActionJoyStick button)
    {
        if (OnRollBtnPress != null)
            OnRollBtnPress(press, button);
    }

    void OnPlayerCreated(Player playerController)
    {
        if (playerController.isLocalPlayer)
        {
            this.player = playerController;
        }
    }
    void Callback_OnMovementStickChange(Vector2 pos)
    {
        if (OnMovementBtnDrag != null)
            OnMovementBtnDrag(pos);
    }
    public void ActionBtnPress(bool isPress, ActionJoyStick button)
    {
        if (isPress == true)
            selectedSkillBtn = button;
        else
            selectedSkillBtn = null;
        UpdateActivateSkillButton(isPress);
        if (OnSkillBtnPress != null)
            OnSkillBtnPress(isPress, button);
        ActionBtnDrag(ref button.pos);
    }
    public void ActionBtnDrag(ref Vector2 value)
    {
        if (OnActionBtnDrag != null)
            OnActionBtnDrag(value);
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
