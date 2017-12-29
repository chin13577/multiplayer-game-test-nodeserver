using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CancelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static System.Action<bool> OnCancelSkill;
    public Image image;
    Tweener tween;
    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        image.transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        InputHandler.OnSkillBtnPress += Callback_OnSkillBtnPress;
        InputHandler.OnRollBtnPress += Callback_OnRollBtnPress;
    }
    private void OnDisable()
    {
        InputHandler.OnSkillBtnPress -= Callback_OnSkillBtnPress;
        InputHandler.OnRollBtnPress -= Callback_OnRollBtnPress;
    }

    private void Callback_OnRollBtnPress(bool obj, ActionJoyStick button)
    {
        if (button.isCooldown) return;
        if (obj == true)
            ShowButton();
        else
            HideButton();
    }

    private void Callback_OnSkillBtnPress(bool arg1, ActionJoyStick button)
    {
        if (button.isCooldown) return;
        if (arg1 == true)
            ShowButton();
        else
            HideButton();
    }
    void ShowButton()
    {
        if (tween != null) DOTween.Kill(tween);
        tween = image.transform.DOScale(1, 0.2f);
    }
    void HideButton()
    {
        if (tween != null) DOTween.Kill(tween);
        tween = image.transform.DOScale(0, 0.2f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnCancelSkill != null)
            OnCancelSkill(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnCancelSkill != null)
            OnCancelSkill(false);
    }
}
