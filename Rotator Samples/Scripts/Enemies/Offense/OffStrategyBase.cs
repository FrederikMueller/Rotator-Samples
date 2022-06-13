using UnityEngine;

public abstract class OffStrategyBase : IOffStrategy
{
    protected GameObject GameObject { get; set; }
    protected OffComponent Offense { get; set; }

    public OffStrategyBase(OffComponent enemyOff)
    {
        Offense = enemyOff;
        GameObject = enemyOff.gameObject;
    }
    public abstract void Init();
    public abstract void ClearEvents();
    public abstract void Attack();
}