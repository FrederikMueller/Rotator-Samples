using System;
using System.Collections.Generic;

public enum SpawnPattern { TightLine, Square }
public enum SpawnPosition { Top, TopLeft, TopRight }
// For now the fastest workflow I can think of is to just create an enemydata SO, set it up how you want it to be, then add it to a master list and simply use the
// index of that to get it. Over time you will then create an enum for enemydata SOs you're using a lot. Then you can ref that or an ID and make an overload
// for the add spawn call for enum and int. Enum for already established enemydata SOs and int for just added/specialized stuff.
public static class SpawnSetsDB
{
    public static List<SpawnEventData> LoadStage(int id)
    {
        List<SpawnEventData> events = new List<SpawnEventData>();
        // Currently the logic is to reset the global timestamp for each "Stage".
        switch (id)
        {
            //            Stage Timestamp to spawn on   Enemy to Spawn (base prefab + data)   Num Enemies    Pattern    xCenter, ycenter
            //            Stage Timestamp to spawn on   Enemy to Spawn (base prefab + data)   Num Enemies  xgap, ygap   xCenter, ycenter
            case 0:
                events.Add(new SpawnEventData(101, PoolableObject.BasicEnemy, 0, GetPattern(5, SpawnPattern.TightLine, -20, 45)));
                events.Add(new SpawnEventData(204, PoolableObject.BasicEnemy, 1, GetPattern(5, SpawnPattern.TightLine, 20, 60)));
                //events.Add(new SpawnEventData(304, PoolableObject.BasicEnemy, 2, GetPattern(4, SpawnPattern.TightLine, 0, 45)));
                events.Add(new SpawnEventData(404, PoolableObject.BasicEnemy, 1, GetPattern(5, SpawnPattern.TightLine, -20, 60)));
                events.Add(new SpawnEventData(306, PoolableObject.BasicEnemy, 0, GetPattern(11, SpawnPattern.TightLine, -10, 60)));
                events.Add(new SpawnEventData(506, PoolableObject.BasicEnemy, 1, GetPattern(1, SpawnPattern.TightLine, 0, 60)));
                events.Add(new SpawnEventData(706, PoolableObject.BasicEnemy, 1, GetPattern(1, SpawnPattern.TightLine, 10, 60)));
                events.Add(new SpawnEventData(806, PoolableObject.BasicEnemy, 1, GetPattern(1, SpawnPattern.TightLine, 30, 60)));
                events.Add(new SpawnEventData(906, PoolableObject.BasicEnemy, 1, GetPattern(1, SpawnPattern.TightLine, 40, 60)));
                events.Add(new SpawnEventData(1006, PoolableObject.BasicEnemy, 3, GetPattern(10, SpawnPattern.TightLine, -10, 60)));

                //events.Add(new SpawnEventData(102, PoolableObject.Blocker, GetPattern(4, SpawnPattern.TightLine, -20, 40)));
                //events.Add(new SpawnEventData(100, PoolableObject.Blocker, GetPattern(4, SpawnPattern.TightLine, -20, 50)));

                //events.Add(new SpawnEventData(201, PoolableObject.Chaser, GetPattern(3, SpawnPattern.TightLine, +25, 45)));
                //events.Add(new SpawnEventData(202, PoolableObject.Chaser, GetPattern(3, SpawnPattern.TightLine, +25, 40)));
                //events.Add(new SpawnEventData(200, PoolableObject.Chaser, GetPattern(3, SpawnPattern.TightLine, +25, 50)));

                //events.Add(new SpawnEventData(302, PoolableObject.Shooter, GetPattern(3, 6, 0, 0, 40)));
                //events.Add(new SpawnEventData(300, PoolableObject.Shooter, GetPattern(4, 9, 0, 0, 45)));
                //events.Add(new SpawnEventData(301, PoolableObject.Shooter, GetPattern(5, 12, 0, 0, 50)));

                //events.Add(new SpawnEventData(500, PoolableObject.Shooter, GetPattern(10, 10, 0, 0, 35)));
                return events;
            case 1:

                return events;

            default:
                return null;
        }
    }

    // Enums for patterns. Count of mobs. Enum for pattern position. Generate from there.
    public static SpawnPatternBuilder GetPattern(int count, SpawnPattern patternName, float xCenter, float yCenter)
    {
        switch (patternName)
        {
            case SpawnPattern.TightLine:
                float xgap = 5;
                float ygap = 0;
                float xStart = xCenter - ((count - 1) * xgap / 2);
                float yStart = yCenter - ((count - 1) * ygap / 2);

                return new SpawnPatternBuilder(count, xStart, yStart, xgap, ygap);

            default:
                return new SpawnPatternBuilder(0, 0, 0, 0, 0);
        }
    }
    public static SpawnPatternBuilder GetPattern(int count, float xgap, float ygap, float xCenter, float yCenter)
    {
        float xSpan = (count - 1) * xgap;
        float ySpan = (count - 1) * ygap;
        float xStart = xCenter - (xSpan / 2f);
        float yStart = yCenter - (ySpan / 2f);

        return new SpawnPatternBuilder(count, xStart, yStart, xgap, ygap);
    }
    public static float EnumToWorldPos(SpawnPosition spawnPosition)
    {
        switch (spawnPosition)
        {
            case SpawnPosition.Top:
                return 0;
            case SpawnPosition.TopLeft:
                return -20;
            case SpawnPosition.TopRight:
                return 20;
            default:
                return 0;
        }
    }
}