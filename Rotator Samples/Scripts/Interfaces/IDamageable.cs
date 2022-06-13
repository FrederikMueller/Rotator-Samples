public interface IDamageable
{
    void TakeHit(int amt);
    void TakeInvertedHit(int amt);
}

public interface IInjectableDefense
{
    void Inject(DefComponent enemyDef);
}

public interface IHasFireRate
{
    float BaseRate { get; set; }
}