using Sirenix.OdinInspector;
using System;
using TimeControl;
using UnityEngine;

public class EnemyCoreNew : MonoBehaviour, ISpawnable
{
    // Fundamental
    [HideInInspector] public TCEnemyNew timeController;
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public TimeKeeper TimeKeeper;
    [HideInInspector] public MoveComponent eMov;
    [HideInInspector] public DefComponent eDef;
    [HideInInspector] public OffComponent eOff;
    [SerializeField, Required] public GameObject nose;
    [SerializeField, Required] private SpriteRenderer model;
    [SerializeField, Required] private PoolableObject basePOType;

    // Events
    public event Action OnFixedUpdateForward;
    public event Action OnFixedUpdateBackward;
    public event Action OnSpawnEvent;
    public event Action OnDeathEvent;

    // State
    public EnemyData EnemyData { get; set; }
    protected bool isDead = true;
    public PoolableObject POType { get => basePOType; }
    public SpawnEvent SpawnEvent { get; set; }
    public int SpawnID { get; set; }

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
        TimeKeeper = GameManager.TimeKeeper;
        timeController = GetComponent<TCEnemyNew>();
        eMov = GetComponent<MoveComponent>();
        eDef = GetComponent<DefComponent>();
        eOff = GetComponent<OffComponent>();
        SetDead(true);
    }
    public void OnSpawn()
    {
        TCEventSubs();
        LoadEnemyData();
        SetDead(false);

        timeController.ResetIndexes();
        timeController.RegisterObject();

        OnSpawnEvent?.Invoke();
    }
    public void TCEventSubs()
    {
        timeController.RewindCalled += OnRewind;
        timeController.ForwardCalled += OnForward;
        timeController.ReachedBeginning += RemoveOrForward; // => on -1 or wait one frame
        //timeController.ReachedEnd +=
        //timeController.TimeKeeper.GlobalForwardEvent +=
        //timeController.TimeKeeper.GlobalRewindEvent +=
    }
    protected void LoadEnemyData()
    {
        // EnemyData must be injected via SpawnManager already otherwise null errors
        // Include model, scale, and other cosmetic stuff
        gameObject.transform.localScale = EnemyData.Scale;

        model.sprite = EnemyData.Model;
        int w = model.sprite.texture.width;
        int h = model.sprite.texture.height;

        if (EnemyData.ScaleModelToHitbox)
            ScaleModelToHitbox(w, h);
        else
            ScaleHitboxToModel(w, h);

        EnemyData.Defense.InjectLogic(eDef);
        EnemyData.Movement.InjectLogic(eMov);
        EnemyData.Offense.InjectLogic(eOff);

        eDef.Init();
        eMov.Init();
        eOff.Init();
    }

    private void ScaleModelToHitbox(int w, int h)
    {
        var modelScale = model.transform.localScale;
        modelScale.x *= 1 / (w / 100f);
        modelScale.y *= 1 / (h / 100f);
        model.transform.localScale = modelScale;
    }

    private void ScaleHitboxToModel(int w, int h)
    {
        float x = w / 100f;
        float y = h / 100f;
        gameObject.GetComponent<BoxCollider>().size = new Vector3(x, y, .4f);
    }

    public void SetDead(bool value) => isDead = value;

    private void FixedUpdate()
    {
        if (isDead)
            return;

        if (!timeController.IsRewinding)
        {
            OnFixedUpdateForward?.Invoke();

            timeController.ProgressForwardsInHistory();
        }
        else
        {
            timeController.ProgressBackwardsInHistory();
        }
    }
    public void OnRewind()
    {
        eMov.ResetMovementSpeed();
    }
    public void OnForward()
    {
        if (GetComponent<Boss>())
            GetComponent<Boss>().RestartBehavior();
    }

    public void RemoveOrForward()
    {
        if (GameManager.PlayerForward.GetComponent<TCPlayer>().CurrentIndex == 0)
            return; // Prevents removal of enemy if its in sync with Red. Blue==0 is irrelevant because we want inverted enemies to despawn there.
        FullReset();
    }
    public void OnDeath()
    {
        if (SpawnEvent != null)
            SpawnEvent.spawnInfo[SpawnID].isDead = true;
        FullReset();
    }
    public void FullReset()
    {
        gameObject.transform.position = new Vector3(1000f, 950f, 0f);

        SetDead(true);
        OnDeathEvent?.Invoke();

        eDef.FullReset();
        eMov.FullReset();
        eOff.FullReset();
        ClearEvents();

        EnemyData = null;
        SpawnEvent = null;
        SpawnID = 0;

        gameObject.transform.localScale = new Vector3(1, 1, 1);

        timeController.FullReset();
        GameManager.PoolManager.ReturnPoolableObject(gameObject);
    }
    public void ClearEvents()
    {
        OnFixedUpdateForward = null;
        OnFixedUpdateBackward = null;
        OnSpawnEvent = null;
        OnDeathEvent = null;
    }
}