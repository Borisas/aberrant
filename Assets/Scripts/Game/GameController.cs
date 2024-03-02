using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private Area2D _enemySpawnArea = null;
    [SerializeField] private Transform _enemyParent = null;
    [SerializeField] private EnemySpawner _enemySpawner = null;

    private NTUtils.Timer _spawnTimer;

    private List<Enemy> _enemies = new List<Enemy>();

    private void Awake() {
        
        _spawnTimer = new NTUtils.Timer()
            .SetOnComplete(SpawnEnemy)
            .SetLooping(true)
            .Start(1.0f)
            .Complete();
        
        _enemies = new List<Enemy>();
    }

    private void Update() {
        _spawnTimer.Run();
    }

    public void SpawnEnemy() {
        var p = _enemySpawnArea.GetRandomPoint();
        var cfg = Database.GetInstance().Main.GetEnemyById(EnemyId.Blob);
        var go = Instantiate(cfg.Prefab, _enemyParent);
        go.transform.position = p;
        go.Setup(cfg);
        _enemies.Add(go);
    }

    public List<Enemy> GetEnemies() => _enemies;
}
