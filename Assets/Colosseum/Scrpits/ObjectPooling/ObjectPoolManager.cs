using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    public static ObjectPoolManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public ObjectInPool prefabs;

    public int amount;
    public bool autoIncrement;
    public Queue<ObjectInPool> objectQueue = new Queue<ObjectInPool>();

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;
        Initialize();
    }

    //Initializes the game for each level.
    void Initialize()
    {
        for (int i = 0; i < amount; i++)
        {
            ObjectInPool obj = Instantiate(prefabs, transform.position, Quaternion.identity);
            obj.ResetValue();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(this.transform);
            objectQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (objectQueue.Count > 0)
        {
            ObjectInPool g = objectQueue.Dequeue();
            g.gameObject.SetActive(true);
            return g.gameObject;
        }else if (autoIncrement)
        {
            ObjectInPool obj = Instantiate(prefabs, transform.position, Quaternion.identity);
            obj.ResetValue();
            return obj.gameObject;
        }
        return null;
    }
    public void Destroy(ObjectInPool obj)
    {
        obj.ResetValue();
        obj.transform.SetParent(this.transform);
        obj.gameObject.SetActive(false);
        objectQueue.Enqueue(obj);
    }

    private void OnDestroy()
    {
        objectQueue.Clear();
    }



}
