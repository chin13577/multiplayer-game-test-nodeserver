using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public static User instance;

    UserDataJson data;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Initialize()
    {
        data = new UserDataJson();
    }
    public UserDataJson GetPlayerData()
    {
        return this.data;
    }
    public void SetPlayerData(UserDataJson p)
    {
        this.data = p;
    }

}
