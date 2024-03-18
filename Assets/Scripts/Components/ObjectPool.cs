using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public Transform ItemsParent;
    public Item ItemPrefab;
    public int ItemAmount = 20;

    private List<Item> _pooledObjects = new List<Item>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        for (int i = 0; i < ItemAmount; i++)
        {
            Item obj = Instantiate(ItemPrefab);
            obj.Hide();
            _pooledObjects.Add(obj);
        }
    }

    public Item GetPooledObject(string itemId)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if(_pooledObjects[i].IsPassive())
            {
                return _pooledObjects[i];
            }
        }

        Item obj = Instantiate(ItemPrefab);
        obj.Hide();
        _pooledObjects.Add(obj);

        return obj;
    }
}
