﻿using System.Collections;
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

    IEnumerator actionCo;
    void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        Player.OnPlayerCreated += OnPlayerCreated;
        InputHandler.instance.OnSkillBtnPress += Callback_OnSkillBtnPress;
        InputHandler.instance.OnActionBtnDrag += Callback_OnSkillBtnDrag;
        InputHandler.instance.rollBtn.OnPress += Callback_OnRollBtnPress;
    }
    private void OnDisable()
    {
        Player.OnPlayerCreated -= OnPlayerCreated;
        InputHandler.instance.OnSkillBtnPress -= Callback_OnSkillBtnPress;
        InputHandler.instance.OnActionBtnDrag -= Callback_OnSkillBtnDrag;
        InputHandler.instance.rollBtn.OnPress -= Callback_OnRollBtnPress;
    }
    void OnPlayerCreated(Player playerController)
    {
        if (playerController.isLocalPlayer)
        {
            player = playerController;
        }
    }
    void Callback_OnSkillBtnPress(bool isPress,int buttonIndex)
    {
        if (buttonIndex == -1) return;
        if (isPress)
        {
            currentSkill = player.skill[buttonIndex];
            ShowSkillGuide(currentSkill);
            
        }
        else
        {
            player.UseSkill(currentSkill.skillName, currentSkillTransform);
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
    void Callback_OnRollBtnPress(bool isPress)
    {
        if (isPress)
        {
            // show trails
        }
        else
        {
            //Roll
            player.Roll();
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
