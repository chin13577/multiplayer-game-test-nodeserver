using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    bool isAction;
    public override void Action(Transform initTransform)
    {
        isAction = true;
        transform.parent = null;
        transform.position = initTransform.position + Vector3.up * 0.5f;
        Quaternion quaternion = Quaternion.FromToRotation(transform.forward, initTransform.forward);
        transform.rotation = quaternion;
        StartCoroutine(UpdatePos(10f, 6f));
    }

    public override void ResetValue()
    {
        gameObject.SetActive(false);
        transform.parent = SkillFactory.Instance.transform;
        transform.rotation = Quaternion.identity;
        isAction = false;
        StopAllCoroutines();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Skill") return;
        if (other.name != owner)
        {
            if (other.tag == "Player" )
            {
                other.GetComponent<Player>().Knockback(this.transform.forward);
            }
            base.DestroyObject();
        }
    }
    IEnumerator UpdatePos(float speed, float lifeTime)
    {
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            yield return null;
        }
        base.DestroyObject();
    }
}
