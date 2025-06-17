using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
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