using UnityEngine;

[CreateAssetMenu(fileName = "Nothing", menuName = "ScriptableObjects/Offense/Nothing")]
public class NothingSO : InjectableOffSO
{
    public override void InjectLogic(OffComponent enemyOff) => enemyOff.Offense = new Nothing(enemyOff);
}