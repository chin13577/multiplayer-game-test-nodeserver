using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : Skill
{
    public override void Action(Transform initTransform)
    {
        transform.parent = null;
        transform.position = initTransform.position;
        Quaternion quaternion = Quaternion.FromToRotation(transform.forward, initTransform.forward);
        transform.rotation = quaternion;
        StartCoroutine(Frezz( 6f));
    }

    public override void ResetValue()
    {
        gameObject.SetActive(false);
        transform.parent = SkillFactory.Instance.transform;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
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
    IEnumerator Frezz(float lifeTime)
    {
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }
        base.DestroyObject();
    }

}
