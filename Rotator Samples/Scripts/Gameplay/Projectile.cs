using Sirenix.OdinInspector;
using System;
using TimeControl;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    // Type Information
    public TCPosition timeController;
    // Stats & Values
    public int Damage { get; private set; }
    public virtual PoolableObject POType { get => PoolableObject.Projectile; }

    [SerializeField] private float baseSpeed;
    [SerializeField] private int baseDamage;

    private float currentSpeed;

    // Management
    protected GameManager GameManager;
    protected TimeKeeper TimeKeeper;
    public GameObject owner;
    public GameObject model;
    [SerializeField] public Sprite[] sprites;

    public bool inGracePeriod;
    public bool inPool = true;
    public bool inactive;
    private int stacksizeOnInactive;

    public void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
        TimeKeeper = GameManager.TimeKeeper;
        timeController = GetComponent<TCPosition>();

        timeController.RewindCalled += Activate;
        timeController.RewindCalled += () => Debug.Log("rewind called");
        timeController.ForwardCalled += Activate;
        timeController.ForwardCalled += () => Debug.Log("forward called");

        timeController.ReachedBeginning += AbsorbAndReset;

        Damage = baseDamage;
    }
    public void Setup()
    {
    }
    private void FixedUpdate()
    {
        if (inPool)
            return;

        if (!timeController.IsRewinding)
        {
            if (inactive && timeController.CurrentIndex > stacksizeOnInactive + 1000)
            {
                FullReset();
                return;
            }
            gameObject.transform.position += gameObject.transform.up * currentSpeed;
            timeController.ProgressForwardsInHistory();
            OutOfBoundsCheck();
        }
        else // Reversed
        {
            timeController.ProgressBackwardsInHistory();
        }
    }

    private void AbsorbAndReset()
    {
        // reached beginning
        if (owner.gameObject.tag == "Player")
            owner.gameObject.GetComponent<PlayerOffense>().AbsorbProjectile();
        SetInactive();

        FullReset();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (inPool || inactive)
            return;

        if (owner.tag == "Player")
        {
            PlayerOwnedProjectileHit(other);
        }
        else
        {
            EnemyOwnedProjectilehit(other);
        }
    }
    // Projectile Hits
    protected virtual void EnemyOwnedProjectilehit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (other.gameObject.tag == "Player")
            {
                SetInactive();
                other.GetComponent<PlayerDefense>().TakeHit(this.Damage);
            }
            else
            {
                // No interaction between enemy projectiles and enemy ships
            }
        }
    }
    protected virtual void PlayerOwnedProjectileHit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (other.gameObject.tag == "BasicEnemy")
            {
                if (timeController.DynamicType == DynamicType.Projectile)
                {
                    other.GetComponent<DefComponent>().Defense.TakeHit(Damage);
                    //other.GetComponent<MoveComponent>().Speed = 0;
                    //if (other.GetComponent<OffComponent>().Offense is IHasFireRate ocfr)
                    //    ocfr.BaseRate = 10;
                }
                else if (timeController.DynamicType == DynamicType.InvertedProjectile)
                    other.GetComponent<DefComponent>().Defense.TakeInvertedHit(Damage);

                SetInactive();
                other.gameObject.GetComponent<EnemyCoreNew>().OnDeathEvent += FullReset;
                // unsub if proj is rewinded out of the enemy
            }
            if (other.gameObject.tag == "Player" && (timeController.CurrentIndex > 50 || timeController.IsRewinding) && !other.GetComponent<TCPlayer>().IsRewinding)
            {
                other.gameObject.GetComponent<PlayerOffense>().AbsorbProjectile();
                SetInactive();
                FullReset();
            }
        }
    }

    // Helper Methods
    public void FullReset()
    {
        // De-Register from TimeKeeper
        TimeKeeper.DeRegisterObject(timeController);
        // Reset History by zeroing out these values. The array still exists and can be reused.
        // Reset every stats/state/value to baseline
        Damage = baseDamage;
        timeController.DynamicType = DynamicType.Projectile;
        inPool = true;

        inactive = false;
        inGracePeriod = false;
        owner = null;

        stacksizeOnInactive = 0;

        // Re-Enter the pool
        GameManager.PoolManager.ReturnPoolableObject(gameObject);
    }
    public void OutOfBoundsCheck()
    {
        if (!timeController.IsRewinding && OutOfBoundsYAxis() && !inactive)
        {
            SetInactive();
        }
    }
    public bool OutOfBoundsYAxis()
    {
        return gameObject.transform.position.y >= 56 || gameObject.transform.position.y <= -10 ||
            gameObject.transform.position.x <= -60 || gameObject.transform.position.x >= 60;
    }
    public void SetInactive()
    {
        currentSpeed = 0f;
        this.gameObject.transform.position = new Vector3(200f, -240f, 0f);
        inactive = true;
        inGracePeriod = false;
        stacksizeOnInactive = timeController.CurrentIndex;
    }
    public void Launch(GameObject owner)
    {
        this.owner = owner;

        transform.up = owner.transform.up;

        timeController.SetRewinding(false);
        timeController.ResetIndexes();

        inactive = false;
        stacksizeOnInactive = 0;

        currentSpeed = baseSpeed;
        inPool = false;
        inGracePeriod = true;

        SetSprite(owner);

        RegisterProjectile(owner);
    }

    public void RegisterProjectile(GameObject owner)
    {
        if (owner.GetComponent<PlayerCore>() && !owner.GetComponent<PlayerCore>().isRed) // Blue / Inverted player shoots // is ACTING backwards
        {
            timeController.SetDynamicType(DynamicType.InvertedProjectile);
            timeController.RegisterObject();
        }
        else
        {
            timeController.RegisterObject();
        }
    }
    public void Activate()
    {
        currentSpeed = baseSpeed;
        inactive = false;
    }
    private void SetSprite(GameObject owner)
    {
        if (owner.CompareTag("Player") && owner.gameObject.GetComponent<PlayerCore>().isRed)
            model.GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
        if (owner.CompareTag("Player") && !owner.gameObject.GetComponent<PlayerCore>().isRed)
            model.GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
        if (owner.CompareTag("BasicEnemy"))
            model.GetComponentInChildren<SpriteRenderer>().sprite = sprites[2];
    }
    public void OnSpawn()
    {
    }
}