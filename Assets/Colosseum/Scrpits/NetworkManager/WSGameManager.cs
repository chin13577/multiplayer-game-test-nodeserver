using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using Newtonsoft.Json;

public class WSGameManager : MonoBehaviour
{
    public static WSGameManager instance;
    public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> bullets = new Dictionary<string, GameObject>();
    public GameObject playerPrefab;
    public GameObject skillPointPrefab;
    public Transform[] spawnPoint;
    public SocketIOComponent socket;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    IEnumerator Start()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        socket.On("OnPlay", OnPlay);
        socket.On("OnLeaveRoom", OnLeaveRoom);
        socket.On("OnMove", OnMove);
        socket.On("OnRotate", OnRotate);
        socket.On("OnRotate", OnRotate);
        socket.On("OnAnimChange", OnAnimChange);
        yield return new WaitForSeconds(0.3f);
        var obj = new
        {
            spawnPoints = GenerateSpawnPointJson(),
            name = User.instance.GetPlayerData().name
        };
        print(JsonConvert.SerializeObject(obj));
        socket.Emit("Play", new JSONObject(JsonConvert.SerializeObject(obj)));
    }
    List<float[]> GenerateSpawnPointJson()
    {
        List<float[]> positions = new List<float[]>();
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            positions.Add(new float[3] { spawnPoint[i].position.x, spawnPoint[i].position.y, spawnPoint[i].position.z });
        }
        return positions;
    }

    #region send data

    public void SendPosition(Vector3 pos)
    {
        PositionJson p = new PositionJson(pos);
        print(p.ToJson());
        socket.Emit("Move", new JSONObject(JsonConvert.SerializeObject(p)));
    }
    
    public void SendSkillPosition(string name,Vector3 pos)
    {
        PositionJson p = new PositionJson(pos);
        var obj = new
        {
            skillName = name,
            position = p.position
        };
        socket.Emit("SpawnSkill", new JSONObject(JsonConvert.SerializeObject(obj)));
    }

    public void SendRotaion( Quaternion rot)
    {
        RotationJson r = new RotationJson(rot);
        print(r.ToJson());
        socket.Emit("Rotate", new JSONObject(JsonConvert.SerializeObject(r)));
    }
    public void SendAnimation(AnimationJson animJson)
    {
        AnimationJson data = animJson;
        socket.Emit("Animate", new JSONObject(JsonConvert.SerializeObject(data)));
    }
    #endregion
    #region recieve data
    private void OnPlay(SocketIOEvent obj)
    {
        print(obj.data + " ");
        var data = new
        {
            players = new PlayerJson[0]
        };
        data = JsonConvert.DeserializeAnonymousType(obj.data + "", data);
        print(JsonConvert.SerializeObject(data));
        foreach (PlayerJson item in data.players)
        {
            if (players.ContainsKey(item.name) == false)
            {
                GameObject g = Instantiate(playerPrefab);
                g.name = item.name;
                // is local
                if (User.instance.GetPlayerData().name == item.name)
                {
                    g.GetComponent<Player>().SetIsLocal();
                    g.AddComponent<PlayerController>();
                    g.GetComponent<SynchronizeTransform>().isLocal = true ;
                }
                else
                {
                    g.GetComponent<SynchronizeTransform>().isLocal = false;
                }

                g.GetComponent<Player>().playerData = item;
                g.transform.position = PositionJson.FromJson(item.position);
                // add player to dict
                players.Add(item.name, g);
            }
        }

    }
    private void OnLeaveRoom(SocketIOEvent obj)
    {
        string name = obj.data["name"].str;
        Destroy(players[name].gameObject);
        players.Remove(name);
    }

    private void OnMove(SocketIOEvent obj)
    {
        var data = new
        {
            name = "",
            position = new float[3]
        };
        data = JsonConvert.DeserializeAnonymousType(obj.data + "", data);
        float[] vect = data.position;
        players[data.name].GetComponent<SynchronizeTransform>().SetTargetPosition(PositionJson.FromJson(vect));
        //players[data.name].transform.position = PositionJson.FromJson(vect);
    }
    private void OnRotate(SocketIOEvent obj)
    {
        var data = new
        {
            name = "",
            rotation = new float[3]
        };
        data = JsonConvert.DeserializeAnonymousType(obj.data + "", data);
        float[] euler = data.rotation;
        //players[data.name].transform.rotation = RotationJson.FromJson(euler);
        players[data.name].GetComponent<SynchronizeTransform>().SetTargetRotation(RotationJson.FromJson(euler));
    }
    private void OnAnimChange(SocketIOEvent obj)
    {
        AnimationJson anim = JsonConvert.DeserializeObject<AnimationJson>(obj.data.GetField("animation")+"");
        players[obj.data.GetField("name").str].GetComponent<PlayerAnimatorController>().UpdateAnimation(anim.name, anim.args);
    }
    void OnUseSkill(SkillData skillData, Transform currentSkillTransform)
    {
        GameObject g = SkillFactory.Instance.GetSkillObject(skillData.skillName);
        if (g != null)
        {
            Skill obj = g.GetComponent<Skill>();
            obj.owner = this.name;
            obj.Action(currentSkillTransform);
        }
    }
    #endregion
}
