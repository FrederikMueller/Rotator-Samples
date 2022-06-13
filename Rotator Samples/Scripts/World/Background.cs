using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public bool isReversed;
    private void Start()
    {
        FindObjectOfType<TimeKeeper>().GlobalRewindEvent += () => isReversed = true;
        FindObjectOfType<TimeKeeper>().GlobalForwardEvent += () => isReversed = false;
    }

    private void FixedUpdate()
    {
        if (isReversed)
            transform.position = new Vector3(transform.position.x, transform.position.y + .02f, 0);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y - .02f, 0);
    }
}