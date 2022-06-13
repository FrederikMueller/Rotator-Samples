using UnityEngine;

public class StraightDown : MoveStrategyBase
{
    public override float BaseSpeed { get; set; }
    public StraightDown(MoveComponent enemyMov, float speed) : base(enemyMov) => BaseSpeed = speed;

    public override void Init()
    {
        Movement.Speed = BaseSpeed;
        Movement.OnResetMovementSpeed += () => Movement.Speed = BaseSpeed;
        Movement.OnZeroMovementSpeed += () => Movement.Speed = 0;
    }

    public override void Move()
    {
        GameObject.transform.position += new Vector3(0f, -Movement.Speed, 0f);
        UpdatePosition();
    }

    public override void ClearEvents()
    { }
}