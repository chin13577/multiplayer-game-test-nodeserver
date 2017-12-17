using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerController : MonoBehaviour
{
    public delegate void onPlayerCreated(PlayerController playerController);
    public static event onPlayerCreated OnPlayerCreated;
    ControllerManager controller;
    public PlayerAnimatorController animController;
    public LayerMask layer;
    public Transform atkSpawnPoint;
    [Header("Skill")]
    public Transform skillOrigin;
    public Transform singleTargetTransform;
    public Transform aoeTransform;
    Transform currentSkillTransform;
    Skill currentSkill;
    #region Variables
    public bool isLocalPlayer;
    public Skill[] skill;
    Vector3 direction;
    Vector3 velocity;
    Vector3 oldPosition;
    float speed { get { return runSpeed + rollSpeed + knockBackSpeed; } }
    float runSpeed;
    float rollSpeed;
    float knockBackSpeed;

    CharacterController c_controller;
    bool isCanMove = false;
    bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, 0.3f, layer);
        }
    }
    bool isRolling;
    bool isKnockback;

    #endregion
    #region Unity Callback
    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        c_controller = GetComponent<CharacterController>();
        velocity.Set(transform.forward.x, velocity.y, transform.forward.z);
        controller = ControllerManager.instance;
        // morkup skill.
        skill = new Skill[2] { new Skill(Skill.SkillType.Single), new Skill(Skill.SkillType.AOE) };
        skill[0].distance = 4f;
        skill[1].distance = 4f;
        if (isLocalPlayer && OnPlayerCreated != null)
        {
            OnPlayerCreated(this);
        }
    }
    private void OnEnable()
    {
        if (isLocalPlayer == false)
            return;
        controller.OnLeftJoyStickChange += Callback_OnJoyStickValueChange;
        controller.OnSkillBtnPress += Callback_OnSkillBtnPress;
        controller.OnSkillBtnDrag += Callback_OnSkillBtnDrag;
    }
    private void OnDisable()
    {
        if (isLocalPlayer == false)
            return;
        controller.OnLeftJoyStickChange -= Callback_OnJoyStickValueChange;
        controller.OnSkillBtnPress -= Callback_OnSkillBtnPress;
        controller.OnSkillBtnDrag -= Callback_OnSkillBtnDrag;
    }
    private void Update()
    {
        if (velocity.y <= 0 && isGrounded)
        {
            velocity.y = 0;
            if (isCanMove && !isKnockback)
                velocity.Set(transform.forward.x, velocity.y, transform.forward.z);
        }
        else
            velocity = velocity + Physics.gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
            animController.UpdateAnimation("IsHit");
        if (Input.GetKeyDown(KeyCode.S))
            Roll();
        c_controller.Move(velocity * Time.deltaTime * speed);

    }

    #endregion
    private void Callback_OnJoyStickValueChange(Vector2 vect)
    {
        if (vect != Vector2.zero)
        {
            RotateCharacter(new Vector3(vect.x, 0, vect.y));
            animController.UpdateAnimation("Speed", 1);
            isCanMove = true;
            runSpeed = 3;
        }
        else
        {
            animController.UpdateAnimation("Speed", 0);
            isCanMove = false;
            runSpeed = 0;
        }

    }
    void Callback_OnSkillBtnPress(bool isPress, int skill)
    {
        if (isPress)
        {
            currentSkill = this.skill[skill];
            if (currentSkill.skillType == Skill.SkillType.Single)
            {
                currentSkillTransform = singleTargetTransform;
                aoeTransform.gameObject.SetActive(false);
                singleTargetTransform.gameObject.SetActive(true);
                currentSkillTransform.localScale = new Vector3(1, 1, currentSkill.distance);
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
        }
    }
    void Callback_OnSkillBtnDrag(Vector2 value)
    {
        if (currentSkill == null) return;
        if (currentSkill.skillType == Skill.SkillType.AOE)
        {
            value = value * currentSkill.distance;
            currentSkillTransform.position = new Vector3(transform.position.x + value.x, transform.position.y, transform.position.z + value.y);
        }
        else if (currentSkill.skillType == Skill.SkillType.Single)
        {
            if (value == Vector2.zero) return;
            Vector3 dir = new Vector3(value.x, 0, value.y);
            Quaternion quaternion = Quaternion.LookRotation(dir);
            currentSkillTransform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            currentSkillTransform.rotation = quaternion;
        }
    }


    void RotateCharacter(Vector3 dir)
    {
        Quaternion quaternion = Quaternion.LookRotation(dir);
        transform.rotation = quaternion;
    }
    public void Attack()
    {
        if (isRolling || isKnockback) { return; }
        GameObject g = ObjectPoolManager.instance.GetObject();
        if (g != null)
        {
            g.transform.parent = null;
            g.transform.position = atkSpawnPoint.position;
            ObjectInPool obj = g.GetComponent<ObjectInPool>();
            obj.owner = this.name;
            obj.Action(transform.forward, 10f);
        }

    }
    public void Roll()
    {
        if (isRolling || isKnockback) { return; }
        isRolling = true;
        animController.UpdateAnimation("IsRolling");
        DOTween.To((x) => rollSpeed = x, 10, 0, 0.5f).OnComplete(() => isRolling = false);
    }
    Tween knockbackTween;
    public void Knockback(Vector3 dir)
    {
        if (isRolling) { return; }
        isKnockback = true;
        animController.UpdateAnimation("IsHit");
        velocity.Set(dir.x, velocity.y, dir.z);
        if (knockbackTween != null)
            knockbackTween.Kill();
        knockbackTween = DOTween.To((x) => knockBackSpeed = x, 10, 0, 0.5f).OnComplete(() => isKnockback = false);
    }


}
