using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SynchronizeObject : MonoBehaviour
{
    Action<Vector3> OnPositionChanged;
    Action<Quaternion> OnRotationChanged;
    Action<int> OnAnimationChanged;

    public bool isSyncPosition;
    public bool isSyncRotation;
    public bool isSyncAnimation;
    Vector3 oldPosition;
    Quaternion oldRotation;
    int currentHashAnimation;


    public Animator anim;
    private void Start()
    {
        if (isSyncPosition)
            SyncPosition();
        if (isSyncRotation)
            SyncRotation();
        if (isSyncAnimation)
            SyncAnimation();
    }

    void FixedUpdate()
    {
        if (isSyncPosition)
            SyncPosition();
        if (isSyncRotation)
            SyncRotation();
        if (isSyncAnimation)
            SyncAnimation();
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
    void SyncAnimation()
    {
        if(currentHashAnimation!= anim.GetCurrentAnimatorStateInfo(0).shortNameHash)
        {
            currentHashAnimation = anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
            if (OnAnimationChanged != null)
                OnAnimationChanged(currentHashAnimation);
        }
    }
}
