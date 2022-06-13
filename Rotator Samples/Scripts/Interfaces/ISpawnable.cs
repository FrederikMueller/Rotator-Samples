public interface ISpawnable : IPoolable
{
    SpawnEvent SpawnEvent { get; set; }
    int SpawnID { get; set; }
    EnemyData EnemyData { get; set; }
    void OnSpawn();
    void TCEventSubs();
}

public interface IPoolable
{
    PoolableObject POType { get; }
}