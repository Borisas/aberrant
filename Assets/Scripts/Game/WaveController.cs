
using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    public System.Action OnWaveCompleted;

    [SerializeField] private EnemySpawner _enemySpawner = null;

    private int _enemiesInWave = 0;
    private int _enemiesSpawned = 0;

    private float _spawnInterval = 0.0f;
    private float _spawnTimer = 0.0f;

    private bool _waveInProgress = false;

    private void Awake() {
    }

    public void BeginWave(int index) {
        _waveInProgress = true;
        _enemiesSpawned = 0;
        _spawnInterval = 1.0f;
        _enemiesInWave = UnityEngine.Random.Range(15, 21);
    }


    private void Update() {

        if (!_waveInProgress) return;
        
        if (GetEnemySpawnsRemaining() > 0) {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval) {
                var e = _enemySpawner.SpawnEnemy();
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

    public int GetEnemySpawnsRemaining() {
        return _enemiesInWave - _enemiesSpawned;
    }

    public bool IsInProgress() {
        return _waveInProgress;
    }
}