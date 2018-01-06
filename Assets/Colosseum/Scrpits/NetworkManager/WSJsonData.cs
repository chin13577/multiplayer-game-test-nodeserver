using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserDataJson
{
    public string name;
    public string room;
    public UserDataJson()
    {
        name = "";
        room = "";
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

[Serializable]
public class RoomDataJson
{
    public RoomJson[] roomData;
}
[Serializable]
public class RoomJson
{
    public string name;
    public int length;
}
public class PlayerDataJson
{
    public string name;
    public float hp;
    public float[] position;
    public float[] rotation;
}