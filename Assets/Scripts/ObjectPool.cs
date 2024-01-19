using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : SingleTon<ObjectPool>
{
    [System.Serializable]
    public class PoolInfo
    {
        public GameObject prefab;
        public int poolSize;
    }

    public List<PoolInfo> poolInfos;

    private Dictionary<string, Queue<GameObject>> pooledObjects = new Dictionary<string, Queue<GameObject>>();
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach (var info in poolInfos)
        {
            string key = info.prefab.name;

            if (!pooledObjects.ContainsKey(key))
            {
                Queue<GameObject> newQueue = new Queue<GameObject>();
                pooledObjects.Add(key, newQueue);
            }

            for (int i = 0; i < info.poolSize; i++)
            {
                GameObject newObj = Instantiate(info.prefab, Vector3.zero, Quaternion.identity, transform);
                newObj.name = key;
                newObj.SetActive(false);
                pooledObjects[key].Enqueue(newObj);
            }
        }
    }
    public void OffObject()
    {
        Transform[] g = GetComponentsInChildren<Transform>();
        for (int i = 1; i < g.Length; i++)
        {
            g[i].gameObject.SetActive(false);
        }
    }
    public void ReturnAllToPool(string _name)
    {
        if (pooledObjects.ContainsKey(_name))
        {
            Laser[] g = GetComponentsInChildren<Laser>();

            foreach(Laser laser in g)
            {
                if(laser.gameObject.activeSelf == true)
                    ReturnToPool(laser.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return objects to an unknown pool: " + _name);
        }
    }
    public GameObject GetPooledObject(string _name, Vector3 pos)
    {
        if (pooledObjects.ContainsKey(_name) && pooledObjects[_name].Count > 0)
        {
            GameObject obj = pooledObjects[_name].Dequeue();
            obj.transform.position = pos;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            PoolInfo poolInfo = GetPoolInfoByName(_name);
            if (poolInfo != null)
            {
                GameObject newObj = Instantiate(poolInfo.prefab, pos, Quaternion.identity, transform);
                newObj.name = _name;
                return newObj;
            }
            else
            {
                Debug.LogWarning("Trying to get an object from an unknown pool: " + _name);
                return null;
            }
        }
    }

    private PoolInfo GetPoolInfoByName(string _name)
    {
        foreach (var info in poolInfos)
        {
            if (info.prefab.name == _name)
            {
                return info;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        string key = obj.name;
        if (pooledObjects.ContainsKey(key))
        {
            pooledObjects[key].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Trying to return an object to an unknown pool: " + key);
        }
    }
}