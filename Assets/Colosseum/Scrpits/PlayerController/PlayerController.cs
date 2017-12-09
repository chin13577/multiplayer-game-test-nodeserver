using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerController : MonoBehaviour
{
    public delegate void onAnimationChange(string name, object args = null);
    public event onAnimationChange OnAnimationChange;

    public TouchJoyStick joyStick;
    public PlayerAnimatorController animController;
    public LayerMask layer;

    #region Variables
    Vector3 direction;
    Vector3 velocity;
    Vector3 oldPosition;
    float speed { get { return runSpeed + rollSpeed; } }
    float runSpeed;
    float rollSpeed;

    CharacterController c_controller;
    float knockback = 0;
    bool isCanMove = false;
    bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, 0.3f, layer);
        }
    }
    bool isRolling;

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
    }
    private void OnEnable()
    {
        joyStick.OnValueChange += Callback_OnJoyStickValueChange;
    }
    private void OnDisable()
    {
        joyStick.OnValueChange -= Callback_OnJoyStickValueChange;
    }
    private void Update()
    {
        if (knockback > 0) { knockback -= Time.deltaTime; }
        if (velocity.y <= 0 && isGrounded)
        {
            velocity.y = 0;
            if (isCanMove)
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
    void RotateCharacter(Vector3 dir)
    {
        Quaternion quaternion = Quaternion.LookRotation(dir);
        transform.rotation = quaternion;
    }
    public void Attack()
    {

    }
    public void Roll()
    {
        if (isRolling) { return; }
        isRolling = true;
        animController.UpdateAnimation("IsRolling");
        DOTween.To((x) => rollSpeed = x, 10, 0, 0.5f).OnComplete(() => isRolling = false);
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody other = hit.collider.attachedRigidbody;
        if (other == null || other.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        other.velocity = pushDir * 10f;
    }

}
