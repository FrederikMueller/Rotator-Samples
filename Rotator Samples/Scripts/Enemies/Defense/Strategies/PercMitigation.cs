using System;
using UnityEngine;

public class PercMitigation : DefStrategyBase
{
    public PercMitigation(DefComponent enemyDef, float mitigation) : base(enemyDef) => Mitigation = mitigation;

    public float Mitigation;
    public event Action<float> MitigationChanged;
    public override void Init()
    {
        var t = Defense.eCore.timeController.AddFloat(ref MitigationChanged);
        t.OutputEvent += UpdateMitigationValue;
    }
    public override void ClearEvents()
    {
        MitigationChanged = null;
    }
    public void UpdateMitigationValue(float diff)
    {
        Mitigation -= diff;
    }
    public void ModifyMitigationValue(float diff)
    {
        Mitigation += diff;
        MitigationChanged?.Invoke(Mitigation);
    }
    public override void TakeHit(int amt)
    {
        if (!Defense.eCore.timeController.IsRewinding)
        {
            var dmg = Mathf.RoundToInt(amt * (1 - Mitigation));
            Defense.ReduceHealth(dmg);
        }
    }
    public override void TakeInvertedHit(int amt)
    {
        if (Defense.eCore.timeController.IsRewinding)
        {
            var dmg = Mathf.RoundToInt(amt * (1 - Mitigation));
            Defense.ReduceHealth(dmg);
        }
    }
}