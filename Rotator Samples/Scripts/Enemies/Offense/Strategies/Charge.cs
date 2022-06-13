using System;
using UniRx;
using UnityEngine;

public class Charge : OffStrategyBase
{
    // Values
    public float CD;
    public float CastTime;
    public float ChargeDuration;
    public float ChargeSpeed;
    // Logic
    private bool isCharging;
    public bool onCD;

    public Charge(OffComponent enemyOff, float cd, float casttime, float cDur, float cSpeed) : base(enemyOff)
    {
        CD = cd;
        CastTime = casttime;
        ChargeDuration = cDur;
        ChargeSpeed = cSpeed;
    }

    public override void Init()
    { }

    public override void Attack()
    {
        if (!onCD)
            StartCharge();
        if (isCharging)
            ChargeMovement();
    }
    private void StartCharge()
    {
        onCD = true;
        var s = Observable.Timer(TimeSpan.FromSeconds(CastTime)).Subscribe(_ => ChargeCast()).AddTo(GameObject);
    }
    public void ChargeCast()
    {
        // Timer-based calls can resolve during outside the fixed update ofc, so they would break the cycle in a sense.
        // this chargecast can resolve when the obj is already rewinding for example
        // clear all timers on rewind or just return out of the call

        isCharging = true;

        var dir = Offense.target.transform.position - Offense.eMov.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Offense.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Offense.eMov.ZRotUpdate();

        Offense.eMov.ZeroMovementSpeed();
        Offense.eMov.DisableRotation();

        var t = Observable.Timer(TimeSpan.FromSeconds(CD)).Subscribe(_ => onCD = false);
        var s = Observable.Timer(TimeSpan.FromSeconds(ChargeDuration)).Subscribe(_ => EndOfCharge());
    }
    public void EndOfCharge()
    {
        isCharging = false;
        Offense.eMov.ResetMovementSpeed();
        Offense.eMov.ResetRotation();
    }
    public void ChargeMovement()
    {
        GameObject.transform.position += new Vector3(GameObject.transform.up.x * ChargeSpeed, GameObject.transform.up.y * ChargeSpeed, GameObject.transform.up.z);
        Offense.eMov.XPosUpdate();
        Offense.eMov.YPosUpdate();
    }

    public override void ClearEvents()
    { }
}