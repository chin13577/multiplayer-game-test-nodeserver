using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public static SkillManager instance = null;

    [Header("Skill")]
    public Transform skillOrigin;
    public Transform singleTargetTransform;
    public Transform aoeTransform;
    public Transform normalAtkTransform;
    Transform currentSkillTransform;
    Skill currentSkill;
    PlayerController player;

    IEnumerator actionCo;
    void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerCreated += OnPlayerCreated;
        ControllerManager.instance.OnSkillBtnPress += Callback_OnSkillBtnPress;
        ControllerManager.instance.OnSkillBtnDrag += Callback_OnSkillBtnDrag;
        ControllerManager.instance.atkBtn.OnPress += Callback_OnAttackBtnPress;
        ControllerManager.instance.rollBtn.OnPress += Callback_OnRollBtnPress;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerCreated -= OnPlayerCreated;
        ControllerManager.instance.OnSkillBtnPress -= Callback_OnSkillBtnPress;
        ControllerManager.instance.OnSkillBtnDrag -= Callback_OnSkillBtnDrag;
        ControllerManager.instance.atkBtn.OnPress -= Callback_OnAttackBtnPress;
        ControllerManager.instance.rollBtn.OnPress -= Callback_OnRollBtnPress;
    }
    void OnPlayerCreated(PlayerController playerController)
    {
        if (playerController.isLocalPlayer)
        {
            player = playerController;
        }
    }
    void Callback_OnSkillBtnPress(bool isPress, int skill)
    {
        if (isPress)
        {
            currentSkill = player.skill[skill];
            StartCoroutine(actionCo = UpdateMagicRingTransform());
            if (currentSkill.skillType == Skill.SkillType.Single)
            {
                currentSkillTransform = singleTargetTransform;
                aoeTransform.gameObject.SetActive(false);
                singleTargetTransform.gameObject.SetActive(true);
                currentSkillTransform.localScale = new Vector3(1, 1, currentSkill.distance);
                Quaternion quaternion = Quaternion.LookRotation(player.transform.forward);
                currentSkillTransform.rotation = quaternion;
            }
            else if (currentSkill.skillType == Skill.SkillType.AOE)
            {
                currentSkillTransform = aoeTransform;
                aoeTransform.gameObject.SetActive(true);
                singleTargetTransform.gameObject.SetActive(false);
            }
        }
        else
        {
            //reset
            currentSkill = null;
            currentSkillTransform = null;
            aoeTransform.gameObject.SetActive(false);
            singleTargetTransform.gameObject.SetActive(false);
            StopCoroutine(actionCo);
        }
    }
    void Callback_OnSkillBtnDrag(Vector2 value)
    {
        if (currentSkill == null) return;
        if (currentSkill.skillType == Skill.SkillType.AOE)
        {
            value = value * currentSkill.distance;
            currentSkillTransform.position = new Vector3(player.transform.position.x + value.x, player.transform.position.y, player.transform.position.z + value.y);
        }
        else if (currentSkill.skillType == Skill.SkillType.Single)
        {
            if (value == Vector2.zero) return;
            Vector3 dir = new Vector3(value.x, 0, value.y);
            Quaternion quaternion = Quaternion.LookRotation(dir);
            currentSkillTransform.rotation = quaternion;
            currentSkillTransform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }
    }
    void Callback_OnAttackBtnPress(bool isPress)
    {
        if (isPress)
        {
            // show trails
            normalAtkTransform.gameObject.SetActive(true);
            StartCoroutine(actionCo = UpdateAtkTransform());
        }
        else
        {
            //attack
            normalAtkTransform.gameObject.SetActive(false);
            player.Attack();
            StopCoroutine(actionCo);
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
    IEnumerator UpdateAtkTransform()
    {
        while (true)
        {
            skillOrigin.position = player.transform.position;
            skillOrigin.rotation = player.transform.rotation;
            yield return null;
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
