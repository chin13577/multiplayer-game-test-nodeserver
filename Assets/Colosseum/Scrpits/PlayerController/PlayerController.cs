using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TouchJoyStick joyStick;
    public Animator anim;
    public LayerMask layer;

    #region Variables
    Vector3 direction;
    Vector3 velocity;
    [SerializeField] float speed;

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
    bool canJump = true;
    bool isJumping = false;

    #endregion
    #region Unity Callback
    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        c_controller = GetComponent<CharacterController>();
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
            isJumping = false;
            if (isCanMove)
                velocity.Set(transform.forward.x, velocity.y, transform.forward.z);
            else
                velocity = Vector3.zero;
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
            anim.SetFloat("Speed", 1);
            isCanMove = true;
        }
        else
        {
            anim.SetFloat("Speed", 0);
            isCanMove = false;
        }
    }
    void RotateCharacter(Vector3 dir)
    {
        Quaternion quaternion = Quaternion.LookRotation(dir);
        transform.rotation = quaternion;
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
