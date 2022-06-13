using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField, Required] private PoolManager poolManager;
    [SerializeField, Required] private TimeKeeper timeKeeper;
    [SerializeField, Required] public List<EnemyData> EnemyDataMasterList = new List<EnemyData>();
    [SerializeField, Required] private List<List<SpawnEventData>> spawnEventCollection = new List<List<SpawnEventData>>();

    private Stack<SpawnEvent> spawnEvents = new Stack<SpawnEvent>();
    private Stack<SpawnEvent> redoStack = new Stack<SpawnEvent>();
    private int currentStage;

    public event Action SpawnEventStackEmptied;
    public event Action SpawnEventStackNonZero;

    private void Awake()
    {
        spawnEventCollection.Add(SpawnSetsDB.LoadStage(0));

        if (isActive)
            InitCurrentStage();
    }
    private void FixedUpdate()
    {
        if (!isActive)
            return;

        // condition to spawn something via stack
        if (!timeKeeper.IsWorldRewinding && spawnEvents.Count > 0 && spawnEvents.Peek().timestamp == timeKeeper.CurrentWorldFrame)
        {
            var spawnEvent = spawnEvents.Pop();

            for (int i = 0; i < spawnEvent.spawnInfo.Length; i++)
            {
                if (!spawnEvent.spawnInfo[i].isDead)
                {
                    SpawnObject(spawnEvent, i);
                }
            }
            redoStack.Push(spawnEvent);
            if (spawnEvents.Count == 0)
                SpawnEventStackEmptied?.Invoke();
            // GC Collectable or some refs lingering?
        }
        // condition to put something on the spawn stack from the redo stack
        if (timeKeeper.IsWorldRewinding && redoStack.Count > 0 && redoStack.Peek().timestamp == timeKeeper.CurrentWorldFrame)
        {
            var e = redoStack.Pop();
            if (spawnEvents.Count == 0)
                SpawnEventStackNonZero?.Invoke();
            spawnEvents.Push(e);
        }
    }
    // Grabs the collection of SpawnEvents from the array with the currentStage index
    private void InitCurrentStage()
    {
        foreach (var spawnEvent in spawnEventCollection[currentStage].OrderByDescending(x => x.timestamp).ToList())
        {
            CreateSpawnEventFromEventData(spawnEvent);
        }
    }
    public void CreateSpawnEventFromEventData(SpawnEventData sED)
    {
        CreateSpawnEvent(sED.timestamp, sED.type, sED.dataID, new SpawnInfoBuilder(sED.count, sED.xStart, sED.yStart, sED.xGap, sED.yGap));
    }
    public void CreateSpawnEventFromSO(SpawnEventSO sO)
    {
        CreateSpawnEvent(sO.timestamp, sO.type, sO.dataID, new SpawnInfoBuilder(sO.count, sO.xStart, sO.yStart, sO.xGap, sO.yGap));
    }
    public void CreateSpawnEventFromSO(SpawnEventSO sO, int timestamp)
    {
        CreateSpawnEvent(timestamp, sO.type, sO.dataID, new SpawnInfoBuilder(sO.count, sO.xStart, sO.yStart, sO.xGap, sO.yGap));
        // Timestamp + Type + Count + Pattern
    }
    // The PoolableObject must implement ISpawnable, havent split the pool building into non-spawnables and spawnables.
    public void CreateSpawnEvent(int timestamp, PoolableObject typeSpawnable, int dataID, SpawnInfoBuilder infoBuilder)
    {
        var spawnable = poolManager.Prefabs[typeSpawnable].GetComponent<ISpawnable>();
        if (spawnable != null)
        {
            SpawnInfo[] spawnInfos = new SpawnInfo[infoBuilder.count];

            for (int i = 0; i < infoBuilder.count; i++)
            {
                spawnInfos[i].xPos = infoBuilder.xStart + (i * infoBuilder.xGap);
                spawnInfos[i].yPos = infoBuilder.yStart + (i * infoBuilder.yGap);
            }

            var spawnEvent = new SpawnEvent(timestamp, spawnable, dataID, spawnInfos);
            spawnEvents.Push(spawnEvent);
        }
        else
            Debug.Log("Called a non-spawnable object! Cant spawn that");
    }
    private void SpawnObject(SpawnEvent spawnEvent, int id)
    {
        var obj = poolManager.FetchPooledObject(spawnEvent.typeToSpawn.POType);
        obj.transform.position = new Vector3(spawnEvent.spawnInfo[id].xPos, spawnEvent.spawnInfo[id].yPos, 0);

        var iSpawnable = obj.GetComponent<ISpawnable>();
        iSpawnable.SpawnEvent = spawnEvent;
        iSpawnable.SpawnID = id;
        iSpawnable.EnemyData = EnemyDataMasterList[spawnEvent.enemyDataID];
        iSpawnable.OnSpawn();
    }

    // OLD / UNUSED
    // Timer / Observable based
    public IDisposable StartSpawning(PoolableObject po, int interval = 3)
    {
        // pauses but doesnt adjust its internal timing to rewinding
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(interval))
            .Where(_ => !timeKeeper.IsWorldRewinding)
            .Subscribe(_ => SpawnObject(po)).AddTo(this);
    }
    private void SpawnObject(PoolableObject poolableObject)
    {
        int x = UnityEngine.Random.Range(-50, 51);

        var obj = poolManager.FetchPooledObject(poolableObject);
        obj.transform.position = new Vector3(x, 48, 0);
        obj.GetComponent<ISpawnable>().OnSpawn();
    }
}