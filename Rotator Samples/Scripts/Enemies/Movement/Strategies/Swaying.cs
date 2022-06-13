using UnityEngine;

public class Swaying : MoveStrategyBase
{
    // Values
    public float horizontalSpeed;
    public int framesPerTurn;
    // Logic
    private float hSpeedBase;
    private int counter;

    public override float BaseSpeed { get; set; }
    public Swaying(MoveComponent enemyMov, float vSpeed, float hSpeed, int fpTurn) : base(enemyMov)
    {
        BaseSpeed = vSpeed;
        horizontalSpeed = hSpeed;
        framesPerTurn = fpTurn;
    }

    public override void Init()
    {
        Movement.Speed = BaseSpeed;
        hSpeedBase = horizontalSpeed;

        Movement.OnResetMovementSpeed += () => Movement.Speed = BaseSpeed;
        Movement.OnResetMovementSpeed += () => horizontalSpeed = hSpeedBase;

        Movement.OnZeroMovementSpeed += () => Movement.Speed = 0;
        Movement.OnZeroMovementSpeed += () => horizontalSpeed = 0;

        counter = framesPerTurn / 2;
    }
    // stuff like this neeeds to be recorded because it will super weird if you can glitch the rhythm of an object just because of an underlying counter
    public override void Move()
    {
        if (counter >= framesPerTurn)
        {
            counter = 0;
            horizontalSpeed *= -1;
        }
        GameObject.transform.position += new Vector3(horizontalSpeed, -Movement.Speed, 0f);
        counter++;
        UpdatePosition();
    }

    public override void ClearEvents()
    { }
}