﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SkillData
{
    public enum SkillType { None, Single, AOE, Area };
    public SkillType skillType;
    public SkillFactory.SkillName skillName;
    public float size;
    public float distance;
    public bool isSelectArea;
    public float coolDown;
    public float eventTime;
    [SerializeField] AnimationClip castAnim;
    public SkillData(SkillType type)
    {
        this.skillType = type;
        if (type == SkillType.AOE || type == SkillType.Single)
        {
            isSelectArea = true;
        }
    }
    public AnimationClip GetAnimation()
    {
        return castAnim;
    }
}
