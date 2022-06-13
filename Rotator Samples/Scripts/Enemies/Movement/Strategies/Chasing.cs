using UnityEngine;

public class Chasing : MoveStrategyBase
{
    public override float BaseSpeed { get; set; }
    public Chasing(MoveComponent enemyMov, float speed) : base(enemyMov) => BaseSpeed = speed;

    public override void Init()
    {
        Movement.Speed = BaseSpeed;
        Movement.CanRotateBaseline = true;
        Movement.CanRotate = true;
        Movement.OnResetMovementSpeed += () => Movement.Speed = BaseSpeed;
        Movement.OnZeroMovementSpeed += () => Movement.Speed = 0;
    }

    public override void Move()
    {
        var newPos = new Vector3(GameObject.transform.up.x * Movement.Speed, GameObject.transform.up.y * Movement.Speed, GameObject.transform.up.z);
        GameObject.transform.position += newPos;
        UpdatePosition();
    }

    public override void ClearEvents()
    { }
}