using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillList")]
public class SkillList : ScriptableObject
{
    public List<SkillData> skillList;
}
