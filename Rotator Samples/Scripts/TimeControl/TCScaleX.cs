using TimeControl;
using UnityEngine;
using System;

public class TCScaleX : TCBase
{
    #region Props & Fields

    private FloatMemento[] history;
    private FloatMemento currentSnapshot;

    #endregion Props & Fields

    protected override void Awake()
    {
        base.Awake();
        /* Events
        ForwardCalled +=
        RewindCalled +=
        */
    }
    public override void RegisterObject()
    {
        TimeKeeper.RegisterObject(this);
    }

    #region Methods

    public override void WriteHistory()
    {
        history[CurrentIndex].scale = transform.localScale.x;
    }
    public override void ReadHistory()
    {
        currentSnapshot = history[CurrentIndex];
        var scale = new Vector3(currentSnapshot.scale, currentSnapshot.scale, currentSnapshot.scale);
        gameObject.transform.localScale = scale;
    }
    protected override void CreateHistory()
    {
        history = new FloatMemento[3000];
        for (int i = 0; i < 3000; i++)
        {
            history[i] = new FloatMemento(0);
        }
    }
    protected override void CheckForArrayResize(int amtToAdd)
    {
        if (CurrentIndex > history.Length - 1) // to avoid out of bounds because nowIndex gets increased afterwards.
        {
            Array.Resize(ref history, history.Length + amtToAdd);
            for (int i = 0; i < amtToAdd; i++)
            {
                history[CurrentIndex + i] = new FloatMemento(0);
            }
        }
    }

    #endregion Methods
}