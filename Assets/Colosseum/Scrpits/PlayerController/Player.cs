using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    public enum PlayerState { Idle, Rolling, Casting, Hit }
    [SerializeField] private PlayerState _playerState = PlayerState.Idle;
    public PlayerState playerState
    {
        get
        {
            return _playerState;
        }
        set
        {
            _playerState = value;
            if (OnUpdatePlayerState != null)
                OnUpdatePlayerState(value);
        }
    }

    public delegate void onPlayerCreated(Player playerController);
    public static event onPlayerCreated OnPlayerCreated;
    public Action<PlayerState> OnUpdatePlayerState;

    InputHandler controller;
    public bool isLocal;
    public PlayerAnimatorController animController;
    public LayerMask layer;
    public Transform atkSpawnPoint;
    #region Variables
    public PlayerJson playerData;
    public SkillData[] skill;
    Vector3 direction;
    Vector3 velocity;
    Vector3 oldPosition;
    float speed { get { return runSpeed + rollSpeed + knockBackSpeed; } }
    float runSpeed;
    float rollSpeed;
    float knockBackSpeed;
    Vector2 currentMoveDir;

    CharacterController c_controller;
    bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, 0.3f, layer);
        }
    }

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
        // mork up
        for (int i = 0; i < skill.Length; i++)
        {
            skill[i] = SkillFactory.Instance.GetSkillData(skill[i].skillName);
        }
    }
    private void OnDisable()
    {
        if (isLocal == false)
            return;
        InputHandler.OnMovementBtnDrag -= Callback_OnJoyStickValueChange;
    }
    private void Update()
    {
        if (isLocal == false) { return; }

        if (velocity.y <= 0 && isGrounded)
        {
            velocity.y = 0;
            if (playerState != PlayerState.Hit)
                velocity.Set(transform.forward.x, velocity.y, transform.forward.z);
        }
        else
            velocity = velocity + Physics.gravity * Time.deltaTime;
        c_controller.Move(velocity * Time.deltaTime * speed);
    }

    #endregion
    public void Callback_OnJoyStickValueChange(Vector2 vect)
    {
        currentMoveDir = vect;
        if (playerState != PlayerState.Idle && playerState != PlayerState.Rolling)
        {
            runSpeed = 0;
            return;
        }
        if (vect != Vector2.zero)
        {
            RotateCharacter(new Vector3(vect.x, 0, vect.y));
            animController.UpdateAnimation("Speed", 1);
            animController.SendAnimToServer("Speed", 1);
            runSpeed = 3;
        }
        else
        {
            animController.UpdateAnimation("Speed", 0);
            animController.SendAnimToServer("Speed", 0);
            runSpeed = 0;
        }
        if (playerState == PlayerState.Rolling)
            runSpeed = 0;
    }
    void RotateCharacter(Vector3 dir)
    {
        Quaternion quaternion = Quaternion.LookRotation(dir);
        transform.rotation = quaternion;
    }
    public void SetIsLocal()
    {
        isLocal = true;
        InputHandler.OnMovementBtnDrag += Callback_OnJoyStickValueChange;
        if (OnPlayerCreated != null)
        {
            OnPlayerCreated(this);
        }
    }
    public void UseSkill(SkillData skillData, Transform currentSkillTransform, Vector3 attackDir)
    {
        playerState = PlayerState.Casting;
        animController.UpdateAnimation("Speed", 0);
        animController.SendAnimToServer("Speed", 0);
        runSpeed = 0;
        rollSpeed = 0;
        RotateCharacter(attackDir);
        // cast anim
        animController.UpdateAnimation("Casting", skillData.skillName.ToString(), () =>
        {
            playerState = PlayerState.Idle;
            Callback_OnJoyStickValueChange(currentMoveDir);
            // Initial skill
            SpawnSkill(skillData, currentSkillTransform);
        });
        animController.SendAnimToServer("Casting", skillData.skillName.ToString());

    }
    /// <summary>
    /// Spawn Skill
    /// </summary>
    /// <param name="skillData"></param>
    /// <param name="currentSkillTransform"></param>
    void SpawnSkill(SkillData skillData, Transform currentSkillTransform)
    {
        GameObject g = SkillFactory.Instance.GetSkillObject(skillData.skillName);
        if (g != null)
        {
            Skill obj = g.GetComponent<Skill>();
            obj.owner = this.name;
            obj.Action(currentSkillTransform);
        }
    }
    public void Roll()
    {
        if (playerState == PlayerState.Hit) { return; }
        playerState = PlayerState.Rolling;
        runSpeed = 0;
        rollSpeed = 6;
        animController.SendAnimToServer("IsRolling", 0.3f);
        animController.UpdateAnimation("IsRolling", 0.3f, () =>
        {
            rollSpeed = 0;
            playerState = PlayerState.Idle;
            Callback_OnJoyStickValueChange(currentMoveDir);
        });
        //DOTween.To((x) => rollSpeed = x, 10, 0, 0.45f).OnComplete(() => playerState = PlayerState.Idle);
    }
    Tween knockbackTween;
    public void Knockback(Vector3 dir)
    {
        if (playerState == PlayerState.Rolling) { return; }
        playerState = PlayerState.Hit;
        animController.UpdateAnimation("IsHit");
        animController.SendAnimToServer("IsHit");
        velocity.Set(dir.x, velocity.y, dir.z);
        if (knockbackTween != null)
            knockbackTween.Kill();
        knockbackTween = DOTween.To((x) => knockBackSpeed = x, 10, 0, 0.5f).OnComplete(() => playerState = PlayerState.Idle);
    }


}
