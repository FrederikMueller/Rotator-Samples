using System;
using UnityEngine;

public class DefComponent : MonoBehaviour, IHasHealth
{
    // Fundamental
    [HideInInspector] public EnemyCoreNew eCore;
    [HideInInspector] public MoveComponent eMov;
    [HideInInspector] public OffComponent eOff;

    // Events
    public event Action<int> HealthChanged;
    // State
    public DefStrategyBase Defense { get; set; }
    public int Health { get; set; }

    private void Awake()
    {
        eCore = GetComponent<EnemyCoreNew>();
        eMov = GetComponent<MoveComponent>();
        eOff = GetComponent<OffComponent>();
    }
    public void Init()
    {
        Health = eCore.EnemyData.maxHealth;
        Defense.Init();

        var t = eCore.timeController.AddInt(ref HealthChanged);
        t.OutputEvent += ModifyHealth;
    }
    public void FullReset()
    {
        Defense.ClearEvents();
        Defense = null;

        Health = 0;

        HealthChanged = null;
    }

    public void ReduceHealth(int value)
    {
        Health -= value;
        HealthChanged.Invoke(value);
        CheckDeathCondition();
    }
    public void ModifyHealth(int value)
    {
        Health += value;
        Debug.Log("modify health called");
        CheckDeathCondition();
    }
    private void CheckDeathCondition()
    {
        if (Health <= 0)
        {
            eCore.OnDeath();
        }
    }
}