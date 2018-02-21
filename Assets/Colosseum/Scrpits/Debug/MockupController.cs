using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockupController : MonoBehaviour {

    public Player p;

    Vector2 oldVect;
    Vector2 vect;
    bool temp;
	// Update is called once per frame
	void Update () {
        if (Input.anyKey)
        {
            vect.x = Input.GetAxis("Horizontal");
            vect.y = Input.GetAxis("Vertical");
            if (oldVect != vect)
            {
                oldVect = vect;
                //p.Callback_OnJoyStickValueChange(vect.normalized);
            }
            temp = true;
        }
        else
        {
            if (temp == true)
            {
                vect = Vector2.zero;
                //p.Callback_OnJoyStickValueChange(vect);
            }
            temp = false;
        }
    }
}
