using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : MonoBehaviour, IHasHealth, IDamageable
{
    [SerializeField] public PlayerCore pCore;
    [SerializeField] public PlayerMovement pMove;
    [SerializeField] public PlayerOffense pOff;

    [TabGroup("Values"), SerializeField] private int health;

    public int Health { get => health; set => health = value; }

    public virtual void TakeHit(int value)
    {
        if (pCore.timeController.IsRewinding)
            return;

        Health -= value;
        if (Health <= 0)
        {
            pCore.gameManager.ResetScene();
        }
    }
    public virtual void TakeInvertedHit(int value)
    {
        if (!pCore.timeController.IsRewinding)
            return;

        Health -= value;
        if (Health <= 0)
        {
            pCore.gameManager.ResetScene();
        }
    }
}