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
        socket.On("OnSkillCreated", OnSkillCreated);
        socket.On("OnSkillUpdated", OnSkillUpdated);
        socket.On("OnDamaged", OnDamaged);
        socket.On("OnDead", OnDead);
        socket.On("OnDestroySkill", OnDestroySkill);
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
    public void SendSpawnSkill(string owner, SkillName skillName, Vector3 position, Vector3 direction)
    {
        var obj = new
        {
            id = owner + "_" + DateTime.Now.ToString("mm.ss.ffff"),
            owner = owner + "",
            skillName = skillName.ToString(),
            position = VectorJson.ToFloat(position),
            direction = VectorJson.ToFloat(direction)
        };
        socket.Emit("SpawnSkill", new JSONObject(JsonConvert.SerializeObject(obj)));
    }
    public void SendDestroySkill(string id)
    {
        print("DestroySkill id: " + id);
        socket.Emit("DestroySkill", new JSONObject(JsonConvert.SerializeObject(id)));
    }
    public void SendAttack(string target, SkillName skillName, Vector3 direction)
    {
        var obj = new
        {
            target = target,
            skillName = skillName.ToString(),
            direction = VectorJson.ToFloat(direction)
        };
        socket.Emit("Attack", new JSONObject(JsonConvert.SerializeObject(obj)));
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
        foreach (PlayerJson p in data.players)
        {
            print(p.name);
            if (playerDict.ContainsKey(p.name) == false)
            {
                GameObject g = Instantiate(playerPrefab);
                g.name = p.name;
                // is local
                if (User.instance.GetPlayerData().name == p.name)
                {
                    g.AddComponent<PlayerController>();
                    g.GetComponent<Player>().isLocal = true;
                    g.GetComponent<SynchronizeTransform>().isLocal = true;
                    Camera.main.GetComponent<CameraTracking>().target = g.transform;
                }
                else
                {
                    g.GetComponent<Player>().isLocal = false;
                    g.GetComponent<SynchronizeTransform>().isLocal = false;
                }

                g.GetComponent<Player>().SetPlayerData(p);
                g.transform.position = VectorJson.ToVector3(p.position);
                // add player to dict
                playerDict.Add(p.name, g);
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
        print(obj.data.GetField("name").str + " " + anim.name);
        playerDict[obj.data.GetField("name").str].GetComponent<PlayerAnimatorController>().UpdateAnimation(anim.name, anim.args);
    }
    private void OnSkillCreated(SocketIOEvent obj)
    {
        Debug.Log("[SocketIO] OnSkillCreated: " + obj.name + " " + obj.data);
        var skill = new
        {
            data = new SkillJson()
        };
        skill = JsonConvert.DeserializeAnonymousType(obj.data + "", skill);

        //generate skill
        GameObject g = SkillFactory.Instance.GetSkillObject(
            (SkillName)Enum.Parse(typeof(SkillName),
            skill.data.skillName));
        if (g != null)
        {
            Skill s = g.GetComponent<Skill>();
            s.id = skill.data.id;
            s.owner = skill.data.owner;
            s.EnterState(VectorJson.ToVector3(skill.data.position), VectorJson.ToVector3(skill.data.direction));
        }
        skillDict.Add(skill.data.id, g);

    }

    private void OnSkillUpdated(SocketIOEvent obj)
    {
        Debug.Log("[SocketIO] OnSkillUpdated: " + obj.name + " " + obj.data);
        var skill = new
        {
            data = new SkillJson()
        };
        skill = JsonConvert.DeserializeAnonymousType(obj.data + "", skill);
        SkillJson s = skill.data;
        if (skillDict.ContainsKey(s.id))
        {
            skillDict[s.id].GetComponent<Skill>().UpdateState(
                VectorJson.ToVector3(s.position),
                VectorJson.ToVector3(s.direction)
                );
        }
    }
    private void OnDead(SocketIOEvent obj)
    {
        var data = new
        {
            target = ""
        };
        data = JsonConvert.DeserializeAnonymousType(obj.data + "", data);
        if (User.instance.GetPlayerData().name == data.target)
        {
            playerDict[data.target].GetComponent<Player>().Dead();
        }
    }
    private void OnDamaged(SocketIOEvent obj)
    {
        var data = new
        {
            target = "",
            hp = new float(),
            skillName = "",
            direction = new float[3]
        };
        data = JsonConvert.DeserializeAnonymousType(obj.data + "", data);
        print(data.target);
        float[] direction = data.direction;
        playerDict[data.target].GetComponent<Player>().GetDamage(data.hp);

        if (User.instance.GetPlayerData().name != data.target)
            return;
        if (data.skillName == SkillName.Fireball.ToString())
        {
            playerDict[data.target].GetComponent<Player>().Knockback(VectorJson.ToVector3(direction));
        }
        else if (data.skillName == SkillName.Lightning.ToString())
        {
            playerDict[data.target].GetComponent<Player>().Stun();
        }
        else
        {
            playerDict[data.target].GetComponent<Player>().Stun();
        }
    }
    private void OnDestroySkill(SocketIOEvent obj)
    {
        Debug.Log("[SocketIO] OnDestroySkill: " + obj.data.GetField("id").str);
        string id = obj.data.GetField("id").str;
        if (skillDict.ContainsKey(id))
        {
            skillDict[id].GetComponent<Skill>().DestroyObject();
            skillDict.Remove(id);
        }
    }
    #endregion
}
