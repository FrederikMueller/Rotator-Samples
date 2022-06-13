using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEvent", menuName = "ScriptableObjects/SpawnEvent")]
public class SpawnEventSO : ScriptableObject
{
    public int timestamp;
    public PoolableObject type;
    public int dataID;
    public int count;
    public float xStart, yStart, xGap, yGap;
}

public class SpawnEventData
{
    public int timestamp;
    public PoolableObject type;
    public int dataID;
    public int count;
    public float xStart, yStart, xGap, yGap;

    public SpawnEventData(int timestamp, PoolableObject basePrefab, int dataID, SpawnPatternBuilder pattern)
    {
        this.timestamp = timestamp;
        this.type = basePrefab;
        this.dataID = dataID;
        this.count = pattern.count;
        xStart = pattern.xStart;
        yStart = pattern.yStart;
        xGap = pattern.xGap;
        yGap = pattern.yGap;
    }
}
public struct SpawnInfo
{
    public bool isDead;
    public float xPos, yPos;
}
public struct SpawnInfoBuilder
{
    public int count;
    public float xStart, yStart;
    public float xGap, yGap;

    public SpawnInfoBuilder(int count, float xstart, float ystart, float xgap, float ygap)
    {
        this.count = count;
        xStart = xstart;
        yStart = ystart;
        xGap = xgap;
        yGap = ygap;
    }
}

public struct SpawnPatternBuilder
{
    public int count;
    public float xStart, yStart;
    public float xGap, yGap;

    public SpawnPatternBuilder(int count, float xstart, float ystart, float xgap, float ygap)
    {
        this.count = count;
        xStart = xstart;
        yStart = ystart;
        xGap = xgap;
        yGap = ygap;
    }
}