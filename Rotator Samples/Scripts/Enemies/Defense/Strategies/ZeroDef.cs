public class ZeroDef : DefStrategyBase
{
    public ZeroDef(DefComponent enemyDef) : base(enemyDef)
    { }
    public override void Init()
    {
    }
    public override void ClearEvents()
    {
    }
    public override void TakeHit(int amt)
    {
        if (!Defense.eCore.timeController.IsRewinding)
        {
            Defense.ReduceHealth(amt);
        }
    }
    public override void TakeInvertedHit(int amt)
    {
        if (Defense.eCore.timeController.IsRewinding)
        {
            Defense.ReduceHealth(amt);
        }
    }
}