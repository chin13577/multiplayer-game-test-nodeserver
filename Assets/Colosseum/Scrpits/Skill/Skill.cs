using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public enum SkillType { None, Single, AOE, Area };
    public SkillType skillType;
    public float distance;
    public bool isSelectArea;
    public Skill(SkillType type)
    {
        this.skillType = type;
        if (type == SkillType.AOE || type == SkillType.Single)
        {
            isSelectArea = true;
        }
    }
}
