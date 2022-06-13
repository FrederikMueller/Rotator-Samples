using UnityEngine;

public class InjectableDefSO : ScriptableObject
{
    public virtual void InjectLogic(DefComponent enemyDef)
    {
    }
}
public class InjectableOffSO : ScriptableObject
{
    public virtual void InjectLogic(OffComponent enemyOff)
    {
    }
}
public class InjectableMoveSO : ScriptableObject
{
    public virtual void InjectLogic(MoveComponent enemyMov)
    {
    }
}