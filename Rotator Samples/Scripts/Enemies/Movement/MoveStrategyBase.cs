using UnityEngine;

public abstract class MoveStrategyBase : IMoveStrategy
{
    protected GameObject GameObject { get; set; }
    protected MoveComponent Movement { get; set; }
    public abstract float BaseSpeed { get; set; }

    public MoveStrategyBase(MoveComponent enemyMov)
    {
        Movement = enemyMov;
        GameObject = enemyMov.gameObject;
    }
    public abstract void Init();
    public abstract void ClearEvents();
    public abstract void Move();
    public void UpdatePosition()
    {
        Movement.XPosUpdate();
        Movement.YPosUpdate();
    }
}