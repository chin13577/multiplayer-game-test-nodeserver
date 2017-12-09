using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour {

    public Vector3 offset;
    public Transform target;
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position+ offset, target.position+ offset, 0.2f);
	}
}
