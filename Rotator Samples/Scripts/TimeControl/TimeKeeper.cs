using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    // First I wanna try to have a reusable list / datastructure of stacks that can keep track of all objects in play.
    // Then it will be easy to selectively and globally rewind objects.
    // The data structure sits on the timekeeper and each object has a reference to its own stack. The object adds their own state to its own stack every fixed frame.
    // When a rewind is triggered, it has to go through the timekeeper. The timekeeper will then start and stop the rewind process on the object.

    // You probably want to use different lists for different objects, so that you can use different kinds of mementos to save space and allow for more state being saved for impportant objects.
    //public List<Stack<BasicMemento>> BasicHistories;
    //public List<Stack<ProjectileMemento>> ProjectileHistories;
    // List of Stacks of all used memento types (keep efficient, small)
    // PlayerHistory, BossHistory, BasicHistory, ProjectileHistory...

    // Small lists of reusable stacks so we dont resize the larger list. Not sure how important this is.
    public int CurrentWorldFrame { get; set; }
    public bool IsWorldRewinding { get; set; }
    [SerializeField] private TMP_Text worldFrameTxt;
    // You want a list of each type of object, separately because that will save a ton of time as long as you keep the type rewinding in the game.
    public List<IDynamic> Projectiles;
    public List<IDynamic> InvertedProjectiles;
    public List<IDynamic> Players;
    public List<IDynamic> InvertedPlayers;
    public List<IDynamic> Enemies;
    public List<IDynamic> InvertedEnemies;
    public List<IDynamic> Asteroids;
    public List<IDynamic> InvertedAsteroids;

    public event Action GlobalRewindEvent;
    public event Action GlobalForwardEvent;
    public Action<PoolableObject, int> taction;

    //public Stack<WorldEvent> worldEvents;
    // Command Pattern or something like that
    // Either store stack here, or have each object store their own command / event and check for CurrentWorldFrame, which is kindof trash because of wasteful calls.
    // Have each object with a possible event have the capability to add its event to the worldstack
    // how would i do that based on my current knowledge?
    private void Awake()
    {
        //BasicHistories = new List<Stack<BasicMemento>>();
        //ProjectileHistories = new List<Stack<ProjectileMemento>>();

        Projectiles = new List<IDynamic>();
        Enemies = new List<IDynamic>();
        Players = new List<IDynamic>();
        Asteroids = new List<IDynamic>();
        InvertedProjectiles = new List<IDynamic>();
        InvertedEnemies = new List<IDynamic>();
        InvertedPlayers = new List<IDynamic>();
        InvertedAsteroids = new List<IDynamic>();
        // List of Gameobjects or list of trackers that each objects holds
    }
    private void FixedUpdate()
    {
        if (IsWorldRewinding)
            CurrentWorldFrame--;
        else
            CurrentWorldFrame++;
        worldFrameTxt.text = CurrentWorldFrame.ToString();
    }
    // Registering objects to keep track of them in lists
    public void RegisterObject(IDynamic obj)
    {
        //Debug.Log($"Registered a {obj.DynamicType}.");
        switch (obj.DynamicType)
        {
            case DynamicType.Enemy:
                Enemies.Add(obj);
                break;
            case DynamicType.Player:
                Players.Add(obj);
                break;
            case DynamicType.Projectile:
                Projectiles.Add(obj);
                break;
            case DynamicType.Asteroid:
                Asteroids.Add(obj);
                break;
            case DynamicType.InvertedEnemy:
                InvertedEnemies.Add(obj);
                break;
            case DynamicType.InvertedPlayer:
                InvertedPlayers.Add(obj);
                break;
            case DynamicType.InvertedProjectile:
                InvertedProjectiles.Add(obj);
                break;
            case DynamicType.InvertedAsteroid:
                InvertedAsteroids.Add(obj);
                break;
            default:
                Debug.Log("Relevant list not found.");
                break;
        }
    }
    public void DeRegisterObject(IDynamic obj)
    {
        switch (obj.DynamicType)
        {
            case DynamicType.Enemy:
                Enemies.Remove(obj);
                break;
            case DynamicType.Player:
                Players.Remove(obj);
                break;
            case DynamicType.Projectile:
                Projectiles.Remove(obj);
                break;
            case DynamicType.Asteroid:
                Asteroids.Remove(obj);
                break;
            case DynamicType.InvertedEnemy:
                InvertedEnemies.Remove(obj);
                break;
            case DynamicType.InvertedPlayer:
                InvertedPlayers.Remove(obj);
                break;
            case DynamicType.InvertedProjectile:
                InvertedProjectiles.Remove(obj);
                break;
            case DynamicType.InvertedAsteroid:
                InvertedAsteroids.Remove(obj);
                break;
            default:
                Debug.Log("Relevant list not found.");
                break;
        }
    }

    // Time Manipulation on objects in lists
    public void GlobalRewind()
    {
        GlobalRewindEvent?.Invoke();
        IsWorldRewinding = true;

        TypeRewind(DynamicType.Enemy);
        TypeRewind(DynamicType.Projectile);
        TypeRewind(DynamicType.Player);
        TypeRewind(DynamicType.Asteroid);

        TypeForward(DynamicType.InvertedEnemy);
        TypeForward(DynamicType.InvertedProjectile);
        TypeForward(DynamicType.InvertedPlayer);
        TypeForward(DynamicType.InvertedAsteroid);
    }
    public void GlobalForward()
    {
        GlobalForwardEvent?.Invoke();
        IsWorldRewinding = false;

        TypeForward(DynamicType.Enemy);
        TypeForward(DynamicType.Projectile);
        TypeForward(DynamicType.Player);
        TypeForward(DynamicType.Asteroid);

        TypeRewind(DynamicType.InvertedEnemy);
        TypeRewind(DynamicType.InvertedProjectile);
        TypeRewind(DynamicType.InvertedPlayer);
        TypeRewind(DynamicType.InvertedAsteroid);
    }
    public void TypeRewind(DynamicType dynamicType)
    {
        switch (dynamicType)
        {
            case DynamicType.Enemy:
                foreach (var obj in Enemies)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.Player:
                foreach (var obj in Players)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.Projectile:
                foreach (var obj in Projectiles)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.Asteroid:
                foreach (var obj in Asteroids)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.InvertedEnemy:
                foreach (var obj in InvertedEnemies)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.InvertedPlayer:
                foreach (var obj in InvertedPlayers)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.InvertedProjectile:
                foreach (var obj in InvertedProjectiles)
                {
                    obj.Rewind();
                }
                break;
            case DynamicType.InvertedAsteroid:
                foreach (var obj in InvertedAsteroids)
                {
                    obj.Rewind();
                }
                break;
            default:
                Debug.Log("Relevant list not found.");
                break;
        }
    }
    public void TypeForward(DynamicType dynamicType)
    {
        switch (dynamicType)
        {
            case DynamicType.Enemy:
                foreach (var obj in Enemies)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.Player:
                foreach (var obj in Players)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.Projectile:
                foreach (var obj in Projectiles)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.Asteroid:
                foreach (var obj in Asteroids)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.InvertedEnemy:
                foreach (var obj in InvertedEnemies)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.InvertedPlayer:
                foreach (var obj in InvertedPlayers)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.InvertedProjectile:
                foreach (var obj in InvertedProjectiles)
                {
                    obj.Forward();
                }
                break;
            case DynamicType.InvertedAsteroid:
                foreach (var obj in InvertedAsteroids)
                {
                    obj.Forward();
                }
                break;
            default:
                Debug.Log("Relevant list not found.");
                break;
        }
    }

    // Helper Stuff
    public void StopTime(float duration, float timescaleAfterPause = 1f)
    {
        Time.timeScale = 0;
        Observable
            .Timer(TimeSpan.FromSeconds(duration), Scheduler.MainThreadIgnoreTimeScale)
            .Subscribe(_ => Time.timeScale = timescaleAfterPause);
    }
}