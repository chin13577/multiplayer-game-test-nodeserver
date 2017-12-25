using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionJoyStick : MonoBehaviour
{
    public int buttonIndex;
    public bool isActivate;
    public void SetActive(bool isActive)
    {
        isActivate = isActive;
    }
}
