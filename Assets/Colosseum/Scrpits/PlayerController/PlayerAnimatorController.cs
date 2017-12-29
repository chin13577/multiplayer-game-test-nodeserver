using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimatorController : MonoBehaviour
{
    Action animCallback;
    public Animator anim;
    public AnimatorOverrideController runtimeAnimator;
    private void Start()
    {
        runtimeAnimator = Instantiate(runtimeAnimator);
        anim.runtimeAnimatorController = runtimeAnimator;
    }
    public void UpdateAnimation(string name, object args = null, System.Action callback = null)
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
                anim.CrossFade("Rolling", 0.1f);
                break;
            case "Casting":
                SkillData skillData = SkillFactory.Instance.GetSkillData((string)args);
                runtimeAnimator["Attack"] = skillData.GetAnimation();
                runtimeAnimator["Attack"].AddEvent(new AnimationEvent()
                {
                    time = skillData.eventTime,
                    functionName = "AnimationCallback",
                    messageOptions = SendMessageOptions.DontRequireReceiver
                   
                });
                animCallback = callback;
                anim.CrossFade("Attack", 0.15f);
                break;
        }
    }
    void AnimationCallback()
    {
        if(animCallback!=null)
        {
            animCallback();
            animCallback = null;
        }
    }
}
