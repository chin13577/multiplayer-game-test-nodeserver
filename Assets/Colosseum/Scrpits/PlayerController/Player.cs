using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    public delegate void onPlayerCreated(Player playerController);
    public static event onPlayerCreated OnPlayerCreated;
    InputHandler controller;
    public PlayerAnimatorController animController;
    public LayerMask layer;
    public Transform atkSpawnPoint;
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
        controller = InputHandler.instance;
        // morkup skill.
        skill = new Skill[3] { new Skill(Skill.SkillType.Single),new Skill(Skill.SkillType.Single), new Skill(Skill.SkillType.AOE) };
        skill[0].distance = 3f;
        skill[1].distance = 3f;
        skill[2].distance = 3.5f;
    }
    private void Start()
    {
        if (isLocalPlayer && OnPlayerCreated != null)
        {
            OnPlayerCreated(this);
        }
    }
    private void OnEnable()
    {
        if (isLocalPlayer == false)
            return;
        controller.OnMovementBtnDrag += Callback_OnJoyStickValueChange;
    }
    private void OnDisable()
    {
        if (isLocalPlayer == false)
            return;
        controller.OnMovementBtnDrag -= Callback_OnJoyStickValueChange;
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
