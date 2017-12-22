using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionJoyStick : MonoBehaviour
{
    public enum ActionButton { Roll = -1, Attack, Skill1, Skill2 }
    public ActionButton buttonType;
    public bool isActivate;
    public void SetActive(bool isActive)
    {
        isActivate = isActive;
    }
}
