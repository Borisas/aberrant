
using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

    public System.Action OnWaveCompleted;

    [SerializeField] private EnemySpawner _enemySpawner = null;

    private List<Enemy> _enemies = new List<Enemy>();
    private int _enemiesInWave = 0;
    private int _enemiesSpawned = 0;

    private float _spawnInterval = 0.0f;
    private float _spawnTimer = 0.0f;

    private bool _waveInProgress = false;

    private void Awake() {
        _enemies = new List<Enemy>();
        
    }

    public void BeginWave() {
        _enemies.Clear();
        _waveInProgress = true;
        _enemiesSpawned = 0;
        _spawnInterval = 1.0f;
        _enemiesInWave = UnityEngine.Random.Range(15, 21);
    }


    private void Update() {

        if (!_waveInProgress) return;
        
        if (GetEnemiesRemaining() > 0) {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval) {
                var e = _enemySpawner.SpawnEnemy();
                _enemies.Add(e);
                _spawnTimer -= _spawnInterval;
                _enemiesSpawned++;
            }
        }
        else {

            if (IsAllEnemiesDead()) {
                _waveInProgress = false;
                OnWaveCompleted?.Invoke();
            }
        }
    }

    public int GetEnemiesRemaining() {
        return _enemiesInWave - _enemiesSpawned;
    }

    public int GetEnemiesRemainingAlive() {

        int deadCount = 0;
        
        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[i] == null || !_enemies[i].IsAlive()) {
                deadCount++;
            }
        }

        return _enemiesInWave - deadCount;
    }

    bool IsAllEnemiesDead() {

        for (int i = 0; i < _enemies.Count; i++) {
            if (_enemies[i] == null || !_enemies[i].IsAlive()) {
                return false;
            }
        }

        return true;
    }

}