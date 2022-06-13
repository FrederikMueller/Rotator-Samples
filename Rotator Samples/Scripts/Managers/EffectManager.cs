using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum EffectID { Null, Inversion, Despawn, ShockCombo }
public class EffectManager : MonoBehaviour
{
    [SerializeField, Required] public PoolManager poolManager;
    [SerializeField, Required] private TimeKeeper timeKeeper;
    [SerializeField, Required] public Dictionary<string, AudioClip> audioClipDB = new Dictionary<string, AudioClip>();
    // Addressables BTW

    private Stack<EffectObj> effectObjs = new Stack<EffectObj>();

    private Stack<EffectEvent> forwardsEffects = new Stack<EffectEvent>();
    private Stack<EffectEvent> backwardsEffects = new Stack<EffectEvent>();

    private void Awake()
    {
        EffectDB.InitDBTesting();

        foreach (var clip in Resources.LoadAll<AudioClip>("Sounds"))
        {
            audioClipDB.Add(clip.name, clip);
        }
        DW.Log("Audio DB", audioClipDB.Count);
    }
    private void Start()
    {
        AddToObjStack(10);
    }
    private void FixedUpdate()
    {
        // var effectEvent = effectEvents.Pop();
        // Forwards Effects. Do I need to store these or will all effects come from organic calls in the world?
        // Mostly unnecessary atm.
        // currently I think all regular, forwards events will just happen through objects or specific large scale events, so there is no need to
        // have a stack for these events
        // also it wouldnt be clean if the effect is putting itself on the stack again. It has to be called via some ingame interaction

        if (timeKeeper.IsWorldRewinding && forwardsEffects.Count > 0 && forwardsEffects.Peek().timestamp == timeKeeper.CurrentWorldFrame)
        {
            forwardsEffects.Pop().effectObj.PlayReversedEffect();
            DW.Log("Red Stack", forwardsEffects.Count);
        }
        if (!timeKeeper.IsWorldRewinding && backwardsEffects.Count > 0 && backwardsEffects.Peek().timestamp == timeKeeper.CurrentWorldFrame)
        {
            backwardsEffects.Pop().effectObj.PlayReversedEffect();
            DW.Log("Blue Stack", backwardsEffects.Count);
        }
    }

    public void PlayEffect(EffectID id, Vector3 pos, bool inverted = false, bool mixed = false)
    {
        effectObjs.Pop().PlayEffect(id, pos, inverted, mixed);
        if (effectObjs.Count < 5)
            AddToObjStack(5);
        DW.Log("EPool", poolManager.Pools[PoolableObject.EffectObj].Pool.Count);
    }
    public void PlayRealtimeEffect(EffectID id, Vector3 pos, bool inverted = false)
    {
        effectObjs.Pop().PlayRealtimeEffect(id, pos, inverted);
        if (effectObjs.Count < 5)
            AddToObjStack(5);
        DW.Log("EPool", poolManager.Pools[PoolableObject.EffectObj].Pool.Count);
    }

    public void AddForwardsEffect(int timestamp, EffectObj effectObj)
    {
        forwardsEffects.Push(new EffectEvent(timestamp, effectObj));
        Debug.Log($"Red Stack has {forwardsEffects.Count} effects on it.");
        DW.Log("Red Stack", forwardsEffects.Count);
    }
    public void AddBackwardsEffect(int timestamp, EffectObj effectObj)
    {
        backwardsEffects.Push(new EffectEvent(timestamp, effectObj));
        DW.Log("Blue Stack", backwardsEffects.Count);
    }
    public void AddToObjStack(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var e = poolManager.FetchPooledObject(PoolableObject.EffectObj);
            effectObjs.Push(e.GetComponent<EffectObj>());
        }
    }
}