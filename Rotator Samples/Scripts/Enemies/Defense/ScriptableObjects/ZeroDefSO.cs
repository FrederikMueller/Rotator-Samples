using UnityEngine;

[CreateAssetMenu(fileName = "ZeroDef", menuName = "ScriptableObjects/Defense/ZeroDef")]
public class ZeroDefSO : InjectableDefSO
{
    public override void InjectLogic(DefComponent enemyDef) => enemyDef.Defense = new ZeroDef(enemyDef);
}