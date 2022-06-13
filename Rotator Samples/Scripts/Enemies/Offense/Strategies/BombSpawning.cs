using System;
using UniRx;
using UnityEngine;

public class BombSpawning : OffStrategyBase
{
    // Values
    public float CD;
    public EnemyData BombEnemyData;

    // Logic
    private bool onCD;

    public BombSpawning(OffComponent enemyOff, float cd, EnemyData bombData) : base(enemyOff)
    {
        CD = cd;
        BombEnemyData = bombData;
    }
    public override void Init()
    { }

    public override void Attack()
    {
        if (!onCD)
        {
            SpawnBomb();
            onCD = true;
            var t = Observable.Timer(TimeSpan.FromSeconds(CD))
                .Subscribe(_ => onCD = false);
        }
    }
    private void SpawnBomb()
    {
        Debug.Log("bomb spawn");

        GameObject b = Offense.eCore.GameManager.PoolManager.FetchPooledObject(PoolableObject.Bomb);
        b.GetComponent<EnemyCore>().InjectEnemyData(BombEnemyData);
        b.GetComponent<EnemyCore>().OnSpawn();

        var xShift = UnityEngine.Random.Range(-25, 25);
        var yShift = UnityEngine.Random.Range(-25, 25);
        b.transform.position = new Vector3(Offense.transform.position.x + xShift, Offense.transform.position.y + yShift, 0f);
    }

    public override void ClearEvents()
    {
    }
}