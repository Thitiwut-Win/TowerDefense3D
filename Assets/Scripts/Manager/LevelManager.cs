using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private TowerHologram selectedTower;
    [SerializeField] private List<Wave> waves;
    [SerializeField] public List<EnemyWaypoints> pathWaypoints;
    [SerializeField] private Transform shuttleSpawnPoint;
    [SerializeField] private Transform shuttleDespawnPoint;
    private int waveIndex = 0;
    public bool isLoop;
    private bool isAuto = false;
    private bool isSpawning = false;
    private int lives = 20;
    private int money = 200;
    private List<BaseEnemy> enemies = new List<BaseEnemy>();
    private List<BaseEnemy> enemyPreviews = new List<BaseEnemy>();
    private int numbersOfHorde = 1;
    private int waitForHorde = 0;
    private int shuttleInPostion = 0;
    private List<SpaceShuttle> spaceShuttles = new List<SpaceShuttle>();
    [SerializeField] private AudioClip audioClip;
    private Coroutine coroutine;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    private IEnumerator StartWave()
    {
        Wave wave = waves[waveIndex];
        yield return new WaitForSeconds(wave.waveCountdown);
        for (int i = 0; i < wave.hordes.Count;)
        {
            numbersOfHorde = 1;
            waitForHorde = 0;
            StartCoroutine(StartHorde(wave.hordes[i++]));
            while (i < wave.hordes.Count && wave.hordes[i].eHordeStatus == EHordeStatus.WITH_LAST_HORDE)
            {
                numbersOfHorde++;
                StartCoroutine(StartHorde(wave.hordes[i++]));
            }
            yield return new WaitUntil(() => numbersOfHorde == waitForHorde);
            // print(numbersOfHorde + " " + waitForHorde);
        }
        waveIndex++;
        if (waveIndex >= waves.Count && isLoop) waveIndex = 0;
        DespawnShuttle();
    }
    private IEnumerator StartHorde(Horde horde)
    {
        yield return new WaitForSeconds(horde.hordeCountdown);
        for (int i = 0; i < horde.count; i++)
        {
            NavMeshHit closestHit;
            Vector3 spawnPosition = pathWaypoints[horde.pathNumber].waypoints[0].position;
            if (NavMesh.SamplePosition(spawnPosition, out closestHit, 3, 1)) spawnPosition = closestHit.position;
            BaseEnemy enemy = PoolManager.Instance.Spawn<BaseEnemy>(horde.enemyPrefab.name, spawnPosition, Quaternion.identity);
            enemy.SetWaypoint(pathWaypoints[horde.pathNumber]);
            enemy.StartRunningWaypoints();
            enemy.onDead += OnEnemiesDied;
            enemies.Add(enemy);
            yield return new WaitForSeconds(horde.spawnInterval);
        }
        waitForHorde++;
    }
    private void OnReadyToSpawn(SpaceShuttle spaceShuttle)
    {
        spaceShuttle.onGetInPosition -= OnReadyToSpawn;
        shuttleInPostion++;
        if (shuttleInPostion >= spaceShuttles.Count)
        {
            StartCoroutine(StartWave());
        }
    }
    private void SpawnShuttle()
    {
        StopPreview();
        isSpawning = true;
        shuttleInPostion = 0;
        WaveCaller.Instance.DisableCall();
        GameUI.Instance.SetWave(waveIndex + 1);
        Wave wave = waves[waveIndex];
        List<bool> reserved = new List<bool>()
        {
                false, false, false, false, false
        };
        for (int i = 0; i < wave.hordes.Count; i++)
        {
            if (reserved[wave.hordes[i].pathNumber]) continue;
            // print(i);
            SpaceShuttle spaceShuttle = PoolManager.Instance.Spawn<SpaceShuttle>("SpaceShuttle_01", shuttleSpawnPoint.position, Quaternion.identity);
            spaceShuttle.MoveToPosition(pathWaypoints[wave.hordes[i].pathNumber].waypoints[0].position, 8);
            spaceShuttles.Add(spaceShuttle);
            spaceShuttle.onGetInPosition += OnReadyToSpawn;
            reserved[wave.hordes[i].pathNumber] = true;

            AudioSource audioSource = shuttleSpawnPoint.GetComponentInParent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.volume = GameManager.Instance.GetVolume() / 200f;
            audioSource.Play();
        }
    }
    private void DespawnShuttle()
    {
        for (int i = 0; i < spaceShuttles.Count; i++)
        {
            spaceShuttles[i].MoveToPosition(shuttleDespawnPoint.position, 16);
        }
        spaceShuttles.Clear();
        isSpawning = false;
        WaveCaller.Instance.EnableCall();
        StartPreview();
        if (isAuto) SpawnShuttle();
    }
    private IEnumerator Preview()
    {
        Wave wave = waves[waveIndex];
        List<bool> reserved = new List<bool>()
        {
                false, false, false, false, false
        };
        for (int i = 0; i < wave.hordes.Count; i++)
        {
            if (reserved[wave.hordes[i].pathNumber]) continue;
            reserved[wave.hordes[i].pathNumber] = true;
        }
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                if (!reserved[i]) continue;
                NavMeshHit closestHit;
                Vector3 spawnPosition = pathWaypoints[i].waypoints[0].position;
                if (NavMesh.SamplePosition(spawnPosition, out closestHit, 3, 1)) spawnPosition = closestHit.position;
                string enemyName = i == 0 ? "PathEnemyPreview" : "SmartEnemyPreview";
                BaseEnemy enemy = PoolManager.Instance.Spawn<BaseEnemy>(enemyName, spawnPosition, Quaternion.identity);
                enemy.SetWaypoint(pathWaypoints[i]);
                enemy.StartRunningWaypoints();
                enemyPreviews.Add(enemy);
            }
            yield return new WaitForSeconds(5);
        }
    }
    public void StartPreview()
    {
        coroutine = StartCoroutine(Preview());
    }
    public void StopPreview()
    {
        StopCoroutine(coroutine);
        for (int i = enemyPreviews.Count - 1; i >= 0; i--)
        {
            enemyPreviews[i].Die();
        }
        enemyPreviews.Clear();
    }
    public void SetSelectedTower(TowerHologram hologram)
    {
        selectedTower = hologram;
    }
    public TowerHologram GetSelectedTower()
    {
        return selectedTower;
    }
    public int GetLives()
    {
        return lives;
    }
    public int GetMoney()
    {
        return money;
    }
    public void DecreaseLives(int live)
    {
        lives -= live;
        GameUI.Instance.SetLives(Math.Max(lives, 0));
        if (lives <= 0) GameManager.Instance.onGameOver.Invoke();
    }
    public void IncreaseMoney(int mon)
    {
        money += mon;
        GameUI.Instance.SetMoney(money);
    }
    public void CallWave()
    {
        if (!isSpawning)
        {
            SpawnShuttle();
        }
    }
    public void SetAuto()
    {
        isAuto = !isAuto;
    }
    public bool IsSpawning()
    {
        return isSpawning;
    }
    private void OnEnemiesDied(BaseEnemy enemy)
    {
        enemies.Remove(enemy);
        enemy.onDead -= OnEnemiesDied;
    }
    public bool IsEnemiesLeft()
    {
        return enemies.Count > 0;
    }
    #region Save & Load
    public void Save(ref LevelSaveData data)
    {
        data.Lives = lives;
        data.Money = money;
        data.WaveIndex = waveIndex;
    }
    public void Load(LevelSaveData data)
    {
        StopAllCoroutines();
        WaveCaller.Instance.EnableCall();
        isSpawning = false;

        lives = data.Lives;
        money = data.Money;
        waveIndex = data.WaveIndex;
        GameUI.Instance.SetWave(waveIndex + 1);

        GameUI.Instance.SetLives(lives);
        GameUI.Instance.SetMoney(money);

        foreach (BaseEnemy enemy in enemies)
        {
            if (enemy != null) PoolManager.Instance.Despawn(enemy);
        }
        enemies.Clear();
    }
    #endregion
}

[System.Serializable]
public struct LevelSaveData
{
    public int Lives;
    public int Money;
    public int WaveIndex;
}