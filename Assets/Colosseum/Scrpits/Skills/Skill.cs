using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector]
    public SkillData skillData;
    public string owner = "";
    public void SetData(SkillData data)
    {
        this.skillData = data;
    }

    public abstract void ResetValue();
    public abstract void Action(Transform initTransform);
    public void DestroyObject()
    {
        // send data to server.

        SkillFactory.Instance.RecycleObject(this);
    }
}
