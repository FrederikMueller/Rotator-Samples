using UnityEngine;

public class Bomb : OffStrategyBase
{
    // Values
    public float GrowthRate;

    // Logic
    private bool castedBomb;

    public Bomb(OffComponent enemyOff, float grate) : base(enemyOff)
    {
        GrowthRate = grate;
    }

    public override void Init()
    {
    }

    public override void Attack()
    {
        if (!castedBomb)
            InitBombEffect();
    }

    private void InitBombEffect()
    {
        GameObject be = Offense.eCore.GameManager.PoolManager.FetchPooledObject(PoolableObject.BombEffect);

        be.GetComponent<BombEffect>().GrowthRate = GrowthRate;
        be.GetComponent<BombEffect>().OnSpawn();

        Offense.eCore.OnDeathEvent += be.GetComponent<BombEffect>().Despawn;

        be.transform.position = Offense.transform.position;
        castedBomb = true;
    }

    public override void ClearEvents()
    {
    }
}