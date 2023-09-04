using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private Func<GameObject> objectFactory;
    private Queue<GameObject> objectQueue = new Queue<GameObject>();

    public GameObjectPool(Func<GameObject> objectFactory, int initialSize)
    {
        this.objectFactory = objectFactory;

        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        GameObject obj = objectFactory.Invoke();
       // obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }

    public GameObject Get()
    {
        if (objectQueue.Count == 0)
        {
            CreateObject();
        }

        GameObject obj = objectQueue.Dequeue(); 
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }

    public void Clear()
    {
        objectQueue.Clear();
    }
}