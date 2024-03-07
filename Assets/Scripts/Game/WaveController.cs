
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class WaveController : MonoBehaviour {
    private const int SPAWN_ELITES_FROM_WAVE = 3;
    
    public System.Action OnWaveCompleted;

    [SerializeField] private EnemySpawner _enemySpawner = null;

    private float _eliteChance = 0.15f;
    
    private int _enemiesInWave = 0;
    private int _enemiesSpawned = 0;

    private float _spawnInterval = 0.0f;
    private float _spawnTimer = 0.0f;

    private bool _waveInProgress = false;

    private int _waveIndex = 0;

    private float _waveDuration = 15.0f;
    private float _waveTimer = 0.0f;

    private void Awake() {
    }

    public void BeginWave(int index) {
        _waveIndex = index;
        _waveInProgress = true;
        _enemiesSpawned = 0;

        _spawnInterval = GetSpawnInterval(index);

        _waveDuration = Mathf.Min(15.0f + index * 5.0f,60.0f);
        _waveTimer = 0.0f;
        
        int min = 15 + index * 2;
        int max = 21 + index * 3;
        _enemiesInWave = UnityEngine.Random.Range(min, max);
    }

    float GetSpawnInterval(int index) {
        return Mathf.Max(0.25f, 1.0f - (float)index * 0.1f);
    }

    private void Update() {

        if (!_waveInProgress) return;

        _waveTimer += Time.deltaTime;
        
        if (_waveTimer < _waveDuration) {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval) {

                bool elite = false;
                if (_waveIndex >= SPAWN_ELITES_FROM_WAVE) {
                    elite = UnityEngine.Random.value <= _eliteChance;
                }

                var e = _enemySpawner.SpawnEnemy(elite, UnityEngine.Random.value < 0.2f ? EnemyId.Eye : EnemyId.Blob);
                e.OnDie += Enemy_OnDie;
                _spawnTimer -= _spawnInterval;
                _enemiesSpawned++;
            }
        }
        else {
            if (_enemySpawner.GetLivingEnemyCount() <= 0) {
                _waveInProgress = false;
                OnWaveCompleted?.Invoke();
            }
        }
    }

    void Enemy_OnDie(Actor e) {
        e.OnDie -= Enemy_OnDie;
    }

    public float GetWaveTimeRemaining() {
        return Mathf.Max(_waveDuration - _waveTimer,0.0f);
    }

    public bool IsInProgress() {
        return _waveInProgress;
    }

    public EnemySpawner GetEnemySpawner() => _enemySpawner;
}