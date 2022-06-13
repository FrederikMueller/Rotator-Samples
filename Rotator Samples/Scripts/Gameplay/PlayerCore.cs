using UnityEngine;
using Sirenix.OdinInspector;
using TimeControl;
using System;

public class PlayerCore : MonoBehaviour
{
    #region Props & Fields

    [TabGroup("Objects", "External"), SerializeField] public PlayerController playerController;
    [TabGroup("Objects", "External"), SerializeField] public GameManager gameManager;
    [TabGroup("Objects", "External"), SerializeField] public EffectManager effectManager;
    [TabGroup("Objects", "External"), SerializeField] public TCPlayer timeController;
    [TabGroup("Objects", "External"), SerializeField] public PlayerCore otherPlayer;

    [TabGroup("Objects", "Internal"), SerializeField] public PlayerDefense pDef;
    [TabGroup("Objects", "Internal"), SerializeField] public PlayerMovement pMove;
    [TabGroup("Objects", "Internal"), SerializeField] public PlayerOffense pOff;

    [TabGroup("Type", "Type"), SerializeField] public bool isRed;

    private bool callGlobalRewind;
    private bool callGlobalForward;
    private bool canCastGlobalInversion;

    public event Action ActiveFixedUpdate;

    #endregion Props & Fields

    #region UnityCallbacks

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        timeController = GetComponent<TCPlayer>();
        playerController = gameManager.PlayerController;
        effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();

        if (isRed)
            Activate();
        else
            gameObject.SetActive(false);
    }
    private void Start()
    {
        timeController.RewindCalled += OnRewind;

        timeController.ForwardCalled += Activate;
        timeController.ReachedBeginning += ReachedBeginning;
        //timeController.ReachedEnd +=
        timeController.TimeKeeper.GlobalForwardEvent += ForwardsInversionEvent;
        timeController.TimeKeeper.GlobalRewindEvent += RewindInversionEvent;
    }
    private void FixedUpdate()
    {
        if (this == playerController.ActiveShip)
        {
            HandleActive();
            timeController.ProgressForwardsInHistory();
        }
        else
        {
            timeController.ProgressBackwardsInHistory();
        }
    }
    private void LateUpdate()
    {
        if (callGlobalForward)
        {
            callGlobalForward = false;
            effectManager.PlayRealtimeEffect(EffectID.Inversion, transform.position);
            effectManager.PlayRealtimeEffect(EffectID.Inversion, otherPlayer.transform.position);

            timeController.TimeKeeper.StopTime(1);
            timeController.TimeKeeper.GlobalForward();
        }
        if (callGlobalRewind)
        {
            callGlobalRewind = false;
            effectManager.PlayRealtimeEffect(EffectID.Inversion, transform.position);

            timeController.TimeKeeper.StopTime(1);
            timeController.TimeKeeper.GlobalRewind();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.CurrentlyActivePlayer == this && other.gameObject.tag == "BasicEnemy")
        {
            // TODO Full reset of scene, temporary
            // Take damge and bounce away from
            gameManager.ResetScene();
        }
    }

    #endregion UnityCallbacks

    #region Rewind-related

    public void ActivateOnPos(Vector3 pos)
    {
        // Sign up for FORWARD event
        gameManager.CurrentlyActivePlayer = this;
        playerController.ActiveShip = this;
        gameManager.InactivePlayer = otherPlayer;

        transform.position = pos;
        canCastGlobalInversion = true;
    }
    public void Activate()
    {
        gameManager.CurrentlyActivePlayer = this;
        playerController.ActiveShip = this;
        gameManager.InactivePlayer = otherPlayer;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        otherPlayer.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;

        canCastGlobalInversion = true;
    }
    public void OnRewind()
    {
        pOff.shooting = false;
    }
    private void ReachedBeginning()
    {
        if (isRed)
        {
            callGlobalForward = true;
        }
        else
        {
            effectManager.PlayRealtimeEffect(EffectID.Despawn, transform.position);
            pMove.HideOffscreen();
        }
    }
    private void ForwardsInversionEvent()
    {
        if (!isRed)
        {
            otherPlayer.GetStateFromOtherPlayer();
            otherPlayer.Activate();
        }
    }
    private void RewindInversionEvent()
    {
        if (isRed)
        {
            otherPlayer.gameObject.SetActive(true);
            otherPlayer.GetStateFromOtherPlayer();
            otherPlayer.Activate();
        }
    }
    private void GetStateFromOtherPlayer()
    {
        pOff.laserAmmoCurrent = otherPlayer.pOff.laserAmmoCurrent;
        pMove.speed = otherPlayer.pMove.speed;
        pDef.Health = otherPlayer.pDef.Health;

        transform.position = otherPlayer.transform.position;
        transform.rotation = otherPlayer.transform.rotation;
    }
    public void GlobalInversion()
    {
        if (isRed && canCastGlobalInversion)
        {
            callGlobalRewind = true;
        }
        if (!isRed && canCastGlobalInversion) // able to invert (red: no blue ship up, blue: CD/resource/whatever)
        {
            callGlobalForward = true;
        }
    }

    #endregion Rewind-related

    #region Core Logic

    private void HandleActive()
    {
        pMove.Move();
        pMove.OutOfBoundsReset();
        // Core Event to kick off all other functionalities
        ActiveFixedUpdate?.Invoke();
    }

    #endregion Core Logic
}