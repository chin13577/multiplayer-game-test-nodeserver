using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using Newtonsoft.Json;

public class WSGameManager : MonoBehaviour
{
    public static WSGameManager instance;
    public Dictionary<string, GameObject> playerDict = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> skillDict = new Dictionary<string, GameObject>();
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
        socket.On("OnAnimChange", OnAnimChange);
        socket.On("OnSpawnSkill", OnSpawnSkill);
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
        var obj = new
        {
            position = VectorJson.ToFloat(pos)
        };
        //print(JsonConvert.SerializeObject(obj));
        socket.Emit("Move", new JSONObject(JsonConvert.SerializeObject(obj)));
    }

    public void SendRotaion(Quaternion rot)
    {
        var obj = new
        {
            rotation = QuaternionJson.ToFloat(rot)
        };
        //print(JsonConvert.SerializeObject(obj));
        socket.Emit("Rotate", new JSONObject(JsonConvert.SerializeObject(obj)));
    }
    public void SendAnimation(AnimationJson animJson)
    {
        AnimationJson data = animJson;
        //print(data.ToJson());
        socket.Emit("Animate", new JSONObject(JsonConvert.SerializeObject(data)));
    }
    public void SendSpawnSkill(string owner, SkillName skillName, Vector3 position, Quaternion direction)
    {
        var obj = new
        {
            id = owner + "_" + DateTime.Now.ToString("hh.mm.ss.ffffff"),
            owner = owner + "",
            skillName = skillName.ToString(),
            position = VectorJson.ToFloat(position),
            direction = QuaternionJson.ToFloat(direction)
        };
        socket.Emit("SpawnSkill", new JSONObject(JsonConvert.SerializeObject(obj)));
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
            if (playerDict.ContainsKey(item.name) == false)
            {
                GameObject g = Instantiate(playerPrefab);
                g.name = item.name;
                // is local
                if (User.instance.GetPlayerData().name == item.name)
                {
                    g.AddComponent<PlayerController>();
                    g.GetComponent<Player>().isLocal = true;
                    g.GetComponent<SynchronizeTransform>().isLocal = true;
                }
                else
                {
                    g.GetComponent<Player>().isLocal = false;
                    g.GetComponent<SynchronizeTransform>().isLocal = false;
                }

                g.GetComponent<Player>().playerData = item;
                g.transform.position = VectorJson.ToVector3(item.position);
                // add player to dict
                playerDict.Add(item.name, g);
            }
        }

    }
    private void OnLeaveRoom(SocketIOEvent obj)
    {
        string name = obj.data["name"].str;
        Destroy(playerDict[name].gameObject);
        playerDict.Remove(name);
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
        playerDict[data.name].GetComponent<SynchronizeTransform>().SetTargetPosition(VectorJson.ToVector3(vect));
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
        playerDict[data.name].GetComponent<SynchronizeTransform>().SetTargetRotation(QuaternionJson.ToQuaternion(euler));
    }
    private void OnAnimChange(SocketIOEvent obj)
    {
        AnimationJson anim = JsonConvert.DeserializeObject<AnimationJson>(obj.data.GetField("animation") + "");
        playerDict[obj.data.GetField("name").str].GetComponent<PlayerAnimatorController>().UpdateAnimation(anim.name, anim.args);
    }
    void OnSpawnSkill(SocketIOEvent obj)
    {
        var data = new
        {
            id = "",
            owner = "",
            skillName = "",
            position = new float[3],
            direction = new float[3]
        };
        data = JsonConvert.DeserializeAnonymousType(obj.data + "", data);
        GameObject g = SkillFactory.Instance.GetSkillObject((SkillName)Enum.Parse(typeof(SkillName), data.skillName));
        if (g != null)
        {
            Skill skill = g.GetComponent<Skill>();
            skill.id = data.id;
            skill.owner = data.owner;
            skill.Action(VectorJson.ToVector3(data.position),QuaternionJson.ToQuaternion(data.direction));
        }
        skillDict.Add(data.id, g);
    }
    #endregion
}
