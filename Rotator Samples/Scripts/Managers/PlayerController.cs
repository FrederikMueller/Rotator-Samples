using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [TabGroup("Objects", "Objects"), SerializeField] private PlayerCore redShip;

    public PlayerCore ActiveShip { get; set; }
    private Controls controls;
    private Vector3 position;
    private float angle;
    public Vector3 Position { get => position; }

    #region Unitycallbacks

    private void Awake()
    {
        SetupControls();
        ActiveShip = redShip;
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
    //ASDF
    private void Update()
    {
        var moveDir = controls.Player.WASD.ReadValue<Vector2>();
        position = new Vector3(moveDir.x * ActiveShip.pMove.Speed, moveDir.y * ActiveShip.pMove.Speed, 0f);

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(ActiveShip.transform.position);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    private void FixedUpdate()
    {
        RotateTowardsMouse();
    }

    #endregion Unitycallbacks

    private void SetupControls()
    {
        controls = new Controls();
        controls.Enable();
        controls.Player.Fire.performed += _ => ActiveShip.pOff.shooting = true;
        controls.Player.Fire.canceled += _ => ActiveShip.pOff.shooting = false;
        controls.Player.Fire2.started += _ => ActiveShip.pOff.ShootOrb();
        controls.Player.TriggerInversion.performed += _ => ActiveShip.GlobalInversion();
    }
    public void RotateTowardsMouse()
    {
        ActiveShip.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}