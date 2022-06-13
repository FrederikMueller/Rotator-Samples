using UnityEngine;

[CreateAssetMenu(fileName = "StraightDown", menuName = "ScriptableObjects/Movement/StraightDown")]
public class StraightDownSO : InjectableMoveSO
{
    public float speed;
    public override void InjectLogic(MoveComponent enemyMov) => enemyMov.Movement = new StraightDown(enemyMov, speed);
}