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
public class QuaternionJson
{
    public static float[] ToFloat(Quaternion q)
    {
        return new float[3] { q.eulerAngles.x, q.eulerAngles.y, q.eulerAngles.z };
    }
    public static Quaternion ToQuaternion(float[] euler)
    {
        return Quaternion.Euler(euler[0], euler[1], euler[2]);
    }
}
[Serializable]
public class VectorJson
{
    public static float[] ToFloat(Vector3 pos)
    {
        return new float[3] { pos.x, pos.y, pos.z };
    }
    public static Vector3 ToVector3(float[] position)
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}
[Serializable]
public class AnimationJson
{
    public string name;
    public object args;
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}