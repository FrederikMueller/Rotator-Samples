using System;
using UnityEngine;

public class OffComponent : MonoBehaviour
{
    [HideInInspector] public EnemyCoreNew eCore;
    [HideInInspector] public MoveComponent eMov;
    [HideInInspector] public DefComponent eDef;

    // Events

    // State
    public OffStrategyBase Offense { get; set; }
    [HideInInspector] public bool canAttack;
    public GameObject target;

    // Methods
    private void Awake()
    {
        eCore = GetComponent<EnemyCoreNew>();
        eMov = GetComponent<MoveComponent>();
        eDef = GetComponent<DefComponent>();
    }
    public void Init()
    {
        Offense.Init();
        eCore.OnFixedUpdateForward += Offense.Attack;
        target = eCore.GameManager.PlayerForward;
    }
    public void FullReset()
    {
        Offense.ClearEvents();
        Offense = null;

        canAttack = true;
        target = null;
    }
}