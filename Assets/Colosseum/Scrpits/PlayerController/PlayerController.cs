using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IControllable
{
    GameObject skillPointPrefab;
    Transform skillOrigin;
    Transform aoeTransform;
    Transform singleTargetTransform;
    Transform currentSkillTransform;
    SkillData currentSkill;
    Player player;
    Vector3 attackDir;
    bool isPressRollBtnSuccess;

    IEnumerator actionCo;

    void Awake()
    {
        player = GetComponent<Player>();
        InputHandler.instance.AddController(this);
    }
    void Start()
    {
        skillOrigin = Instantiate(WSGameManager.instance.skillPointPrefab).transform;
        aoeTransform = skillOrigin.GetChild(0);
        singleTargetTransform = skillOrigin.GetChild(1);
        aoeTransform.gameObject.SetActive(false);
        singleTargetTransform.gameObject.SetActive(false);
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
    IEnumerator UpdateMagicRingTransform()
    {
        while (true)
        {
            skillOrigin.position = player.transform.position;
            yield return null;
        }
    }

    public void OnMovementBtnPress(bool isPress, Vector2 pos)
    {
        if (isPress)
        {
            player.Moving();
        }
        else
        {
            player.StopMoving();
        }
    }

    public void OnMovementBtnDrag(Vector2 pos)
    {
        player.RotatePlayer(pos);
    }

    public void OnActionBtnDrag(Vector2 pos)
    {
        if (currentSkill == null) return;
        if (currentSkill.skillType == SkillData.SkillType.AOE)
        {
            if (pos == Vector2.zero) return;
            pos = pos * currentSkill.distance;
            currentSkillTransform.localScale = Vector3.one * currentSkill.size;
            currentSkillTransform.position = new Vector3(player.transform.position.x + pos.x, player.transform.position.y, player.transform.position.z + pos.y);
        }
        else if (currentSkill.skillType == SkillData.SkillType.Single)
        {
            if (pos == Vector2.zero) return;
            Vector3 dir = new Vector3(pos.x, 0, pos.y);
            Quaternion quaternion = Quaternion.LookRotation(dir);
            currentSkillTransform.rotation = quaternion;
            currentSkillTransform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
        attackDir = pos;
    }

    public void OnSkillBtnPress(bool isPress, bool isCancelSkill, ActionJoyStick button)
    {
        if (button.isCooldown == true) return;
        if (button.buttonIndex == -1) return;
        if (player.playerState == Player.PlayerState.Casting || player.playerState == Player.PlayerState.Casting ||
            player.playerState == Player.PlayerState.Hit)
        {
            return;
        }

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
                player.UseSkill(currentSkill, currentSkillTransform, attackDir);
                //cooldown
                button.SetCoolDown(currentSkill.coolDown);
            }
            //reset
            HideSkillGuide();

        }
    }

    public void OnRollBtnPress(bool isPress, bool isCancelSkill, ActionJoyStick button)
    {
        if (button.isCooldown) return;
        if(!isPress && !isCancelSkill)
        {
            player.Roll();
            button.SetCoolDown(0.5f);
        }
    }
}
