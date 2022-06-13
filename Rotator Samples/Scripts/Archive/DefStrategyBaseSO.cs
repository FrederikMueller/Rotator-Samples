using UnityEngine;

// Test / Decide whether to use scriptableobjects for the strategies at this level or some lower level and how that all would relate
public abstract class DefStrategyBaseSO : ScriptableObject, IDefStrategy
{
    protected GameObject GameObject { get; set; }
    protected DefComponent DefComponent { get; set; }

    public abstract DefStrategyBaseSO GetInstance();

    public void Init(DefComponent enemyDef)
    {
        DefComponent = enemyDef;
        GameObject = enemyDef.gameObject;
    }
    public abstract void TakeHit(int amt);
    public abstract void TakeInvertedHit(int amt);
}