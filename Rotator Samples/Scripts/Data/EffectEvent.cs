using UnityEngine;

public class EffectEvent
{
    public int timestamp;
    public EffectObj effectObj;

    public EffectEvent(int timestamp, EffectObj obj)
    {
        this.timestamp = timestamp;
        effectObj = obj;
    }
}