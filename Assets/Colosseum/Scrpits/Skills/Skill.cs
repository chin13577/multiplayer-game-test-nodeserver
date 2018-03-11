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
    public abstract void EnterState(Vector3 position, Vector3 direction);
    public abstract void UpdateState(Vector3 position, Vector3 direction);
    public void DestroyObject()
    {
        SkillFactory.Instance.RecycleObject(this);
    }
}
