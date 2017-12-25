using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Skill
{
    public override void Action(Transform initTransform)
    {
        transform.position = initTransform.position;
        Quaternion quaternion = Quaternion.FromToRotation(transform.forward, initTransform.forward);
        transform.rotation = quaternion;
        StartCoroutine(Duration(0.5f));
    }

    public override void ResetValue()
    {
        gameObject.SetActive(false);
        transform.parent = SkillFactory.Instance.transform;
        transform.localScale = new Vector3(1, 1, skillData.distance);
        StopAllCoroutines();
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
    IEnumerator Duration(float lifeTime)
    {
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }
        base.DestroyObject();
    }

}
