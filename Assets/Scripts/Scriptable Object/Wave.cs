using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Info")]
[System.Serializable]
public class Wave : ScriptableObject
{
    public List<Horde> hordes;
    public float waveCountdown;
}

[System.Serializable]
public class Horde
{
    public BaseEnemy enemyPrefab;
    public int count;
    public float spawnInterval;
    public float hordeCountdown;
    public int pathNumber;
}