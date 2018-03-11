using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : Skill
{
    public override void EnterState(Vector3 position, Vector3 direction)
    {
        transform.parent = null;
        transform.position = position;
        transform.localScale = new Vector3(skillData.size, transform.localScale.y, skillData.size);
        Quaternion quaternion = Quaternion.LookRotation(direction);
        transform.rotation = quaternion;
    }
    public override void UpdateState(Vector3 position, Vector3 direction)
    {
    }
    public override void ResetValue()
    {
        gameObject.SetActive(false);
        transform.parent = SkillFactory.Instance.transform;
        transform.rotation = Quaternion.identity;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != owner)
        {
            if (other.tag == "Player")
            {
                print("Frezz");
            }
        }
    }
}
