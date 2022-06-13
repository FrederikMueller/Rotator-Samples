using DG.Tweening;
using System;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [HideInInspector] public EnemyCoreNew eCore;
    [HideInInspector] public DefComponent eDef;
    [HideInInspector] public OffComponent eOff;

    public MoveStrategyBase Movement { get; set; }

    // Events
    public event Action OnResetMovementSpeed;
    public event Action OnZeroMovementSpeed;

    public event Action<float> XPosChanged;
    public event Action<float> YPosChanged;
    public event Action<float> ZRotChanged;

    // State
    public bool CanRotate { get; set; }
    public bool CanChase { get; set; }
    public float Speed { get; set; }

    public bool CanRotateBaseline;
    private Vector3 currentPos;
    private Vector3 currentRot;

    private void Awake()
    {
        eCore = GetComponent<EnemyCoreNew>();
        eDef = GetComponent<DefComponent>();
        eOff = GetComponent<OffComponent>();
    }
    public void Init()
    {
        currentPos = new Vector3(0, 0, 0);
        currentRot = new Vector3(0, 0, 0);

        var tx = eCore.timeController.AddFloat(ref XPosChanged);
        tx.OutputEvent += XPosFromHistory;

        var ty = eCore.timeController.AddFloat(ref YPosChanged);
        ty.OutputEvent += YPosFromHistory;

        var tz = eCore.timeController.AddFloat(ref ZRotChanged);
        tz.OutputEvent += ZRotFromHistory;

        Movement.Init();

        eCore.OnSpawnEvent += FaceInDirection;
        eCore.OnFixedUpdateForward += Movement.Move;
        eCore.OnFixedUpdateForward += HandleRotation;

        // Every frame we push out an update OR go back to on demand and be clear how to call from outside! If movement is happening every frame
        // then the automatic update is as good as the event only. If you have 2 movement changes in a frame (mov / off) then this is better.
        // Less is worse ofc
        //eCore.OnFixedUpdateForward += XPosUpdate;
        //eCore.OnFixedUpdateForward += YPosUpdate;
        //eCore.OnFixedUpdateForward += YPosUpdate;
    }
    public void FullReset()
    {
        Movement.ClearEvents();
        Movement = null;

        CanRotate = false;
        CanChase = false;
        Speed = 0f;
        CanRotateBaseline = false;

        ClearEvents();
    }

    public void ClearEvents()
    {
        OnResetMovementSpeed = null;
        OnZeroMovementSpeed = null;
        XPosChanged = null;
        YPosChanged = null;
        ZRotChanged = null;
    }
    // TC NEW
    public void XPosFromHistory(float xpos)
    {
        currentPos.x = xpos;
        transform.position = currentPos;
    }
    public void YPosFromHistory(float ypos)
    {
        currentPos.y = ypos;
        transform.position = currentPos;
    }
    public void ZRotFromHistory(float zrot)
    {
        currentRot.z = zrot;
        transform.localEulerAngles = currentRot;
    }

    public void XPosUpdate() => XPosChanged(transform.position.x);
    public void YPosUpdate() => YPosChanged(transform.position.y);
    public void ZRotUpdate() => ZRotChanged(transform.rotation.eulerAngles.z);

    private void HandleRotation()
    {
        if (CanRotate)
        {
            var dir = eOff.target.transform.position - this.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            ZRotUpdate();
        }
    }

    private void FaceInDirection()
    {
        var dir = Vector3.down;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    public void ResetMovementSpeed()
    {
        OnResetMovementSpeed?.Invoke();
    }
    public void ZeroMovementSpeed()
    {
        OnZeroMovementSpeed?.Invoke();
    }
    public void DisableRotation() => CanRotate = false;
    public void ResetRotation() => CanRotate = CanRotateBaseline;
}