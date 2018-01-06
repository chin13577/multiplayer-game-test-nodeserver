using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

public class AccountManager : MonoBehaviour
{
    public InputField nameField;
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
        socket.On("OnJoinRoom", OnJoinLobby);
        //socket.On("OnLeaveRoom", OnJoinLobby);
        socket.On("OnRoomCreated", (e) =>
        {
            //{"roomData":[{"name":"lobby","length":1}]}
            RoomDataJson rooms = JsonConvert.DeserializeObject<RoomDataJson>(e.data + "");

            Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        });
        socket.On("OnLeaveRoom", (e) => {
            Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
        });
    }

    public void OnLoginButtonClick()
    {
        UserDataJson p = new UserDataJson();
        p.name = nameField.text;
        p.room = "lobby";
        User.instance.SetPlayerData(p);
        print(User.instance.GetPlayerData().ToJson());
        socket.Emit("OnJoinRoom", new JSONObject(User.instance.GetPlayerData().ToJson()));

    }
    public void JoinRoomButtonClick()
    {
        UserDataJson p = new UserDataJson();
        p.name = nameField.text;
        p.room = "1";
        User.instance.SetPlayerData(p);
        print(User.instance.GetPlayerData().ToJson());
        socket.Emit("OnJoinRoom", new JSONObject(User.instance.GetPlayerData().ToJson()));

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            socket.Emit("Test");
        }
    }
    //TODO : join to lobby
    public void OnJoinLobby(SocketIOEvent e)
    {
        Debug.Log(e.data + "");
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
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
