using UnityEngine;

// Test / Decide whether to use scriptableobjects for the strategies at this level or some lower level and how that all would relate
public abstract class DefStrategyBase : IDefStrategy
{
    protected GameObject GameObject { get; set; }
    protected DefComponent Defense { get; set; }

    public DefStrategyBase(DefComponent enemyDef)
    {
        Defense = enemyDef;
        GameObject = enemyDef.gameObject;
    }
    public abstract void Init();
    public abstract void ClearEvents();
    public abstract void TakeHit(int amt);
    public abstract void TakeInvertedHit(int amt);
}