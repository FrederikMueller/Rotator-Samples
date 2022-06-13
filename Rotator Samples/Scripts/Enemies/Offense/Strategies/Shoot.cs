using System;
using UniRx;
using UnityEngine;

public class Shoot : OffStrategyBase, IHasFireRate
{
    private bool onCD;

    public event Action<byte, int> BaseRateChanged;
    public float BaseRate { get; set; }
    private byte baseRateID;

    public Shoot(OffComponent enemyOff, float rate) : base(enemyOff) => BaseRate = rate;

    public override void Init()
    { }
    public override void ClearEvents()
    {
        BaseRateChanged = null;
    }

    public void OnBaseRateChanged()
    {
        BaseRateChanged?.Invoke(baseRateID, 2);
    }
    public override void Attack()
    {
        if (!onCD)
        {
            InitShot();
            onCD = true;
            var t = Observable.Timer(TimeSpan.FromSeconds(BaseRate))
                .Subscribe(_ => onCD = false);
        }
    }
    private void InitShot()
    {
        GameObject proj = Offense.eCore.GameManager.PoolManager.FetchPooledObject(PoolableObject.Projectile);
        proj.transform.position = Offense.eCore.nose.transform.position;

        proj.GetComponent<Projectile>().Launch(GameObject);
    }
}