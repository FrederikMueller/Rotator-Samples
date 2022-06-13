using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "ScriptableObjects/Movement/Asteroid")]
public class AsteroidSO : InjectableMoveSO
{
    public float speed;
    public override void InjectLogic(MoveComponent enemyMov) => enemyMov.Movement = new Asteroid(enemyMov, speed);
}