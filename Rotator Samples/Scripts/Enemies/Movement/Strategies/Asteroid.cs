using UnityEngine;

public class Asteroid : MoveStrategyBase
{
    // Values
    public override float BaseSpeed { get; set; }
    // Logic
    private float f;
    private float xSpeed;

    public Asteroid(MoveComponent enemyMov, float speed) : base(enemyMov) => BaseSpeed = speed;

    public override void Init()
    {
        Movement.Speed = BaseSpeed;
        xSpeed = Random.Range(-.5f, .5f);
    }

    public override void Move()
    {
        GameObject.transform.position += new Vector3(xSpeed * Movement.Speed, -Movement.Speed, 0f);
        UpdatePosition();
        Rotate();
        f++;
    }
    public void Rotate()
    {
        GameObject.transform.rotation = Quaternion.AngleAxis(f, Vector3.forward);
        Movement.ZRotUpdate();
    }

    public override void ClearEvents()
    { }
}