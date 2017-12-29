using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    private static SkillFactory instance = null;

    public static SkillFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillFactory>();
                return instance;
            }
            else
                return instance;
        }
    }
    public SkillCreator skillCreator;
    public enum SkillName { Fireball, Lightning, Frost }
    public SkillList skills;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        skills = Instantiate(skills);
    }
    public GameObject GetSkillObject(SkillName skillName)
    {
        return skillCreator.GenerateObject((int)skillName);
    }
    public void RecycleObject(Skill skill)
    {
        skillCreator.Destroy(skill);
    }
    public SkillData GetSkillData(SkillName skillName)
    {
        foreach (SkillData data in skills.skillList)
        {
            if (data.skillName == skillName)
            {
                return data;
            }
        }
        return null;
    }
    public SkillData GetSkillData(string skillName)
    {
        foreach (SkillData data in skills.skillList)
        {
            if (data.skillName.ToString() == skillName)
            {
                return data;
            }
        }
        return null;
    }
}
