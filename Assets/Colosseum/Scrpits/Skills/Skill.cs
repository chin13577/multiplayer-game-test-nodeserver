using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector]
    public SkillData skillData;
    public string id = "";
    public string owner = "";
    public void SetData(SkillData data)
    {
        this.skillData = data;
    }

    public abstract void ResetValue();
    public abstract void Action(Vector3 position,Quaternion direction);
    public void DestroyObject()
    {
        // send data to server.

        SkillFactory.Instance.RecycleObject(this);
    }
}
