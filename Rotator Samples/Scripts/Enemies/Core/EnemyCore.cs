using Sirenix.OdinInspector;
using System;
using TimeControl;
using UnityEngine;

public class EnemyCore : MonoBehaviour, ISpawnable
{
    // Management
    public TCEnemy timeController; // must be a strategy patern or something similar
    public TCEnemyNew tc2;
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public TimeKeeper TimeKeeper;
    [SerializeField, Required] public GameObject nose;

    // Enemy Core Events
    public event Action OnFixedUpdateForward;
    public event Action OnFixedUpdateBackward;
    public event Action OnSpawnEvent;
    public event Action OnDeathEvent;
    public event Action OnScamazEvent;

    // Enemy Data
    public EnemyData EnemyData { get; set; }
    [TabGroup("EnemyData"), SerializeField] private EnemyData enemyData;
    [HideInInspector] public MoveComponent eMov;
    [HideInInspector] public DefComponent eDef;
    [HideInInspector] public OffComponent eOff;

    // States & Values
    [SerializeField] private bool placedInEditor;
    protected bool isDead = true;
    public PoolableObject POType { get => enemyData.POType; }
    public SpawnEvent SpawnEvent { get; set; }
    public int SpawnID { get; set; }

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
        TimeKeeper = GameManager.TimeKeeper;
        eMov = GetComponent<MoveComponent>();
        eDef = GetComponent<DefComponent>();
        eOff = GetComponent<OffComponent>();

        //SetupEnemyData();
        TCEventSubs();
    }
    public void TCEventSubs()
    {
        timeController = GetComponent<TCEnemy>();
        SetDead(true);

        timeController.RewindCalled += OnRewind;
        timeController.ForwardCalled += OnForward;
        timeController.ReachedBeginning += RemoveOrForward; // => on -1 or wait one frame
        //timeController.ReachedEnd +=
        //timeController.TimeKeeper.GlobalForwardEvent +=
        //timeController.TimeKeeper.GlobalRewindEvent +=

        if (placedInEditor)
            timeController.RegisterObject();
    }
    protected void SetupEnemyData()
    {
        EnemyData.Defense?.InjectLogic(eDef);
        EnemyData.Movement?.InjectLogic(eMov);
        EnemyData.Offense?.InjectLogic(eOff);

        eDef.Health = EnemyData.maxHealth;

        eDef?.Init();
        eMov?.Init();
        eOff?.Init();
    }
    public void InjectEnemyData(EnemyData enemyData)
    {
        this.enemyData.POType = enemyData.POType;
        InjectEnemyData(enemyData.Defense, enemyData.Movement, enemyData.Offense, enemyData.maxHealth);
    }

    public void InjectEnemyData(InjectableDefSO def, InjectableMoveSO move, InjectableOffSO off, int maxHealth)
    {
        def.InjectLogic(eDef);
        move.InjectLogic(eMov);
        off.InjectLogic(eOff);

        eDef.Health = maxHealth;
        Debug.Log($"{eDef.Health}");

        eDef.Init();
        eMov.Init();
        eOff.Init();
    }
    public void RemoveOrForward()
    {
        if (GameManager.PlayerForward.GetComponent<TCPlayer>().CurrentIndex == 0)
            return; // Prevents removal of enemy if its in sync with Red. Blue==0 is irrelevant because we want inverted enemies to despawn there.

        gameObject.transform.position = new Vector3(1000f, 950f, 0f);
        isDead = true;
        OnDeathEvent = null;
        eMov.Speed = 0f;
        TimeKeeper.DeRegisterObject(GetComponent<IDynamic>());
        GameManager.PoolManager.ReturnPoolableObject(gameObject);
    }
    public void OnSpawn()
    {
        SetupEnemyData();
        SetDead(false);

        ResetAndRegister();
        OnSpawnEvent?.Invoke();
    }
    public void ResetAndRegister()
    {
        timeController.ResetState();
        timeController.RegisterObject();
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

    public void OnDeath()
    {
        gameObject.transform.position = new Vector3(1000f, 950f, 0f);
        isDead = true;

        if (SpawnEvent != null)
            SpawnEvent.spawnInfo[SpawnID].isDead = true;

        OnDeathEvent?.Invoke();
        OnDeathEvent = null;
        eMov.Speed = 0f;

        TimeKeeper.DeRegisterObject(GetComponent<IDynamic>());

        GameManager.PoolManager.ReturnPoolableObject(gameObject);
    }
}