using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    // ENEMY DATA TBI
    public string enemyName;
    public string description;

    public Vector3 Scale;
    public Sprite Model;
    public bool ScaleModelToHitbox;

    public InjectableDefSO Defense;
    public InjectableMoveSO Movement;
    public InjectableOffSO Offense;

    public int maxHealth;
    public PoolableObject POType;
    public bool isReversed;
}