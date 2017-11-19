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
        post.AddField("password", 69);
        StartCoroutine(RestfulUtility.Post("127.0.0.1:3000", post, (data) =>
        {
            print(data.text);
        }));
    }
}
