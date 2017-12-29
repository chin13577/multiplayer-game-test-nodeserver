using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlayerController : MonoBehaviour
{

    public static SkillPlayerController instance = null;

    [Header("Skill")]
    public Transform skillOrigin;
    public Transform singleTargetTransform;
    public Transform aoeTransform;
    public Transform normalAtkTransform;
    Transform currentSkillTransform;
    SkillData currentSkill;
    Player player;

    bool isCancelSkill;

    IEnumerator actionCo;
    void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        Player.OnPlayerCreated += OnPlayerCreated;
        InputHandler.OnSkillBtnPress += Callback_OnSkillBtnPress;
        InputHandler.OnActionBtnDrag += Callback_OnSkillBtnDrag;
        InputHandler.instance.rollBtn.OnPress += Callback_OnRollBtnPress;
        CancelButton.OnCancelSkill += Callback_OnCancelSkill;
    }


    private void OnDisable()
    {
        Player.OnPlayerCreated -= OnPlayerCreated;
        InputHandler.OnSkillBtnPress -= Callback_OnSkillBtnPress;
        InputHandler.OnActionBtnDrag -= Callback_OnSkillBtnDrag;
        InputHandler.instance.rollBtn.OnPress -= Callback_OnRollBtnPress;
        CancelButton.OnCancelSkill -= Callback_OnCancelSkill;
    }
    void OnPlayerCreated(Player playerController)
    {
        if (playerController.isLocalPlayer)
        {
            player = playerController;
        }
    }
    private void Callback_OnCancelSkill(bool isCancel)
    {
        isCancelSkill = isCancel;
    }
    void Callback_OnSkillBtnPress(bool isPress, ActionJoyStick button)
    {
        if (button.isCooldown == true) return;
        if (button.buttonIndex == -1) return;
        //if (player.playerState == Player.PlayerState.Rolling) return;

        if (isPress)
        {
            currentSkill = player.skill[button.buttonIndex];
            ShowSkillGuide(currentSkill);
        }
        else
        {
            if (currentSkill == null) return;
            if (isCancelSkill == false)
            {
                player.UseSkill(currentSkill.skillName, currentSkillTransform);
                //cooldown
                button.SetCoolDown(currentSkill.coolDown);
            }
            //reset
            HideSkillGuide();

        }
    }
    void ShowSkillGuide(SkillData skill)
    {
        if (skill.skillType == SkillData.SkillType.Single)
        {
            currentSkillTransform = singleTargetTransform;
            aoeTransform.gameObject.SetActive(false);
            singleTargetTransform.gameObject.SetActive(true);
            currentSkillTransform.localScale = new Vector3(1, 1, skill.distance);
            Quaternion quaternion = Quaternion.LookRotation(player.transform.forward);
            currentSkillTransform.rotation = quaternion;
        }
        else if (skill.skillType == SkillData.SkillType.AOE)
        {
            currentSkillTransform = aoeTransform;
            aoeTransform.gameObject.SetActive(true);
            singleTargetTransform.gameObject.SetActive(false);
        }
        StartCoroutine(actionCo = UpdateMagicRingTransform());
    }
    void HideSkillGuide()
    {
        currentSkill = null;
        currentSkillTransform = null;
        aoeTransform.gameObject.SetActive(false);
        singleTargetTransform.gameObject.SetActive(false);
        StopCoroutine(actionCo);
    }
    void Callback_OnSkillBtnDrag(Vector2 value)
    {
        if (currentSkill == null) return;
        if (currentSkill.skillType == SkillData.SkillType.AOE)
        {
            if (value == Vector2.zero) return;
            value = value * currentSkill.distance;
            currentSkillTransform.position = new Vector3(player.transform.position.x + value.x, player.transform.position.y, player.transform.position.z + value.y);
        }
        else if (currentSkill.skillType == SkillData.SkillType.Single)
        {
            if (value == Vector2.zero) return;
            Vector3 dir = new Vector3(value.x, 0, value.y);
            Quaternion quaternion = Quaternion.LookRotation(dir);
            currentSkillTransform.rotation = quaternion;
            currentSkillTransform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
    }
    bool isPressRollBtnSuccess;
    void Callback_OnRollBtnPress(bool isPress, ActionJoyStick button)
    {
        if (button.isCooldown) return;
        if (isPress)
        {
            isPressRollBtnSuccess = true;
        }
        else
        {
            if (isPressRollBtnSuccess == false) return;
            //Roll
            if (isCancelSkill == false)
            {
                player.Roll();
                InputHandler.instance.rollBtn.SetCoolDown(0.5f);
            }
            isPressRollBtnSuccess = false;
        }
    }
    IEnumerator UpdateMagicRingTransform()
    {
        while (true)
        {
            skillOrigin.position = player.transform.position;
            yield return null;
        }
    }
}
