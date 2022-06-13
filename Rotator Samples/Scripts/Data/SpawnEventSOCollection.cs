using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnEventCollection", menuName = "ScriptableObjects/SpawnEventCollection")]
public class SpawnEventSOCollection : ScriptableObject
{
    public List<SpawnEventSO> spawnEvents = new List<SpawnEventSO>();
    public string Name;
}