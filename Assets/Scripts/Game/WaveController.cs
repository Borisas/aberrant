
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class WaveController : MonoBehaviour {

    [System.Serializable]
    struct SpawnSettings {
        public int TotalWeight;
        public float Delay;

        public override string ToString() {
            return $"Spawn: {JsonWrapper.Serialize(this)}";
        }
    }


    public System.Action OnWaveCompleted;

    [SerializeField] private EnemySpawner _enemySpawner = null;

    private int _waveIndex = 0;
    private bool _waveInProgress = false;
    private float _waveDuration = 15.0f;
    private float _waveTimer = 0.0f;

    private SpawnSettings _nextSpawnSettings = new SpawnSettings();
    private float _spawnTimer = 0.0f;
    WaveControllerConfig _config;


    private void Awake() {
        _config = Database.GetInstance().Main.WaveControllerConfig;
    }

    private void Start() {
    }

    public void BeginWave(int index) {
        _waveIndex = index;
        _waveInProgress = true;
        _waveDuration = Mathf.Min(_config.WaveDurationMin + index * _config.WaveDurationPerWave, _config.WaveDurationMax);
        _waveTimer = 0.0f;

        _spawnTimer = 0.0f;
        GenerateNextSpawn(true);

        ViewWaveInfo.Open(_waveIndex);
    }

    private void Update() {

        if (!_waveInProgress) return;
        _waveTimer += Time.deltaTime;

        if (_waveTimer < _waveDuration) {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _nextSpawnSettings.Delay) {
                ExecuteSpawn();
                GenerateNextSpawn();
                _spawnTimer -= _nextSpawnSettings.Delay;
            }
        }
        else {
            if (_enemySpawner.GetLivingEnemyCount() <= 0) {
                _waveInProgress = false;
                OnWaveCompleted?.Invoke();
            }
        }
    }


    void ExecuteSpawn() {

        var eids = NTUtils.GetEnumList<EnemyId>();
        var possibleEnemies = new List<EnemyInfoMin>();
        for (int i = 0; i < eids.Count; i++) {
            var cfg = Database.GetInstance().Main.GetEnemyById(eids[i]);
            if (cfg == null || cfg.Weight > _waveIndex + 1) {
                continue;
            }
            possibleEnemies.Add(cfg.GetInfoMin());
        }

        var defaultEnemy = Database.GetInstance().Main.GetEnemyById(EnemyId.Blob).GetInfoMin();
        defaultEnemy.Weight = 1;//force weight to 1 for safety reasons


        int toSpawn = _nextSpawnSettings.TotalWeight;
        int eindex = 0;

        while (toSpawn > 0) {
            var e = possibleEnemies.Random();
            if (e.Weight > toSpawn) {
                //spawn blob
                e = defaultEnemy;
            }

            bool elite =
                _waveIndex >= _config.ElitesFromWave &&
                UnityEngine.Random.value <= _config.EliteChance &&
                _waveIndex >= e.Weight * 2;


            toSpawn -= e.Weight * (elite ? 2 : 1);

            _enemySpawner.SpawnEnemyNotNewArea(elite, e.Id);

            eindex++;
        }
    }

    void GenerateNextSpawn(bool firstSpawn = false) {

        int minimumWeight = Mathf.RoundToInt(_config.MinWeight + _waveIndex * _config.MinWeightPerWave);
        int maximumWeight = Mathf.RoundToInt(_config.MaxWeight + _waveIndex * _config.MaxWeightPerWave);

        float delayMin = _config.MinDelay;
        float delayMax = _config.MaxDelay - (float)_waveIndex * _config.MaxDelayLossPerWave;
        delayMax = Mathf.Max(delayMax, delayMin);


        if (firstSpawn) {
            // delayMin = 0.0f;
            // delayMax = 0.0f;
        }
        else {
            bool lastHigh = _nextSpawnSettings.TotalWeight >= maximumWeight * _config.HighSpawnLimit;
            if (lastHigh) {
                minimumWeight = Mathf.Max(1, Mathf.RoundToInt(minimumWeight * _config.HighSpawnReductor));
                maximumWeight = Mathf.Max(1, Mathf.RoundToInt(maximumWeight * _config.HighSpawnReductor));
                delayMin *= _config.HighSpawnDelayIncrease;
                delayMax *= _config.HighSpawnDelayIncrease;
            }

        }


        _nextSpawnSettings.Delay = UnityEngine.Random.Range(delayMin, delayMax);
        _nextSpawnSettings.TotalWeight = UnityEngine.Random.Range(minimumWeight, maximumWeight + 1);
        _enemySpawner.GenerateNewArea();
    }

    void Enemy_OnDie(Actor e) {
        e.OnDie -= Enemy_OnDie;
    }

    public float GetWaveTimeRemaining() {
        return Mathf.Max(_waveDuration - _waveTimer, 0.0f);
    }

    public bool IsInProgress() {
        return _waveInProgress;
    }

    public EnemySpawner GetEnemySpawner() => _enemySpawner;
}