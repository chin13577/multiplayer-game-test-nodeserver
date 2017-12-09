using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{

    public Animator anim;
    public void UpdateAnimation(string name, object args = null)
    {
        switch (name)
        {
            case "Speed":
                anim.SetFloat("Speed", (int)args);
                break;
            case "IsHit":
                anim.SetTrigger("IsHit");
                break;
            case "IsRolling":
                anim.SetTrigger("IsRolling");
                break;
        }
    }
}
