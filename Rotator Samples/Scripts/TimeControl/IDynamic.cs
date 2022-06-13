public enum DynamicType { Enemy, Player, Projectile, Asteroid, Effect, InvertedEnemy, InvertedPlayer, InvertedProjectile, InvertedAsteroid, InvertedEffect };

public interface IDynamic
{
    int BeginningIndex { get; }
    int CurrentIndex { get; }
    int EndIndex { get; }

    void Rewind();
    void Forward();

    DynamicType DynamicType { get; }
}