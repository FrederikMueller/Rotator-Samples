using UnityEngine;

[CreateAssetMenu(fileName = "Chasing", menuName = "ScriptableObjects/Movement/Chasing")]
public class ChasingSO : InjectableMoveSO
{
    public float speed;
    public override void InjectLogic(MoveComponent enemyMov) => enemyMov.Movement = new Chasing(enemyMov, speed);
}