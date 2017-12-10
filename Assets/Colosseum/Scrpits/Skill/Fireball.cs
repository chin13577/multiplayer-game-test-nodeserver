using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ObjectInPool {
    bool isAction;
    public override void Action(Vector3 dir, float speed)
    {
        isAction = true;
        StartCoroutine(UpdatePos(dir, speed,6f));
    }

    public override void ResetValue()
    {
        isAction = false;
        StopAllCoroutines();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name != owner)
        {
            if(other.tag == "Player")
            {
                other.GetComponent<PlayerController>().Knockback(this.transform.forward);
            }
            base.DestroyObject();
        }
    }
    IEnumerator UpdatePos(Vector3 dir,float speed,float lifeTime)
    {
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            transform.Translate(dir * speed * Time.deltaTime);
            yield return null;
        }
        base.DestroyObject();
    }
}
