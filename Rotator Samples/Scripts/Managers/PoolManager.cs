using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

// Wanna use this pooling system? Just wire some
public enum PoolableObject { BasicEnemy, Bomb, BombEffect, Boss, Projectile, ShockOrb, EffectObj }
public class PoolManager : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [TabGroup("Values"), Required] public int ProjectilePoolSize;

    [TabGroup("Prefabs")]
    [SerializeField] private GameObject[] MasterPrefabList; // Maybe find DB-type solution

    public Dictionary<PoolableObject, GameObjectPool> Pools = new Dictionary<PoolableObject, GameObjectPool>();
    public Dictionary<PoolableObject, GameObject> Prefabs = new Dictionary<PoolableObject, GameObject>();

    private Vector3 poolPosition;
    private Vector3 waitPos;
    private void Awake()
    {
        if (!isActive)
            return;

        foreach (var prefab in MasterPrefabList)
        {
            Prefabs.Add(prefab.GetComponent<IPoolable>().POType, prefab);
        }

        // Level loads, pools are created with a predetermined set of enemies that are then split into pools
        // Create a pool factory? Read up on that? Or just a simple method that takes in the prefab (adressables, all in a list, injected?), the number of enemies in the pool, and then store that ref
        // somewhere easily accessible. Dict of string, poolref ? Probably best to use the enemy name in the enemy data SO? The caller can then use the enemydata.name property and the poolmanager
        // has access to all prefabs, which means all enemydata SOs. DO I ever want to create more than one enemy type per enemydata SO? Probably not, just duplicate, change a couple values and name differently.
        // And for runtime changes to stats you can always to those anyways.

        // list of all poolable objects
        // scene loads and hands over a subset of that to pool
        // enum time: use enums to map / refer to the list and set the name of the pool
        poolPosition = new Vector3(1000, 1000, 0);
        waitPos = new Vector3(100, 100, 0);

        // A level would just have a list of calls to this poolmanager to init the pool creations
        BuildPool(PoolableObject.BasicEnemy, 5);
        BuildPool(PoolableObject.Projectile, ProjectilePoolSize);
        BuildPool(PoolableObject.ShockOrb, 30);
        BuildPool(PoolableObject.EffectObj, 30);
        BuildPool(PoolableObject.BombEffect, 30);

        BuildPool(PoolableObject.Bomb, 30);

        //BuildPool(PoolableObject.Waller, 20);
        //BuildPool(PoolableObject.Chaser, 5);
        //BuildPool(PoolableObject.Shooter, 20);
        //BuildPool(PoolableObject.Charger, 30);

        DW.Log("Total Pools", Pools.Count);
        foreach (var p in Pools)
        {
            DW.Log(p.Key.ToString(), p.Value.Pool.Count);
        }
    }
    // Pool-level Methods
    public void BuildPool(PoolableObject poolableObject, int size)
    {
        if (Pools.ContainsKey(poolableObject))
            return;

        var pool = new GameObjectPool(poolableObject, size);
        for (int i = 0; i < size; i++)
        {
            var obj = Instantiate(Prefabs[poolableObject], poolPosition, Quaternion.identity);

            obj.SetActive(false); // LESSON awake is always called instantly when something is instantiated. Start is not.
            pool.Pool.Enqueue(obj);
        }
        Pools.Add(poolableObject, pool);
    }
    public void ExpandPool(GameObjectPool pool, int expandBy, PoolableObject poolableObject)
    {
        for (int i = 0; i < expandBy; i++)
        {
            var obj = Instantiate(Prefabs[poolableObject], poolPosition, Quaternion.identity);

            obj.SetActive(false);
            pool.Pool.Enqueue(obj);
        }
        pool.Size += expandBy;

        //Debug.Log($"Expanded {pool.Type} Pool by {expandBy}.");
    }
    public void ClearPool(PoolableObject poolableObject)
    {
        Pools[poolableObject].Pool.Clear();
        Pools[poolableObject].Pool.TrimExcess();
        Pools[poolableObject].Size = 0;
        Pools.Remove(poolableObject);
        // should be ready for GC
    }

    // PoolableObject-level Methods
    public GameObject FetchPooledObject(PoolableObject poolableObject)
    {
        var targetPool = Pools[poolableObject];

        if (targetPool.Pool.Count < 1)
            ExpandPool(targetPool, 5, poolableObject);

        var gameObject = targetPool.Pool.Dequeue();
        gameObject.transform.position = waitPos;
        gameObject.SetActive(true);
        return gameObject;
    }
    public void ReturnPoolableObject(GameObject gameObject)
    {
        gameObject.transform.position = waitPos;
        Pools[gameObject.GetComponent<IPoolable>().POType].Pool.Enqueue(gameObject);
        //Debug.Log($"{Pools[gameObject.GetComponent<IPoolable>().POType].Type} was found. " +
        //    $"Current number of objects in pool: {Pools[gameObject.GetComponent<IPoolable>().POType].Pool.Count}");
        gameObject.SetActive(false);
        gameObject.transform.position = poolPosition;
        DW.Log(Pools[gameObject.GetComponent<IPoolable>().POType].Type.ToString(), Pools[gameObject.GetComponent<IPoolable>().POType].Pool.Count);
    }
}