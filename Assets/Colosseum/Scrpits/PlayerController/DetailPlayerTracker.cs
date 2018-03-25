using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailPlayerTracker : MonoBehaviour {

    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    void LateUpdate()
    {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
