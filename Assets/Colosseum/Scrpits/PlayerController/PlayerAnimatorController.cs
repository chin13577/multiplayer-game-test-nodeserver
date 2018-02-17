using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator anim;
    public AnimatorOverrideController runtimeAnimator;

    WSGameManager manager;
    Action animCallback;
    private void Start()
    {
        runtimeAnimator = Instantiate(runtimeAnimator);
        anim.runtimeAnimatorController = runtimeAnimator;
        manager = WSGameManager.instance;
    }
    public void UpdateAnimation(string name, object args = null, System.Action callback = null)
    {
        switch (name)
        {
            case "Speed":
                anim.SetFloat("Speed", Convert.ToInt32(args));
                break;
            case "IsHit":
                anim.SetTrigger("IsHit");
                break;
            case "IsRolling":
                runtimeAnimator["Rolling"].AddEvent(new AnimationEvent()
                {
                    time = Convert.ToSingle(args),
                    functionName = "AnimationCallback",
                    messageOptions = SendMessageOptions.DontRequireReceiver

                });
                animCallback = callback;
                anim.Play("Rolling", 0, 0);
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
                anim.Play("Attack",0,0);
                break;
        }
    }
    public void SendAnimToServer(string name, object args = null)
    {
        AnimationJson animJson = new AnimationJson();
        animJson.name = name;
        animJson.args = args;
        manager.SendAnimation(animJson);
    }
    void AnimationCallback()
    {
        if (animCallback != null)
        {
            animCallback();
            animCallback = null;
        }
    }
}
