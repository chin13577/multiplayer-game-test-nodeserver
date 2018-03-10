﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : Skill
{
    public override void UpdateState(Vector3 position, Vector3 direction)
    {
        transform.parent = null;
        transform.position = position;
        transform.localScale = new Vector3(skillData.size, transform.localScale.y, skillData.size);
        Quaternion quaternion = Quaternion.LookRotation(direction);
        transform.rotation = quaternion;
        StartCoroutine(Frezz(6f));
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

    public override void EnterState(Vector3 position, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
