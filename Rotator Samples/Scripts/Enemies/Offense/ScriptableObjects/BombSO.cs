using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "ScriptableObjects/Offense/Bomb")]
public class BombSO : InjectableOffSO
{
    public float GrowthRate;
    public override void InjectLogic(OffComponent enemyOff) => enemyOff.Offense = new Bomb(enemyOff, GrowthRate);
}