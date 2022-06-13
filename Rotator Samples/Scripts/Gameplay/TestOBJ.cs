using UnityEngine;
using TimeControl;
using Sirenix.OdinInspector;
using System;

public class TestOBJ : MonoBehaviour
{
    public int IntValue { get; set; }
    public TCTest timeController;

    // States & Values

    // Unity Callbacks
    private void Awake()
    {
        timeController = GetComponent<TCTest>();
        IntValue = 7;
    }
    private void Start()
    {
        timeController.RewindCalled += OnRewind;
        timeController.ForwardCalled += OnForward;
        //timeController.ReachedBeginning +=
        //timeController.ReachedEnd +=
        //timeController.TimeKeeper.GlobalForwardEvent +=
        //timeController.TimeKeeper.GlobalRewindEvent +=
    }
    private void FixedUpdate()
    {
        if (!timeController.IsRewinding)
        {
            // Do something
            transform.position += new Vector3(-0.07f, 0, 0);
            IntValue += UnityEngine.Random.Range(0, 5);

            timeController.ProgressForwardsInHistory();
        }
        else
        {
            timeController.ProgressBackwardsInHistory();
        }
    }

    // Helper Methods
    public void OnRewind()
    {
        // Triggered when rewind is called
        Debug.Log("CUBE REWIND");
    }
    public void OnForward()
    {
        Debug.Log("CUBE Forward!");
        // Triggered when forward is called
    }
}