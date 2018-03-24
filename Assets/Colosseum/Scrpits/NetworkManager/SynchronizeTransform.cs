using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SocketIO;

public class SynchronizeTransform : MonoBehaviour
{
    public bool isLocal;
    WSGameManager manager;

    Vector3 oldPosition;
    Quaternion oldRotation;
    float timer;

    #region networking
    Vector3 targetPos;
    Quaternion targetRot;
    #endregion
    
    private void Start()
    {
        manager = GameObject.FindObjectOfType<WSGameManager>();
        if (isLocal)
        {
            SyncPosition();
            SyncRotation();
        }
    }

    void Update()
    {
        if (isLocal)
        {
            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {
                timer = 0;
                SyncPosition();
                SyncRotation();
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.125f);
        }
    }
    public void SetTargetPosition(Vector3 pos)
    {
        targetPos = pos;
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
    }
    public void SetTargetRotation(Quaternion q)
    {
        targetRot = q;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.125f);
    }
    void SyncPosition()
    {
        if (oldPosition != transform.position)
        {
            oldPosition = transform.position;
            manager.SendPosition(oldPosition);
        }
    }
    void SyncRotation()
    {
        if (oldRotation != transform.rotation)
        {
            oldRotation = transform.rotation;
            manager.SendRotaion(oldRotation);
        }
    }
}
