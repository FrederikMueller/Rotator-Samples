using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public PlayerCore pCore;
    [SerializeField] public PlayerOffense pOff;
    [SerializeField] public PlayerDefense pDef;

    public float Speed { get => speed; }
    [TabGroup("Values"), SerializeField] public float speed;

    public void HideOffscreen()
    {
        this.gameObject.transform.position = new Vector3(1000f, 1000f, 0f);
        speed = 0f;
    }
    public void Move()
    {
        gameObject.transform.position += pCore.playerController.Position;
    }
    public void OutOfBoundsReset()
    {
        // Hitbox solution would be more extendable
        //TODO aspect ratio independent solution, currently aspect ratio changes how big the playing field is.
        Vector3 pos = transform.position;
        if (transform.position.x <= -51.5f)
        {
            pos.x = -51.5f;
            transform.position = pos;
        }
        if (transform.position.x >= 51.5f)
        {
            pos.x = 51.5f;
            transform.position = pos;
        }
        if (transform.position.y <= -3.5f)
        {
            pos.y = -3.5f;
            transform.position = pos;
        }
        if (transform.position.y >= 53.5f)
        {
            pos.y = 53.5f;
            transform.position = pos;
        }
    }
}