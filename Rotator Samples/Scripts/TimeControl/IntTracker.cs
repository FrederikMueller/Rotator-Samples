using System;
using TimeControl;
using UnityEngine;

public class IntTracker
{
    public IntTracker(byte typeidx, TCEnemyNew tc)
    {
        typeIdx = typeidx;
        timeController = tc;
    }
    public byte typeIdx;
    public TCEnemyNew timeController;

    public event Action<int> OutputEvent;
    public void RecordInput(int value)
    {
        if (!timeController.IsRewinding)
            timeController.PastInts.Push(new IntMementoNew(timeController.CurrentIndex, value, typeIdx));
        else
            timeController.FutureInts.Push(new IntMementoNew(timeController.CurrentIndex, value, typeIdx));

        //Debug.Log($"Tried to push {value} to int-stack {typeIdx} at frame {timeController.CurrentIndex}, but obj was already rewinding!");
    }
    public void SendOutput(int value) => OutputEvent.Invoke(value);
    public void ClearEvent() => OutputEvent = null;
}