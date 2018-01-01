using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class AccountManager : MonoBehaviour
{

    public SocketIOComponent socket;
    private void Awake()
    {
        socket = GameObject.FindObjectOfType<SocketIOComponent>();
    }
    // Use this for initialization
    void Start()
    {
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);
        socket.On("OnJoinRoom", OnJoinRoom);
    }
    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("x", vector.x);
        jsonObject.AddField("y", vector.z);
        return jsonObject;
    }


    public void OnLoginButtonClick()
    {
        PlayerData p = new PlayerData();
        p.name = "Chin";
        string data = JsonUtility.ToJson(p);
        socket.Emit("OnJoinRoom", new JSONObject(data));

    }
    void OnJoinRoom(SocketIOEvent e)
    {
        print(e.name + " " + e.data);
    }
    void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }
    void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }
}
