using System;
using TimeControl;
using UnityEngine;

public class TCTest : TCBase, IHasIntValue
{
    #region Props & Fields

    private IHasIntValue trackedObj;
    private int IntValueLastFrame;

    private VectorIntMemento[] history;
    private VectorIntMemento currentSnapshot;

    public Vector3 Position { get; set; }
    public int IntValue { get => trackedObj.IntValue; set => trackedObj.IntValue = value; }

    #endregion Props & Fields

    private void Start()
    {
        trackedObj = this.gameObject.GetComponent<IHasIntValue>();
        IntValueLastFrame = IntValue;
        ReachedBeginning += () => IntValueLastFrame = IntValue;
        RegisterObject();
    }
    public override void RegisterObject()
    {
        TimeKeeper.RegisterObject(this);
    }

    #region Methods

    public override void WriteHistory()
    {
        history[CurrentIndex].position = transform.position;
        history[CurrentIndex].intValue = IntValue - IntValueLastFrame;

        IntValueLastFrame = IntValue;
    }
    public override void ReadHistory()
    {
        currentSnapshot = history[CurrentIndex];
        gameObject.transform.position = currentSnapshot.position;
        IntValue -= currentSnapshot.intValue;
    }
    protected override void CreateHistory()
    {
        history = new VectorIntMemento[3000];
        for (int i = 0; i < 3000; i++)
        {
            history[i] = new VectorIntMemento(Vector3.zero, 0);
        }
    }
    protected override void CheckForArrayResize(int amtToAdd)
    {
        if (CurrentIndex > history.Length - 1) // to avoid out of bounds because nowIndex gets increased afterwards.
        {
            Array.Resize(ref history, history.Length + amtToAdd);
            for (int i = 0; i < amtToAdd; i++)
            {
                history[CurrentIndex + i] = new VectorIntMemento(Vector3.zero, 0);
            }
        }
    }

    #endregion Methods
}