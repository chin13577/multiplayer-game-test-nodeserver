using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInPool : MonoBehaviour
{
    public string owner = "";
    public abstract void ResetValue();
    public abstract void Action(Vector3 dir,float speed);
    public void DestroyObject()
    {
        ObjectPoolManager.instance.Destroy(this);
    }
}
