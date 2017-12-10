using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SynchronizeTransform : MonoBehaviour
{
    Action<Vector3> OnPositionChanged;
    Action<Quaternion> OnRotationChanged;
    Action<Vector3> OnScaleChanged;

    public bool isSyncPosition;
    public bool isSyncRotation;
    public bool isSyncScale;
    Vector3 oldPosition;
    Quaternion oldRotation;
    Vector3 oldScale;
    
    private void Start()
    {
        if (isSyncPosition)
            SyncPosition();
        if (isSyncRotation)
            SyncRotation();
        if (isSyncScale)
            SyncScale();
    }

    void FixedUpdate()
    {
        if (isSyncPosition)
            SyncPosition();
        if (isSyncRotation)
            SyncRotation();
    }
    void SyncPosition()
    {
        if (oldPosition != transform.position)
        {
            oldPosition = transform.position;
            if (OnPositionChanged != null)
                OnPositionChanged(oldPosition);
        }
    }
    void SyncRotation()
    {
        if(oldRotation != transform.rotation)
        {
            oldRotation = transform.rotation;
            if (OnRotationChanged != null)
                OnRotationChanged(oldRotation);
        }
    }
    void SyncScale()
    {
        if (oldScale != transform.localScale)
        {
            oldScale = transform.localScale;
            if (OnScaleChanged != null)
                OnScaleChanged(oldScale);
        }
    }
}
