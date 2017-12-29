using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActionJoyStick : MonoBehaviour
{
    public Image cooldownImage;
    public int buttonIndex;
    public bool isActivate;
    public bool isCooldown;
    public Vector2 pos;
    public virtual void Initial()
    {
        isCooldown = false;
        isActivate = true;
    }
    public void SetCoolDown(float duration)
    {
        isCooldown = true;
        DOTween.To((x) => cooldownImage.fillAmount = x, 0, 1, duration).OnComplete(() => { isCooldown = false;});
    }
    public void SetActive(bool isActive)
    {
        isActivate = isActive;
    }
}
