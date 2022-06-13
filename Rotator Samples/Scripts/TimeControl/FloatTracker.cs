using System;
using TimeControl;
using UnityEngine;

public class FloatTracker
{
    public FloatTracker(byte typeidx, TCEnemyNew tc)
    {
        typeIdx = typeidx;
        timeController = tc;
    }
    public byte typeIdx;
    public TCEnemyNew timeController;

    public event Action<float> OutputEvent;
    public void RecordInput(float value)
    {
        if (!timeController.IsRewinding)
            timeController.PastFloats.Push(new FloatMementoNew(timeController.CurrentIndex, value, typeIdx));
        else
            Debug.Log($"Tried to push {value} to float-stack {typeIdx} at frame {timeController.CurrentIndex}, but obj was already rewinding!");
    }
    public void SendOutput(float value) => OutputEvent.Invoke(value);
    public void ClearEvent() => OutputEvent = null;
}