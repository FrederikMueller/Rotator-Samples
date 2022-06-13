using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class EffectObj : MonoBehaviour, IPoolable
{
    public PoolableObject POType => PoolableObject.EffectObj;
    [SerializeField, Required] private Animator animator;
    [SerializeField, Required] private AudioSource AudioSource;
    private TimeKeeper timeKeeper;
    private EffectManager effectManager;

    // Effect Values. If you ever have a pooled object that you want to reset fully, then just put all relevant values in a block like this and copy them over
    // into the reset method. Alt mark and strg delete twice = gg
    private Effect effect;
    private Vector3 startPos;
    private Vector3 lastPos;
    private int effectFinishedTimestamp;
    private bool isInverted;
    private bool isMixed;
    private bool isInWorld;
    private bool inversionEventTriggered;

    // blue reversal glitches to vector.zero somehow
    // mixed explos block the stack

    private void Awake()
    {
        startPos = transform.position;
        timeKeeper = FindObjectOfType<TimeKeeper>();
        effectManager = FindObjectOfType<EffectManager>();

        timeKeeper.GlobalRewindEvent += Pause;
        timeKeeper.GlobalForwardEvent += Pause;
    }
    public void PlayRealtimeEffect(EffectID id, Vector3 pos, bool inverted = false)
    {
        effect = EffectDB.GetEffect(id);

        gameObject.transform.position = pos;

        AudioSource.clip = effectManager.audioClipDB[effect.SoundID];
        AudioSource.PlayOneShot(AudioSource.clip);

        animator.Play(effect.AnimationName, 0);

        WaitRealtimeAndDo(effect.AnimDuration, FullReset);
    }
    public void PlayEffect(EffectID id, Vector3 pos, bool inverted = false, bool mixed = false)
    {
        isInverted = inverted;
        isMixed = mixed;

        effect = EffectDB.GetEffect(id);

        gameObject.transform.position = pos;

        AudioSource.clip = effectManager.audioClipDB[effect.SoundID];
        AudioSource.PlayOneShot(AudioSource.clip);

        animator.SetFloat("timeDir", 1f);

        if (isMixed)
            animator.Play($"{effect.AnimationName}M", 0);
        if (isInverted && !isMixed)
            animator.Play($"{effect.AnimationName}Inverted", 0);
        if (!isInverted && !isMixed)
            animator.Play(effect.AnimationName, 0);

        isInWorld = true;
        WaitAndDo(effect.AnimDuration, OnAnimationEnd);
    }
    public void PlayReversedEffect()
    {
        AudioSource.pitch = 1;
        AudioSource.clip = effectManager.audioClipDB[$"{effect.SoundID}REV"];
        AudioSource.PlayOneShot(AudioSource.clip);

        WaitAndDo(AudioSource.clip.length - effect.AnimDuration, PlayReversedAnimation);
    }
    private void PlayReversedAnimation()
    {
        gameObject.transform.position = lastPos;
        animator.enabled = true;
        animator.speed = 1;
        // consider using timeDir to alter animation without copyiong it
        animator.SetFloat("timeDir", -1f);

        if (isMixed)
            animator.Play($"{effect.AnimationName}M");
        if (isInverted && !isMixed)
            animator.Play($"{effect.AnimationName}Inverted");
        if (isInverted && !isMixed)
            animator.Play($"{effect.AnimationName}");

        WaitAndDo(effect.AnimDuration, FullReset);
    }
    private void OnAnimationEnd()
    {
        lastPos = gameObject.transform.position;
        gameObject.transform.position = startPos;
        animator.enabled = false;

        WaitAndDo(AudioSource.clip.length - effect.AnimDuration, EffectFinished);
    }
    private void EffectFinished()
    {
        if (inversionEventTriggered)
        {
            FullReset();
            return;
        }

        effectFinishedTimestamp = timeKeeper.CurrentWorldFrame;

        if (isInverted)
            effectManager.AddBackwardsEffect(effectFinishedTimestamp, this);
        else
            effectManager.AddForwardsEffect(effectFinishedTimestamp, this);
    }
    public void Pause()
    {
        if (!isInWorld)
            return;

        animator.speed = 0;
        AudioSource.Pause();
        inversionEventTriggered = true;

        WaitRealtimeAndDo(1, Reverse);
    }
    public void Reverse()
    {
        animator.speed = 1;
        AudioSource.pitch = -1;
        AudioSource.UnPause();
    }
    public void FullReset()
    {
        effect = null;
        isInverted = false;
        isMixed = false;
        effectFinishedTimestamp = 0;
        lastPos = Vector3.zero;
        startPos = new Vector3(1000, 1000, 0);
        isInWorld = false;
        inversionEventTriggered = false;

        animator.enabled = true;
        animator.speed = 1;
        animator.SetFloat("timeDir", 1);

        AudioSource.pitch = 1;
        AudioSource.UnPause();

        effectManager.poolManager.ReturnPoolableObject(gameObject);
        DW.Log("EPool", effectManager.poolManager.Pools[PoolableObject.EffectObj].Pool.Count);
    }
    private void WaitAndDo(float duration, Action action)
    {
        StartCoroutine(WaitAndDoCoroutine(action, duration));
    }
    private void WaitRealtimeAndDo(float duration, Action action)
    {
        StartCoroutine(WaitAndDoRealtime(action, duration));
    }
    private IEnumerator WaitAndDoCoroutine(Action action, float duration)
    {
        yield return new WaitForSeconds(duration);
        action.Invoke();
    }
    private IEnumerator WaitAndDoRealtime(Action action, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        action.Invoke();
    }
}