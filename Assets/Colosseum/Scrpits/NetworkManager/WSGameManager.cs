using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class WSGameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    public GameObject playerPrefabs;
    public Transform[] spawnPoint;
    public SocketIOComponent socket;
    // Use this for initialization
    IEnumerator Start()
    {
        socket.On("UserConnected", OnUserConnected);
        socket.On("UserDisConnected", OnUserDisConnected);
        yield return new WaitForSeconds(0.3f);
        socket.Emit("Connect");
    }
    /// <summary>
    /// other player connect;
    /// </summary>
    /// <param name="obj"></param>
    private void OnUserConnected(SocketIOEvent obj)
    {
        print(obj.data["name"].str);
    }
    private void OnUserDisConnected(SocketIOEvent obj)
    {
        throw new NotImplementedException();
    }

}
