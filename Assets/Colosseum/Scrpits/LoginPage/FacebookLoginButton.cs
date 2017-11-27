using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookLoginButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    public void OnClick()
    {
        WWWForm post = new WWWForm();
        post.AddField("id", "chinnie");
        post.AddField("password", "69a");
        StartCoroutine(RestfulUtility.Post("http://localhost:3000/login", post, (data) =>
        {
            print(data.text);
        }));
    }
}
