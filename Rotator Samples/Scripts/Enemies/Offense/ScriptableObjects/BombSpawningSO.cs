using UnityEngine;

[CreateAssetMenu(fileName = "BombSpawning", menuName = "ScriptableObjects/Offense/BombSpawning")]
public class BombSpawningSO : InjectableOffSO
{
    public float CD;
    public EnemyData BombData;
    public override void InjectLogic(OffComponent enemyOff) => enemyOff.Offense = new BombSpawning(enemyOff, CD, BombData);
}