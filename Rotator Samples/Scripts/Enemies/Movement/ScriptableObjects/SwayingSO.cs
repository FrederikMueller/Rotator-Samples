using UnityEngine;

[CreateAssetMenu(fileName = "Swaying", menuName = "ScriptableObjects/Movement/Swaying")]
public class SwayingSO : InjectableMoveSO
{
    public float horizontalSpeed;
    public float verticalSpeed;
    public int framesPerTurn;
    public override void InjectLogic(MoveComponent enemyMov) => enemyMov.Movement = new Swaying(enemyMov, verticalSpeed, horizontalSpeed, framesPerTurn);
}