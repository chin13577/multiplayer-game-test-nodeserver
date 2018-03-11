using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    Vector3 targetPos;
    public override void EnterState(Vector3 position, Vector3 direction)
    {
        transform.parent = null;
        transform.position = position + Vector3.up*0.5f;
        targetPos = position+ Vector3.up*0.5f;
        transform.rotation = Quaternion.LookRotation(direction);
        //targetPos = position + new Vector3(direction.x * 5 * Time.deltaTime, 0.5f, direction.z * 5 * Time.deltaTime);
    }
    public override void UpdateState(Vector3 position, Vector3 direction)
    {
        targetPos = position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    private void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(this.transform.position, targetPos, 0.125f);
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
                //send attack.
            }
            if(owner == User.instance.GetPlayerData().name)
            {
                // send data to server.
                WSGameManager.instance.SendDestroySkill(this.id);
            }
            //base.DestroyObject();
        }
    }
   

}
