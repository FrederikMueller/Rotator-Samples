using System;
using TimeControl;
using UnityEngine;

public class ByteTracker
{
    public ByteTracker(byte typeidx, TCEnemyNew tc)
    {
        typeIdx = typeidx;
        timeController = tc;
    }
    public byte typeIdx;
    public TCEnemyNew timeController;

    public event Action<byte> OutputEvent;
    public void RecordInput(byte value)
    {
        if (!timeController.IsRewinding)
            timeController.PastBytes.Push(new ByteMementoNew(timeController.CurrentIndex, value, typeIdx));
        else
            Debug.Log($"Tried to push {value} to byte-stack {typeIdx} at frame {timeController.CurrentIndex}, but obj was already rewinding!");
    }
    public void SendOutput(byte value) => OutputEvent.Invoke(value);
    public void ClearEvent() => OutputEvent = null;
}