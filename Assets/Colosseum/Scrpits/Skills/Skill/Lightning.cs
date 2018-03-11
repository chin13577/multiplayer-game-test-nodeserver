using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Skill
{
    public override void EnterState(Vector3 position, Vector3 direction)
    {
        transform.parent = null;
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    public override void UpdateState(Vector3 position, Vector3 direction)
    {
    }
    public override void ResetValue()
    {
        gameObject.SetActive(false);
        transform.parent = SkillFactory.Instance.transform;
        transform.localScale = new Vector3(1, 1, skillData.distance);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != owner)
        {
            if (other.tag == "Player")
            {
                print("stun");
            }
        }
    }

}
