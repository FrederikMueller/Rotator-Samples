using UnityEngine;

[CreateAssetMenu(fileName = "PercMitigation", menuName = "ScriptableObjects/Defense/PercMitigation")]
public class PercMitigationSO : InjectableDefSO
{
    public float BaseMitigation;
    public override void InjectLogic(DefComponent enemyDef)
    {
        enemyDef.Defense = new PercMitigation(enemyDef, BaseMitigation);
    }
}