using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public override void Action(Vector3 position, Quaternion direction)
    {
        transform.parent = null;
        transform.position = position + Vector3.up * 0.5f;
        Quaternion quaternion = direction;
        transform.rotation = quaternion;
        if (User.instance.name == owner)
        {
            StartCoroutine(UpdatePos(10f, 6f));
        }
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
        if (other.tag == "Skill") return;
        if (other.name != owner)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().Knockback(this.transform.forward);
            }
            base.DestroyObject();
        }
    }
    IEnumerator UpdatePos(float speed, float lifeTime)
    {
        float cooldown = 0;
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //cooldown of send data to server
            cooldown += Time.deltaTime;
            if (cooldown >= 1)
            {
                cooldown = 0;
                //WSGameManager.instance.
            }

            yield return null;
        }
        base.DestroyObject();
    }
}
