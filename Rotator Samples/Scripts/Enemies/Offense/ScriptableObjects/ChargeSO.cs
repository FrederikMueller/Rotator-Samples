using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "ScriptableObjects/Offense/Charge")]
public class ChargeSO : InjectableOffSO
{
    public float CD;
    public float CastTime;
    public float ChargeDuration;
    public float ChargeSpeed;
    public override void InjectLogic(OffComponent enemyOff) => enemyOff.Offense = new Charge(enemyOff, CD, CastTime, ChargeDuration, ChargeSpeed);
}