using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void OnMovementBtnPress(bool isPress, Vector2 pos);
    void OnMovementBtnDrag(Vector2 pos);
    void OnActionBtnDrag(Vector2 pos);
    void OnSkillBtnPress(bool isPress, ActionJoyStick button);
    void OnRollBtnPress(bool isPress, ActionJoyStick button);
}
