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
[Serializable]
public class PlayerJson
{
    public string name;
    public float hp;
    public float[] position;
    public float[] rotation;
}
[Serializable]
public class RotationJson
{
    
    public float[] rotation;
    public RotationJson(Quaternion rot)
    {
        rotation = new float[3] { rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z };
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static Quaternion FromJson(float[] rotation)
    {
        return Quaternion.Euler(rotation[0], rotation[1], rotation[2]);
    }
}
[Serializable]
public class PositionJson
{
    public float[] position;
    public PositionJson(Vector3 pos)
    {
        position = new float[3] { pos.x, pos.y, pos.z };
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static Vector3 FromJson(float[] position)
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}
public class AnimationJson
{
    public string name;
    public object args;
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}