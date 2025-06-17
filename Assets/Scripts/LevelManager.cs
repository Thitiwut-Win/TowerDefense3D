using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] public List<EnemyWaypoints> pathWaypoints;
    private int lives;
    void Start()
    {
        StartCoroutine(StartWave());
    }
    private IEnumerator StartWave()
    {
        foreach (Wave wave in waves)
        {
            yield return new WaitForSeconds(wave.waveCountdown);
            foreach (Horde horde in wave.hordes)
            {
                yield return new WaitForSeconds(horde.hordeCountdown);
                yield return StartCoroutine(StartHorde(horde));
            }
        }
    }
    private IEnumerator StartHorde(Horde horde)
    {
        for (int i = 0; i < horde.count; i++)
        {
            NavMeshHit closestHit;
            Vector3 spawnPosition = pathWaypoints[horde.pathNumber].waypoints[0].position;
            if (NavMesh.SamplePosition(spawnPosition, out closestHit, 3, 1)) spawnPosition = closestHit.position;
            BaseEnemy enemy = PoolManager.Instance.Spawn<BaseEnemy>(horde.enemyPrefab.name, spawnPosition, Quaternion.identity);
            enemy.SetWaypoint(pathWaypoints[horde.pathNumber]);
            enemy.StartRunningWaypoints();
            yield return new WaitForSeconds(horde.spawnInterval);
        }
    }
    public void DecreaseLives(int live)
    {
        lives -= live;
        if (lives <= 0) GameOver();
    }
    private void GameOver()
    {

    }
}