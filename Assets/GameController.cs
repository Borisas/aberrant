using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private Area2D _enemySpawnArea = null;
    [SerializeField] private Transform _enemyParent = null;

    private NTUtils.Timer _spawnTimer;

    private void Awake() {
        _spawnTimer = new NTUtils.Timer();
        _spawnTimer.SetOnComplete(SpawnEnemy);
        _spawnTimer.SetLooping(true);
        _spawnTimer.Start(0.5f);
        _spawnTimer.Complete();
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
    }
}
