using Sirenix.OdinInspector;
using System;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [TabGroup("CoreObjects"), Required] public TimeKeeper TimeKeeper;
    [TabGroup("CoreObjects"), Required] public PoolManager PoolManager;
    [TabGroup("CoreObjects"), Required] public SpawnManager SpawnManager;
    [TabGroup("CoreObjects"), Required] public EffectManager EffectManager;
    [TabGroup("CoreObjects"), Required] public PlayerController PlayerController;
    [TabGroup("CoreObjects"), Required] public UIManager UIManager;
    [TabGroup("CoreObjects"), Required] public GameObject PlayerForward;

    private IDisposable disposable;

    // Properties
    public PlayerCore CurrentlyActivePlayer { get; set; }
    public PlayerCore InactivePlayer { get; set; }

    public MinMemento[] minMementos = new MinMemento[10];
    private void Start()
    {
        //Cursor.SetCursor(cursorTexture, new Vector3(0, 0, 0), CursorMode.Auto);
        //Cursor.visible = false;
        SpawnManager.SpawnEventStackEmptied += ScanForEnemies;
        SpawnManager.SpawnEventStackNonZero += StopScanningForEnemies;
    }
    private void ScanForEnemies()
    {
        disposable = Observable.Interval(TimeSpan.FromSeconds(.5f))
            .Where(_ => TimeKeeper.Enemies.Count == 0)
            .Where(_ => TimeKeeper.InvertedEnemies.Count == 0)
            .Subscribe(_ => StageComplete());
    }

    // Real Stuff
    private void StageComplete()
    {
        disposable.Dispose();
    }
    private void StopScanningForEnemies()
    {
        disposable.Dispose();
    }

    public void ResetScene()
    {
        EffectDB.ClearDB();
        DW.Clear();
        Debug.Log("Cleared static classes, now reset.");
        SceneManager.LoadScene(0);
    }
}