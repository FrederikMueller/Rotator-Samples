using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "ScriptableObjects/Offense/Shoot")]
public class ShootSO : InjectableOffSO
{
    public float BaseRate;
    public override void InjectLogic(OffComponent enemyOff) => enemyOff.Offense = new Shoot(enemyOff, BaseRate);
}