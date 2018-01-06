using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public InputField roomField;
    public SocketIOComponent socket;
    private void Awake()
    {
        socket = GameObject.FindObjectOfType<SocketIOComponent>();
    }
    //TODO : join to lobby
    public void OnJoinLobbyClick()
    {

    }
    //TODO : imprement leave lobby
    public void OnLeaveLobbyClick()
    {

    }

    void OnJoinRoom(SocketIOEvent e)
    {
        print(e.name + " " + e.data);
    }
    void OnLeaveRoom(SocketIOEvent e)
    {
        print(e.name + " " + e.data);
    }
}
