using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCreator : MonoBehaviour
{
    SkillFactory factory;
    public Skill[] skillPrefabs;
    public int amount;
    public bool autoIncrement;
    public List<Queue<Skill>> skillContainer = new List<Queue<Skill>>();

    private void Awake()
    {
        factory = SkillFactory.Instance;
        Initialize();
    }
    //Initializes the game for each level.
    public void Initialize()
    {
        for (int i = 0; i < Enum.GetNames(typeof(SkillFactory.SkillName)).Length; i++)
        {
            skillContainer.Add(new Queue<Skill>());
            for (int j = 0; j < amount; j++)
            {
                Skill obj = Instantiate(skillPrefabs[i], transform.position, Quaternion.identity);
                obj.skillData = factory.skills.skillList[i];
                obj.ResetValue();
                skillContainer[i].Enqueue(obj);
            }
        }
    }

    public GameObject GenerateObject(int skillId)
    {
        if (skillContainer[skillId].Count > 0)
        {
            Skill g = skillContainer[skillId].Dequeue();
            g.gameObject.SetActive(true);
            return g.gameObject;
        }
        else if (autoIncrement)
        {
            Skill obj = Instantiate(skillPrefabs[skillId], transform.position, Quaternion.identity);
            obj.skillData = factory.skills.skillList[skillId];
            obj.ResetValue();
            return obj.gameObject;
        }
        return null;
    }
    public void Destroy(Skill obj)
    {
        obj.ResetValue();
        obj.transform.SetParent(this.transform);
        obj.gameObject.SetActive(false);
        skillContainer[(int)obj.skillData.skillName].Enqueue(obj);
    }

    private void OnDestroy()
    {
        skillContainer.Clear();
    }



}
