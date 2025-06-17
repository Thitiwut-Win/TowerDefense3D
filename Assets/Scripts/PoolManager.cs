using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private List<PoolableObject> poolableObjects;
    private Dictionary<string, PoolableObject> prefabDict = new Dictionary<string, PoolableObject>();
    private Dictionary<string, Queue<PoolableObject>> poolDict = new Dictionary<string, Queue<PoolableObject>>();
    private int initialCount = 3;
    protected override void Awake()
    {
        base.Awake();
        foreach (PoolableObject poolableObject in poolableObjects)
        {
            prefabDict.Add(poolableObject.name, poolableObject);
            poolDict.Add(poolableObject.name, new Queue<PoolableObject>());
            for (int i = 0; i < initialCount; i++)
            {
                PoolableObject p = Instantiate(poolableObject, transform.position, Quaternion.identity);
                p.name = poolableObject.name;
                p.gameObject.SetActive(false);
                p.transform.SetParent(transform);
                p.enabled = false;
                poolDict[poolableObject.name].Enqueue(p);
            }
        }
    }
    public T Spawn<T>(string itemName) where T : PoolableObject
    {
        if (!prefabDict.ContainsKey(itemName))
        {
            Debug.LogError("Not in poolable list");
            return null;
        }
        PoolableObject poolableObject;
        if (poolDict[itemName].Count > 0)
        {
            poolableObject = poolDict[itemName].Dequeue();
            poolableObject.gameObject.SetActive(true);
            poolableObject.enabled = true;
        }
        else
        {
            poolableObject = Instantiate(prefabDict[itemName], transform.position, Quaternion.identity);
            poolableObject.name = itemName;
        }

        poolableObject.OnSpawn();
        poolableObject.transform.SetParent(null);
        return (T)poolableObject;
    }
    public T Spawn<T>(string itemName, Vector3 position, Quaternion rotation) where T : PoolableObject
    {
        T item = Spawn<T>(itemName);
        if (item == null) return item;
        item.transform.position = position;
        item.transform.rotation = rotation;
        return item;
    }
    public void Despawn(PoolableObject poolableObject)
    {
        if (!prefabDict.ContainsKey(poolableObject.name))
        {
            Debug.LogError("Not in poolable list");
            return;
        }
        poolableObject.OnDespawn();
        poolableObject.gameObject.SetActive(false);
        poolableObject.enabled = false;
        poolableObject.transform.position = transform.position;
        poolDict[poolableObject.name].Enqueue(poolableObject);
        poolableObject.transform.SetParent(transform);
    }
}