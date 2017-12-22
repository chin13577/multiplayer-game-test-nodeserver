using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ObjectInPool {
    bool isAction;
    public override void Action(Vector3 dir, float speed)
    {
        isAction = true;
        Quaternion quaternion = Quaternion.FromToRotation(transform.forward,dir);
        transform.rotation = quaternion;
        StartCoroutine(UpdatePos(speed,6f));
    }

    public override void ResetValue()
    {
        transform.rotation = Quaternion.identity;
        isAction = false;
        StopAllCoroutines();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name != owner)
        {
            if(other.tag == "Player")
            {
                other.GetComponent<Player>().Knockback(this.transform.forward);
            }
            base.DestroyObject();
        }
    }
    IEnumerator UpdatePos(float speed,float lifeTime)
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
