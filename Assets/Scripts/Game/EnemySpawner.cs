using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    
    [SerializeField] private Transform _enemyParent = null;
    [SerializeField] private Area2D _enemySpawnArea = null;

    private List<Enemy> _livingEnemies = new List<Enemy>();
    
    //only this should spawn an enemy.
    public Enemy SpawnEnemy() {
        var p = _enemySpawnArea.GetRandomPoint();
        var cfg = Database.GetInstance().Main.GetEnemyById(EnemyId.Blob);
        var go = ObjectPool.Get(cfg.Prefab, _enemyParent);
        go.Setup(cfg);
        go.transform.position = p;
        _livingEnemies.Add(go);
        go.OnDie += Enemy_OnDie;
        return go;
    }

    private void Enemy_OnDie(Actor enemy) {
        _livingEnemies.Remove(enemy as Enemy);
    }

    public int GetLivingEnemyCount() {
        int count = 0;
        for (int i = 0; i < _livingEnemies.Count; i++) {
            if (_livingEnemies[i].IsAlive()) count++;
        }
        return count;
    }
}