using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    Tweener tween;
    bool isCancel;
    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        image.transform.localScale = Vector3.zero;
    }

    public bool IsCancelSkill()
    {
        return isCancel;
    }
    public void ActivateCancelBtn(bool isPress, ActionJoyStick button)
    {
        if (isPress == true && button.isCooldown == false)
            ShowButton();
        else
            HideButton();
    }
    private void Callback_OnActionBtnPress(bool obj, ActionJoyStick button)
    {
        if (obj == true && button.isCooldown==false)
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
        isCancel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCancel = false;
    }
}
