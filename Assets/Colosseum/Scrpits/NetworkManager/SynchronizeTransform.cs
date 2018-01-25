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
    Vector3 targetRot;
    #endregion

    private void Start()
    {
        manager = GameObject.FindObjectOfType<WSGameManager>();
        SyncPosition();
        SyncRotation();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer >= 0.1f)
        {
            timer = 0;
            SyncPosition();
            SyncRotation();
        }
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
