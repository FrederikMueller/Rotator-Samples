public class SpawnEvent
{
    public int timestamp;
    public ISpawnable typeToSpawn; // must be spawnableobject
    public int enemyDataID;
    public SpawnInfo[] spawnInfo;

    public SpawnEvent(int timestamp, ISpawnable type, int enemydataID, SpawnInfo[] spawnInfos)
    {
        this.timestamp = timestamp;
        typeToSpawn = type;
        enemyDataID = enemydataID;
        spawnInfo = spawnInfos;
    }
}