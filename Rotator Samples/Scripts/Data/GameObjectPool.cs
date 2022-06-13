using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    // If it is just the container then it doesnt need to know how to create the pool or what prefab is used.
    // That can be done via poolmanager

    public int Size { get; set; }
    public PoolableObject Type { get; set; }
    public Queue<GameObject> Pool { get; set; }

    public GameObjectPool(PoolableObject type, int size)
    {
        Pool = new Queue<GameObject>(size);
        Type = type;
    }

    public GameObject Fetch() => Pool.Dequeue();
    public void Return(GameObject gameObject) => Pool.Enqueue(gameObject);
}