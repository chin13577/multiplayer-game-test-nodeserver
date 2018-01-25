using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

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
        socket.On("OnJoinRoom", OnJoinRoom);
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
        socket.On("OnStart", (e) =>
        {
            string scene = e.data["data"].str;
            SceneManager.LoadScene(scene);
        });
    }
    
    public void JoinRoomButtonClick(string room)
    {
        UserDataJson p = new UserDataJson();
        p.name = nameField.text;
        p.room = room;
        User.instance.SetPlayerData(p);
        print("Send : "+User.instance.GetPlayerData().ToJson());
        socket.Emit("JoinRoom", new JSONObject(User.instance.GetPlayerData().ToJson()));

    }
    public void PlayBtnClick()
    {
        string scene = "Stage1";
        
        socket.Emit("Start", new JSONObject(JsonConvert.SerializeObject(scene)));
        SceneManager.LoadScene(scene);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            socket.Emit("Test");
        }
    }
    //TODO : join to lobby
    public void OnJoinRoom(SocketIOEvent e)
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
